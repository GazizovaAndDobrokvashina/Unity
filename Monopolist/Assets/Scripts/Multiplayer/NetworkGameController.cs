using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class NetworkGameController : NetworkBehaviour
{
    //текущий активный игрок
    public NetworkPlayer CurrentPlayer;

    //ссылка на дбворк
    private NetworkDBwork _dBwork;

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

    private NetworkGameCanvas _gameCanvas;

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

        if (secondDice.GetIndexOfSurface() > -1)
            stepsForPlayer += secondDice.GetIndexOfSurface();
        CurrentPlayer.SetMaxStep(stepsForPlayer);

        aboutPlayer += "Игроку " + CurrentPlayer.NickName + " выпало ходов: " + stepsForPlayer + "\n";
    }


    //сброс истории, обновление ссылки на дбворк, бросок кубиков первого игрока
    void Start()
    {
        aboutPlayer = "";
        _dBwork = Camera.main.GetComponent<NetworkDBwork>();

        _gameCanvas = gameObject.GetComponent<NetworkGameCanvas>();
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
        if (NetworkGameCanvas.currentSteps < NetworkGameCanvas.maxSteps && !CurrentPlayer.isInJail())
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
        NetworkGameController.aboutPlayer += "Игрок " + CurrentPlayer.NickName + " пытается смухлевать" + "\n";
        _gameCanvas.OpenWarningWindow(CurrentPlayer);
        yield return new WaitWhile(() => _gameCanvas.warningWindow.activeInHierarchy);
        if (_gameCanvas.response)
        {
            if (Random.Range(0, 2) != 1)
            {
                NetworkGameController.aboutPlayer += "Игрок " + CurrentPlayer.NickName + " не попался \n";
            }
            else
            {
                NetworkGameController.aboutPlayer += "Игрок " + CurrentPlayer.NickName + " попался \n";
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

        NetworkPlayer[] players = _dBwork.GetAllPlayers();

        for (int index = 2; index < players.Length; index++)
        {
            players[index].ready = false;
            CurrentPlayer = players[index];
            if (CurrentPlayer.IsBankrupt)
                continue;
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
        NetworkPlayer ourPlayer = _dBwork.GetPlayerbyId(idPlayer);

        if (ourPlayer.GetCurrentStreetPath().canBuy)
        {
            NetworkPathForBuy path = _dBwork.GetPathForBuy(ourPlayer.GetCurrentStreetPath().GetIdStreetPath());
            if (path.IdPlayer != 0 && path.IdPlayer != idPlayer)
            {
                path.StepOnMe(idPlayer);
            }
        }
        else
        {
            NetworkGovermentPath path = _dBwork.GetGovermentPath(ourPlayer.GetCurrentStreetPath().GetIdStreetPath());
            path.StepOnMe(idPlayer);
        }

        if (ourPlayer.Money <= 0)
        {
            CheckForBankrupt(ourPlayer);
        }
    }

    public void CheckForBankrupt(NetworkPlayer player)
    {
        bool haveNotBlockedStreets = false;
        List<int> paths = _dBwork.GetMyPathes(player.IdPlayer);

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
            StartCoroutine(Bankrupting());
        }

//        else
//        {
//            player.IsBankrupt = true;
//        }
    }

    //отправка игрока в тюрьму
    public void GoToJail(int idPlayer, GameCanvas canv)
    {
        NetworkDBwork dBwork = Camera.main.GetComponent<NetworkDBwork>();
        Event newEvent = dBwork.getCourt().events[0];
        if (idPlayer == 1)
        {
            canv.ShowInfoAboutEvent(newEvent.Name + "\n" + newEvent.Info);
        }

        dBwork.GetPlayerbyId(idPlayer).InJail(3);
        dBwork.GetPlayerbyId(idPlayer).Money += newEvent.Price;
    }

    private IEnumerator Bankrupting()
    {
        aboutPlayer += "Игрок " + CurrentPlayer.NickName + " на грани банкротсва" + "\n";
        if (!CurrentPlayer.IsBot())
        {
            _gameCanvas.OpenWarningWindow(CurrentPlayer);
            yield return new WaitWhile(() => _gameCanvas.warningWindow.activeInHierarchy);
            if (_gameCanvas.response)
            {
                _gameCanvas.onButtonClickTrade(CurrentPlayer.IdPlayer);
            }
            else
            {
                aboutPlayer += "Игрок " + CurrentPlayer.NickName + " признал себя банкротом! \n Палочки вверх! \n ";
                CurrentPlayer.IsBankrupt = true;
            }
        }
        else
        {
            //логика ботов при банкротстве
        }
    }
}