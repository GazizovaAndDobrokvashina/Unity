using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using System.Security.Principal;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    //текущий активный игрок
    public Player CurrentPlayer;

    //ссылка на дбворк
    private DBwork _dBwork;

    //счетчик сделанных ходов в игре
    private int CountStepsInAllGame;

    //зарплата игроков
    private int salary = 1000;

    //история действий (текстовое поле)
    public Text aboutPlayerText;

    //кнопка следующего хода
    public GameObject nextStepButton;

    //история действий
    public static string aboutPlayer;

    //скрипт первого кубика
    public Dice firstDice;

    //скрипт второго кубика
    public Dice secondDice;
    
    //количество ходов, выпавших на кубиках
    private int stepsForPlayer;

    private Vector3 posFirstDice;

    private Vector3 posSecondDice;
    
    //корутина броска кубиков
    public IEnumerator Dices()
    {
        
        aboutPlayer += "Игрок " + CurrentPlayer.NickName + " бросает кубики \n";
        firstDice.SetPosition(posFirstDice);
        secondDice.SetPosition(posSecondDice);
        firstDice.gameObject.SetActive(true);
        secondDice.gameObject.SetActive(true);
        stepsForPlayer = 0;
        //сбрасываем индекс первого кубика
        firstDice.resetIndex();
        //сбрасываем индекс второго кубика
        secondDice.resetIndex();
        //дожидаемся ответа от первого кубика
        yield return StartCoroutine(firstDice.WaitForAllSurfaces());
        //дожидаемся ответа от второго кубика
        yield return StartCoroutine(secondDice.WaitForAllSurfaces());

        if (firstDice.GetIndexOfSurface() > -1)
            stepsForPlayer += firstDice.GetIndexOfSurface();

        if (secondDice.GetIndexOfSurface() > -1 )
            stepsForPlayer += secondDice.GetIndexOfSurface();
        CurrentPlayer.SetMaxStep(stepsForPlayer);
        
        aboutPlayer += "Игроку " + CurrentPlayer.NickName + " выпало ходов: " + stepsForPlayer + "\n";
        
    }

      
    //сброс истории, обновление ссылки на дбворк, бросок кубиков первого игрока
    void Start()
    {
        aboutPlayer = "";
        _dBwork = Camera.main.GetComponent<DBwork>();

        _dBwork.GetPlayerbyId(1).NextStep();
        CurrentPlayer = _dBwork.GetPlayerbyId(1);

        posFirstDice = firstDice.transform.position;
        posSecondDice = secondDice.transform.position;
    }

    //вывод истории на экран
    void Update()
    {
        aboutPlayerText.text = aboutPlayer;
    }

    //передача хода между игроками, если игрок сделал достаточно ходов или, если недостаточно, то выяснить у игрока хочет ли он сжульничать
    public void nextStep()
    {
        if (GameCanvas.currentSteps < GameCanvas.maxSteps && !CurrentPlayer.isInJail())
        { 
            StartCoroutine(Cheating());
        }
        else
        {
            StartCoroutine(GoNextStep());
        }
    }

    //ожидание ответа от игрока, действительно ли он хочет сжульничать
    private IEnumerator Cheating()
    {
        GameController.aboutPlayer += "Игрок " + CurrentPlayer.NickName + " пытается смухлевать" + "\n";
        GameCanvas gameCanvas = gameObject.GetComponent<GameCanvas>();
        gameCanvas.OpenWarningWindow(CurrentPlayer);
        yield return new WaitWhile(() => gameCanvas.warningWindow.activeInHierarchy);
        if (gameCanvas.response)
        {
            if (Random.Range(0, 2) != 1)
            {
                GameController.aboutPlayer += "Игрок " + CurrentPlayer.NickName + " не попался \n";
            }
            else
            {
                GameController.aboutPlayer += "Игрок " + CurrentPlayer.NickName + " попался \n";
                cathedPlayer();
            }
            StartCoroutine(GoNextStep());
        }
        else
        {
            yield break;
        }
    }

    //обход игроков, выдача им зарплат
    private IEnumerator GoNextStep()
    {
        checkPlayer(1);
        _dBwork.GetPlayerbyId(1).SetCurrentStep(false);
        firstDice.gameObject.SetActive(false);
        secondDice.gameObject.SetActive(false);
        CurrentPlayer.SetMaxStep(0);
        nextStepButton.GetComponent<CanvasGroup>().interactable = false;
        CountStepsInAllGame++;

        if (CountStepsInAllGame % 10 == 0)
            _dBwork.GetPlayerbyId(1).Money += salary;

        Player[] players = _dBwork.GetAllPlayers();

        for (int index = 2; index < players.Length; index++)
        {
            players[index].ready = false;
            CurrentPlayer = players[index];
            if (CountStepsInAllGame % 10 == 0)
                players[index].Money += salary;
            if (!CurrentPlayer.isInJail())
            {
                yield return StartCoroutine(Dices());
            }
            else
            {
                aboutPlayer += "Прямое включение из тюрьмы: Ход игрока " + CurrentPlayer.NickName + "\n";
            }
            players[index].NextStep();
            yield return new WaitUntil(() => players[index].ready);
            checkPlayer(index);
        }

        CurrentPlayer = _dBwork.GetPlayerbyId(1);
        if (!CurrentPlayer.isInJail())
        {
            gameObject.GetComponent<GameCanvas>().OpenThrowDiceButton();
            yield return new WaitUntil(() => secondDice.gameObject.activeInHierarchy);
            yield return new WaitUntil(() => secondDice.GetIndexOfSurface() > 0);
        }
        else
        {
            aboutPlayer += "Прямое включение из тюрьмы: Ход игрока " + CurrentPlayer.NickName + "\n";
        }
        _dBwork.GetPlayerbyId(1).NextStep();
        nextStepButton.GetComponent<CanvasGroup>().interactable = true;
        _dBwork.GetPlayerbyId(1).SetCurrentStep(true);
    }

    //перевод игрока в место заключения под стражу, если он был поймат при жульничестве
    public void cathedPlayer()
    {
        //перевести плеера в суд, так как он пойман
        //CurrentPlayer = _dBwork.GetPlayerbyId(1);
        CurrentPlayer.move(_dBwork.getCourt());
        GoToJail(CurrentPlayer.IdPlayer, gameObject.GetComponent<GameCanvas>());
    }

    //если игрок закончил ход на чужой улице, то с него снимается плата в пользу игрока, владеющего этой улицей
    void checkPlayer(int idPlayer)
    {
        Player ourPlayer = _dBwork.GetPlayerbyId(idPlayer);

        if (ourPlayer.GetCurrentStreetPath().canBuy)
        {
            PathForBuy path = _dBwork.GetPathForBuy(ourPlayer.GetCurrentStreetPath().GetIdStreetPath());
            if (path.IdPlayer != 0 && path.IdPlayer != idPlayer)
            {
                path.StepOnMe(idPlayer);
            }
        }
        else
        {
            GovermentPath path = _dBwork.GetGovermentPath(ourPlayer.GetCurrentStreetPath().GetIdStreetPath());
            path.StepOnMe(idPlayer);
        }

        //проверка баланса игрока, если он меньше нуля, то проверить, может ли игрок что-то заложить, иначе он банкрот
        if (ourPlayer.Money < 0)
        {
            bool haveNotBlockedStreets = false;
            List<int> paths = _dBwork.GetMyPathes(ourPlayer.IdPlayer);

            foreach (int path in paths)
            {
                if (!_dBwork.GetPathForBuy(path).IsBlocked)
                {
                    haveNotBlockedStreets = true;
                    break;
                }
            }

            if (haveNotBlockedStreets)
            {
                //запуск корутины ожидания ответа от игрока: он хочет сдаться или заложить имеющуюся недвижимость
            }
            else
            {
                ourPlayer.IsBankrupt = true;
            }
        }
    }

    //отправка игрока в тюрьму
    public void GoToJail(int idPlayer, GameCanvas canv)
    {
        DBwork dBwork = Camera.main.GetComponent<DBwork>();
        Event newEvent = dBwork.getCourt().events[0];
        if (idPlayer == 1)
        {
            canv.ShowInfoAboutEvent(newEvent.Name + "\n" + newEvent.Info);
        }
        dBwork.GetPlayerbyId(idPlayer).InJail(3);
        dBwork.GetPlayerbyId(idPlayer).Money += newEvent.Price;
    }
}