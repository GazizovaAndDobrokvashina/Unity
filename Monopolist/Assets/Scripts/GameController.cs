using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Player CurrentPlayer;
    private DBwork _dBwork;
    private int CountStepsInAllGame;
    private int salary = 1000;
    public Text aboutPlayerText;
    public GameObject nextStepButton;
    public static string aboutPlayer;
    

    void Start()
    {
        _dBwork = Camera.main.GetComponent<DBwork>();

        _dBwork.GetPlayerbyId(1).NextStep();
    }

    void Update()
    {
        aboutPlayerText.text = aboutPlayer;
    }
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
        {yield break;}
    }
    

    private IEnumerator GoNextStep()
    {
        checkPlayer(1);
       // gameObject.GetComponent<CanvasGroup>().interactable = false;
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
        //gameObject.GetComponent<CanvasGroup>().interactable = true;
        nextStepButton.GetComponent<CanvasGroup>().interactable = true;
        _dBwork.GetPlayerbyId(1).SetCurrentStep(true);
    }
    
    public void cathedPlayer()
    {
        //перевести плеера в суд, так как он пойман
        CurrentPlayer = _dBwork.GetPlayerbyId(1);
        Debug.Log("попался");
        CurrentPlayer.move(_dBwork.GetPathById(14));
        _dBwork.GetGovermentPath(14).GoToJail(CurrentPlayer.IdPlayer, gameObject.GetComponent<GameCanvas>());
        
        
    }

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