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

    private int StepsInJail;

    private bool alreadyCheat;


    public string NickName
    {
        get { return nickName; }
    }

    public GameCanvas GetGameCanvas()
    {
        return _gameCanvas;
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
        if (alreadyCheat)
        {
            _gameCanvas.ShowInfoAboutEvent("вы уже мухлевали на этом ходе :(");
            corutine = false;
            yield break;
        }

        bool tried = isCheating;
        yield return new WaitUntil(() => isCheating == false);

        if (tried && isGonnaBeCathced)
        {
            if (Random.Range(0, 2) != 1)
            {
                GameController.aboutPlayer += "Игрок " + NickName + " не попался \n";
                alreadyCheat = true;
                isGonnaBeCathced = false;
            }
            else
            {
                GameController.aboutPlayer += "Игрок " + NickName + " попался \n";
                corutine = false;
                _gameCanvas.gameObject.GetComponent<GameController>().cathedPlayer();
                yield break;
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
        if (StepsInJail == 0)
        {
            if (!isMoving && !corutine)
            {
                corutine = true;
                way = _dbWork.GetWay(currentStreetPath.GetIdStreetPath(),
                    path.GetIdStreetPath());
                Debug.Log(currentStreetPath.GetIdStreetParent());
                Debug.Log(path.GetIdStreetPath());
                if (currentSteps + way.Count > maxSteps && !isGonnaBeCathced && !alreadyCheat)
                {
                    GameController.aboutPlayer += "Игрок " + NickName + " пытается смухлевать" + "\n";
                    _gameCanvas.OpenWarningWindow(this);
                    isCheating = true;
                }
                StartCoroutine(Go());
            }
        }
        else
        {
            _gameCanvas.ShowInfoAboutEvent("Вы заключены под стражу" + "\n" + "Осталось ходов: " + StepsInJail);
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


        this.currentStreetPath = findMyPath(destination);
    }

    private StreetPath findMyPath(Vector3 vector3)
    {
        foreach (StreetPath streetPath in _dbWork.GetAllPaths())
        {
            if (streetPath.GetIdStreetPath() == 0)
                continue;
            //if (vector3.Equals(MapBuilder.GetCenter(streetPath.start, streetPath.end)))
            //Vector3 center = MapBuilder.GetCenter(streetPath.start, streetPath.end);
            //if((int)vector3.x == (int)center.x && (int)vector3.z == (int)center.z)  
            Vector3 pos = streetPath.transform.position;
            if((int)pos.x == (int)vector3.x && (int)pos.z == (int)vector3.z)
            return streetPath;
        }
        return _dbWork.GetPathById(1);
    }

    public StreetPath CurrentStreetPath
    {
        get { return currentStreetPath; }
    }

    //следующий ход, генерация ходов, выпадающих на кубике
    public void NextStep()
    {
        if(idPlayer == 1) {
            GameController.aboutPlayer = "";
            
        }

        alreadyCheat = false;
        if (StepsInJail > 0)
        {
            StepsInJail--;
            maxSteps = 0;
        }

        if (StepsInJail == 0)
        {
            maxSteps = Random.Range(2, 13);
        }
        GameController.aboutPlayer += "Игроку " + NickName + " выпало ходов: " + maxSteps + "\n";
        currentSteps = 0;
    }

    public bool isInJail()
    {
        return StepsInJail > 0;
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
            way = _dbWork.GetWayOfSteps(currentStreetPath.GetIdStreetPath(), maxSteps - currentSteps);

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

        GameController.aboutPlayer += "Игрок " + NickName + " пришел на " + currentStreetPath.namePath + "\n";
        PathForBuy pathForBuy = _dbWork.GetPathForBuy(currentStreetPath.GetIdStreetPath());
        if (currentStreetPath.CanBuy && pathForBuy.IdPlayer == 0 && money > pathForBuy.PriceStreetPath)
        {
            GameController.aboutPlayer += "Игрок " + NickName + " купил " + currentStreetPath.namePath + "\n";
            pathForBuy.Buy(this);
        }

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

    //если игрок попадается на жульничестве, то он не может двигаться с клетки суда несколько ходов
    public void InJail(int steps)
    {
        StepsInJail = steps;
    }
}