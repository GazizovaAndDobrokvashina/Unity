using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.NetworkSystem;

public class Player : MonoBehaviour
{
    //ID игрока
    private int idPlayer;

    private string nickName;

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

    private float angle;

    public bool ready;

    public string NickName
    {
        get { return nickName; }
    }

   

    public StreetPath GetCurrentStreetPath()
    {
        return currentStreetPath;
    }

    private void Start()
    {
        _gameCanvas = transform.Find("/Canvas").GetComponent<GameCanvas>();
    }

    private void Update()
    {
        if (!transform.position.Equals(destination))
        {
            //if(transform.rotation != Quaternion.Euler(0, angle ,0) )
            //  transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, angle ,0), Time.deltaTime*50f);
            //else
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
        GameCanvas.destination = currentStreetPath.NamePath;
    }

    public void takeResponse(bool responce)
    {
        isGonnaBeCathced = responce;
        isCheating = false;
    }

    //Корутина движения
    private IEnumerator Go()
    {
        bool tried = isCheating;
        yield return new WaitUntil(() => isCheating == false);

        if (tried && isGonnaBeCathced)
        {
            if (Random.Range(0, 2) == 1)
            {
                corutine = false;
                _gameCanvas.gameObject.GetComponent<GameController>().cathedPlayer();
                yield break;
            }
            else
            {
                isGonnaBeCathced = false;
            }
           
        }
        else if (tried)
        {
            corutine = false;
            yield break;
        }


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
                    angle = MapBuilder.Angle(transform.position, destination);
                    yield return new WaitUntil(() => transform.position == destination);
                }
                else
                {
                    destination = currentStreetPath.end;
                    angle = MapBuilder.Angle(transform.position, destination);
                    yield return new WaitUntil(() => transform.position == destination);
                }

                endFirstStep = true;
                i--;
                continue;
            }
            if (i == num - 1)
            {
                destination = MapBuilder.GetCenter(somewhere.start, somewhere.end);
                angle = MapBuilder.Angle(transform.position, destination);

                currentStreetPath = somewhere;
                yield return new WaitUntil(() => transform.position == destination);
            }
            else
            {
                if (somewhere.isBridge && transform.position.Equals(somewhere.end))
                {
                    destination = somewhere.start;
                    angle = MapBuilder.Angle(transform.position, destination);
                    yield return new WaitUntil(() => transform.position == destination);
                }
                else
                {
                    destination = somewhere.end;
                    angle = MapBuilder.Angle(transform.position, destination);
                    yield return new WaitUntil(() => transform.position == destination);
                }
            }
            currentSteps++;
        }
        corutine = false;
        if (tried && isGonnaBeCathced)
        {
            _gameCanvas.GetComponent<GameController>().nextStep();
        }
    }

    //запуск корутины движения
    public void move(StreetPath path)
    {
        if (!isMoving && !corutine)
        {
            corutine = true;
            way = _dbWork.GetWay(currentStreetPath.GetIdStreetPath(),
                path.GetIdStreetPath());
            if (currentSteps + way.Count > maxSteps && !isGonnaBeCathced)
            {
                _gameCanvas.OpenWarningWindow(this);
                isCheating = true;
            }
            StartCoroutine(Go());
        }
    }


    //конструктор игрока
    public Player(int idPlayer, string nickName, int money, bool isBankrupt, Vector3 destination)
    {
        this.nickName = nickName;
        this.idPlayer = idPlayer;
        this.money = money;
        this.isBankrupt = isBankrupt;
        this.destination = destination;
    }

    public Players GetPlayers()
    {
        Players players = new Players();
        players.IdPlayer = idPlayer;
        players.NickName = nickName;
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
        set { money = value; }
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
        set { isBankrupt = value; }
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
        this.nickName = player.NickName;
        this.isBankrupt = player.IsBankrupt;
        this.maxSteps = player.MaxSteps;
        this.money = player.Money;
        this.speed = player.Speed;

        _dbWork = Camera.main.GetComponent<DBwork>();


        this.currentStreetPath = _dbWork.GetPathById(1);
    }

    public StreetPath CurrentStreetPath
    {
        get { return currentStreetPath; }
    }

    //следующий ход, генерация ходов, выпадающих на кубике
    public void NextStep()
    {
        maxSteps = Random.Range(2, 13);
        currentSteps = 0;
    }

    public void NextStepBot()
    {
        NextStep();

        StartCoroutine(GoBot());
    }

    private IEnumerator GoBot()
    {
//        bool tried = isCheating;
//
//        if (tried && isGonnaBeCathced)
//        {
//            if (Random.Range(0, 2) == 1)
//            {
//                corutine = false;
//                GameController.cathedPlayer();
//                yield break;
//            }
//        }
//        else if (tried)
//        {
//            corutine = false;
//            yield break;
//        }


        while (currentSteps < maxSteps)
        {
            way = _dbWork.GetWayOfSteps(currentStreetPath.GetIdStreetPath(), maxSteps-currentSteps);
            
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
                        angle = MapBuilder.Angle(transform.position, destination);
                        yield return new WaitUntil(() => transform.position == destination);
                    }
                    else
                    {
                        destination = currentStreetPath.end;
                        angle = MapBuilder.Angle(transform.position, destination);
                        yield return new WaitUntil(() => transform.position == destination);
                    }

                    endFirstStep = true;
                    i--;
                    continue;
                }
                if (i == num - 1)
                {
                    destination = MapBuilder.GetCenter(somewhere.start, somewhere.end);
                    angle = MapBuilder.Angle(transform.position, destination);

                    currentStreetPath = somewhere;
                    yield return new WaitUntil(() => transform.position == destination);
                }
                else
                {
                    if (somewhere.isBridge && transform.position.Equals(somewhere.end))
                    {
                        destination = somewhere.start;
                        angle = MapBuilder.Angle(transform.position, destination);
                        yield return new WaitUntil(() => transform.position == destination);
                    }
                    else
                    {
                        destination = somewhere.end;
                        angle = MapBuilder.Angle(transform.position, destination);
                        yield return new WaitUntil(() => transform.position == destination);
                    }
                }
                currentSteps++;
            }
        }

        PathForBuy pathForBuy = _dbWork.GetPathForBuy(currentStreetPath.GetIdStreetPath());
        if(currentStreetPath.CanBuy && pathForBuy.IdPlayer == 0 && money > pathForBuy.PriceStreetPath)
            pathForBuy.Buy(this);
        
        corutine = false;
        ready = true;
//        if (tried && isGonnaBeCathced)
//        {
//            _gameCanvas.GetComponent<GameController>().nextStep();
//        }
    }

    public Players getEntity()
    {
        return new Players(idPlayer, nickName, money, destination.x, destination.z, isBankrupt);
    }
}