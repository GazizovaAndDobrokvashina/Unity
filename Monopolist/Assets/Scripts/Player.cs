using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //ID игрока
    private int idPlayer;

    //деньги игрока
    private int money;

    //количество ходов, выпавших на кубике
    private int maxSteps;

    //сколько ходов уже сделал игрок
    private int currentSteps;

    //банкрот ли игрок
    private bool isBankrupt;

    //положение игрока на карте
    private Vector3 destination;

    //скорость передвижения
    public float speed = 2f;

    //ссылка на ДБворк
    private DBwork _dbWork;

    //движется ли грок
    private bool isMoving = false;
    
    //запущена ли корутина
    private bool corutine = false;

    //улица, на которой находится игрок
    private StreetPath currentStreetPath;

    //путь от одной улицы к другой
    private Queue<int> way;
    
    //пытается ли считерить игрок
    private bool isCheating;
    
    //будет ли игрок пойман прb попытке считерить
    private bool isGonnaBeCathced;

    private GameCanvas _gameCanvas;

    private void Start()
    {
        _gameCanvas = transform.Find("/Canvas").GetComponent<GameCanvas>();
    }
    private void Update()
    {
        if (!transform.position.Equals(destination))
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        if (idPlayer != 1)
            return;

        GameCanvas.currentSteps = currentSteps;
        GameCanvas.maxSteps = maxSteps;
        GameCanvas.money = money;
        //переделать на нормальное название, когда появится
        GameCanvas.destination = currentStreetPath.name;
    }

    public void takeResponse(bool responce)
    {
        isCheating = false;
        isGonnaBeCathced = responce;
    }
    //Корутина движения
    private IEnumerator Go()
    {
//        yield return new WaitUntil(() => isCheating == false);
//
//        if (isGonnaBeCathced)
//        {
//            if (Random.Range(0, 2) == 1)
//            {
//                corutine = false;
//                GameController.cathedPlayer();
//                yield break;
//            } else
//            {
//                isGonnaBeCathced = false;
//            }
//        }
        
        
        
        bool endFirstStep = false;
        int num = way.Count;
        StreetPath somewhere = null;
        for (int i = 0; i < num; i++)
        {
            if (i != 0)
                somewhere = _dbWork.GetPathById(way.Dequeue());

            if (i == 0 && !endFirstStep)
            {
                somewhere = _dbWork.GetPathById(way.Dequeue());
                if (currentStreetPath.isBridge &&
                    (currentStreetPath.start.Equals(somewhere.start) ||
                     currentStreetPath.start.Equals(somewhere.end)))
                {
                    destination = currentStreetPath.start;
                    yield return new WaitUntil(() => transform.position == destination);
                }
                else
                {
                    destination = currentStreetPath.end;
                    yield return new WaitUntil(() => transform.position == destination);
                }

                endFirstStep = true;
                i--;
                continue;
            }
            if (i == num - 1)
            {
                destination = MapBuilder.GetCenter(somewhere.start, somewhere.end);
                yield return new WaitUntil(() => transform.position == destination);
            }
            else
            {
                if (somewhere.isBridge && transform.position.Equals(somewhere.end))
                {
                    destination = somewhere.start;
                    yield return new WaitUntil(() => transform.position == destination);
                }
                else
                {
                    destination = somewhere.end;
                    yield return new WaitUntil(() => transform.position == destination);
                }
            }
            currentSteps++;
        }
        corutine = false;
    }

    //запуск корутины движения
    public void move(StreetPath path)
    {
        if (!isMoving && !corutine)
        {
            corutine = true;
            way = _dbWork.GetWay(currentStreetPath.GetIdStreetPath(),
                path.GetIdStreetPath());
            if (currentSteps + way.Count > maxSteps)
            {    _gameCanvas.OpenWarningWindow(this);
                isCheating = true;
            }
            StartCoroutine(Go());

            currentStreetPath = path;
        }
    }


    //конструктор игрока
    public Player(int idPlayer, int money, bool isBankrupt, Vector3 destination)
    {
        this.idPlayer = idPlayer;
        this.money = money;
        this.isBankrupt = isBankrupt;
        this.destination = destination;
    }

    public Players GetPlayers()
    {
        Players players = new Players();
        players.IdPlayer = idPlayer;
        players.CoordinateX = destination.x;
        players.CoordinateY = destination.z;
        players.IsBankrupt = isBankrupt;
        players.Money = money;
        return players;
    }

    //возврат айдишника игрока
    public int IdPlayer
    {
        get { return idPlayer; }
    }

    //возврат количества денег игрока
    public int Money
    {
        get { return money; }
    }

    //возврат количества ходов, выпавших игроку
    public int MaxSteps
    {
        get { return maxSteps; }
    }

    //возврат ходов, уже проделанных игроком
    public int CurrentSteps
    {
        get { return currentSteps; }
    }

    //возврат банкрот ли игрок
    public bool IsBankrupt
    {
        get { return isBankrupt; }
    }

    //возврат положения игрока на карте
    public Vector3 Destination
    {
        get { return destination; }
    }

    //Возврат скорости игрока
    public float Speed
    {
        get { return speed; }
    }


    public void GetData(Player player)
    {
        this.currentSteps = player.CurrentSteps;
        this.destination = player.Destination;
        this.idPlayer = player.IdPlayer;
        this.isBankrupt = player.IsBankrupt;
        this.maxSteps = player.MaxSteps;
        this.money = player.Money;
        this.speed = player.Speed;

        _dbWork = Camera.main.GetComponent<DBwork>();


        this.currentStreetPath = _dbWork.GetPathById(1);
    }

    //следующий ход, генерация ходов, выпадающих на кубике
    public void NextStep()
    {
        maxSteps = Random.Range(2, 13);
        currentSteps = 0;
    }
}