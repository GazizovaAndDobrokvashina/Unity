using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
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

    //сброс истории, обновление ссылки на дбворк, бросок кубиков первого игрока
    void Start()
    {
        aboutPlayer = "";
        _dBwork = Camera.main.GetComponent<DBwork>();

        _dBwork.GetPlayerbyId(1).NextStep();
    }

    //вывод истории на экран
    void Update()
    {
        aboutPlayerText.text = aboutPlayer;
    }

    //передача хода между игроками, если игрок сделал достаточно ходов или, если недостаточно, то выяснить у игрока хочет ли он сжульничать
    public void nextStep()
    {
        if (GameCanvas.currentSteps < GameCanvas.maxSteps)
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
        GameCanvas gameCanvas = gameObject.GetComponent<GameCanvas>();
        gameCanvas.OpenWarningWindow(CurrentPlayer);
        yield return new WaitWhile(() => gameCanvas.warningWindow.activeInHierarchy);
        if (gameCanvas.response)
        {
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
        nextStepButton.GetComponent<CanvasGroup>().interactable = false;
        CountStepsInAllGame++;

        if (CountStepsInAllGame % 10 == 0)
            _dBwork.GetPlayerbyId(1).Money += salary;

        Player[] players = _dBwork.GetAllPlayers();

        for (int index = 2; index < players.Length; index++)
        {
            players[index].ready = false;
            if (CountStepsInAllGame % 10 == 0)
                players[index].Money += salary;
            players[index].NextStep();
            yield return new WaitUntil(() => players[index].ready);
            checkPlayer(index);
        }

        _dBwork.GetPlayerbyId(1).NextStep();
        nextStepButton.GetComponent<CanvasGroup>().interactable = true;
        _dBwork.GetPlayerbyId(1).SetCurrentStep(true);
    }

    //перевод игрока в место заключения под стражу, если он был поймат при жульничестве
    public void cathedPlayer()
    {
        //перевести плеера в суд, так как он пойман
        CurrentPlayer = _dBwork.GetPlayerbyId(1);
        CurrentPlayer.move(_dBwork.GetPathById(14));
        _dBwork.GetGovermentPath(14).GoToJail(CurrentPlayer.IdPlayer, gameObject.GetComponent<GameCanvas>());
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

        if (ourPlayer.Money < 0)
        {
            ourPlayer.IsBankrupt = true;
        }
    }
}