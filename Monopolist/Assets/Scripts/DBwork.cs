using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DBwork : MonoBehaviour
{
    //массив игроков
    private Player[] players;

    //массив зданий
    private Build[] builds;

    //масссив улиц
    private Street[] streets;

    //массив частей улиц
    private StreetPath[] paths;

    private List<PathForBuy> pathForBuys;

    private List<GovermentPath> govermentPaths;

    //ссылка на DataService
    private DataService dataService;

    //список всех путей между улицами
    private Ways ways;

    private List<string> names;

    //название текущего города
    private string nameOfTown;

    void Start()
    {
        names = new List<string>();
        names.Add("Равшан");
        names.Add("Джамшут");
        names.Add("Мафусаил");
        names.Add("Инокентий");
        names.Add("Геннадий");
        names.Add("Ариэль");
        names.Add("Алтынбек");
        names.Add("Коловрат");
        names.Add("Джаник");
        names.Add("Марфа");
        names.Add("Бадигулжамал");
        names.Add("Дурия");
        names.Add("Антуанетта");
        names.Add("Каламкас");
        names.Add("Еркежан");
        names.Add("Жумабике");
        
    }

    public void DBStart()
    {
        dataService = new DataService("Monopolist.db");
        if (!dataService.IsExist())
            dataService.CreateDB();
    }

    public void SetGameDB(string dbName)
    {
        dataService = new DataService(dbName);
        GetEverithing();
        nameOfTown = dbName.Substring(0, dbName.IndexOf("_") - 1);
    }

    //возврат части улицы исходя из её координат
    public StreetPath GetPathByCoordinates(Vector3 coordinate)
    {
        foreach (StreetPath path in paths)
        {
            if (path.GetIdStreetPath() != 0 &&
                (coordinate.Equals(path.end) || (path.isBridge && coordinate.Equals(path.start))))
            {
                return path;
            }
        }
        return null;
    }

    //заполнение массивов игроков, улиц, частей улиц и зданий, исходя из данных в базе данных
    public void GetEverithing()
    {
        players = new Player[dataService.getPlayers().Count + 1];
        builds = new Build[dataService.getBuilds().Count + 1];
        streets = new Street[dataService.getStreets().Count + 1];
        paths = new StreetPath[dataService.getStreetPaths().Count + 1];
        pathForBuys = new List<PathForBuy>();
        govermentPaths = new List<GovermentPath>();

        foreach (Streets streetse in dataService.getStreets())
        {
            List<StreetPaths> streetPathses = dataService.getAllPathsOfStreet(streetse.IdStreet);
            int[] pathses = new int[streetPathses.Count];

            int k = 0;
            foreach (StreetPaths streetPathse in streetPathses)
            {
                PathsForBuy ifExist = dataService.getPathForBuyById(streetPathse.IdStreetPath);

                if (ifExist != null)
                {
                    List<Builds> buildses = dataService.getBuildsOnTheStreet(streetPathse.IdStreetPath);
                    int[] buildes = new int[buildses.Count];

                    int i = 0;
                    foreach (Builds buildse in buildses)
                    {
                        builds[buildse.IdBuild] = buildse.getBuild();
                        buildes[i] = buildse.IdBuild;
                        i++;
                        Debug.Log(buildse.NameBuild);
                    }

                    paths[streetPathse.IdStreetPath] = ifExist.GetPathForBuy(streetPathse, buildes);
                    pathForBuys.Add(ifExist.GetPathForBuy(streetPathse, buildes));
                }
                else
                {
                    List<Events> eventses = dataService.getEventsOnTheStreet(streetPathse.IdStreetPath);
                    Event[] events = new Event[eventses.Count];

                    int j = 0;
                    foreach (Events eventse in eventses)
                    {
                        events[j] = eventse.GetEvent();
                        j++;
                    }

                    paths[streetPathse.IdStreetPath] = streetPathse.GetGovermentPath(events);
                    govermentPaths.Add(streetPathse.GetGovermentPath(events));
                }

                pathses[k] = streetPathse.IdStreetPath;
            }

            streets[streetse.IdStreet] = streetse.GetStreet(pathses);
        }

        foreach (Players player in dataService.getPlayers())
        {
            players[player.IdPlayer] = player.GetPlayer();
        }
        players[0] = new Player(0, "", 0, true, Vector3.zero);
        streets[0] = new Street(0, "", "", new int[1]);
        paths[0] = new StreetPath(0, "", 0, 0, Vector3.zero, Vector3.zero, false);
        builds[0] = new Build(0,"", "", 0, 0, false);
    }


    //помещение камеры в поле DontDestroyOnLoad для перенесения информации из главного меню в саму игру
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        transform.position = new Vector3(5.63f, 0.43f, -5.63f);
        transform.localEulerAngles = new Vector3(0, -90, 0);
    }

    //возврат массива частей улиц
    public StreetPath[] GetAllPaths()
    {
        return paths;
    }

    public Build[] GetAllBuilds()
    {
        return builds;
    }

    //сохранение игры
    public void SaveGame()
    {
    }

    //сохранение игры как новый файл
    public void SaveGameAsNewFile(string newName)
    {
    }

    //возвращение игрока по его айдишнику
    public Player GetPlayerbyId(int idPlayer)
    {
        return players[idPlayer];
    }

    //Создание новой игры (дописать для онлайна и разых городов)
    public void CreateNewGame(int countOfPlayers, int startMoney, string NameOfGame, bool online, string nameOfTown, string nickName)
    {
        if (NameOfGame.Length != 0 && !NameOfGame.EndsWith(".db"))
        {
            dataService = new DataService(nameOfTown + "_" + NameOfGame + ".db");
        }
        else if (NameOfGame.Length != 0 && NameOfGame.EndsWith(".db"))
        {
            dataService = new DataService(nameOfTown + "_" + NameOfGame);
        }
        else
        {
            dataService = new DataService(nameOfTown + "_Firstgame.db");
        }

        if (!dataService.IsExist())
            dataService.CreateDB();

        GetEverithing();
        players = new Player[countOfPlayers + 1];
        for (int i = 1; i < countOfPlayers + 1; i++)
        {
            Player player;
            if (i==1)
                player = new Player(i, nickName, startMoney, false, MapBuilder.GetCenter(paths[1].start, paths[1].end));
            else 
                player = new Player(i, names[Random.Range(0, names.Count)], startMoney, false, MapBuilder.GetCenter(paths[1].start, paths[1].end));
            players[i] = player;
            dataService.AddPlayer(player);
        }
        
        
        GetEverithing();

        this.nameOfTown = nameOfTown;
    }

    //возврат массива игроков
    public Player[] GetAllPlayers()
    {
        return players;
    }

    //возврат очереди частей улиц между начальной и конечной точкой
    public Queue<int> GetWay(int startId, int endId)
    {
        return ways.Queues[startId, endId];
    }

    //создание массива путей из одной точки в другую, исходя из названия города и его частей улиц
    public void createWays()
    {
        ways = new Ways(nameOfTown, paths);
    }

    //обновить данные игрока
    public void updatePlayer(Player player)
    {
        players[player.IdPlayer] = player;
    }

    public void updateBuild(Build build)
    {
        builds[build.IdBuild] = build;
    }

    //обновить данные о части улицы
    public void updatePath(StreetPath path)
    {
        paths[path.GetIdStreetPath()] = path;
        
    }
    

    public void updatePath(PathForBuy path)
    {
        paths[path.GetIdStreetPath()] = path;
        for (int i=0; i<pathForBuys.Count; i++ )
        {
            if (pathForBuys[i].GetIdStreetPath() == path.GetIdStreetPath())
                pathForBuys[i] = path;
        }
    }

    public void UpdatePath(GovermentPath path)
    {
        paths[path.GetIdStreetPath()] = path;
        for (int i=0; i<govermentPaths.Count; i++ )
        {
            if (govermentPaths[i].GetIdStreetPath() == path.GetIdStreetPath())
                govermentPaths[i] = path;
        }
    }

    //возврат части улицы по её айдишнику
    public StreetPath GetPathById(int id)
    {
        return paths[id];
    }


    public PathForBuy GetPathForBuy(int id)
    {
        if (paths[id].canBuy)
        {
            foreach (PathForBuy pathForBuy in pathForBuys)
            {
                if (pathForBuy.GetIdStreetPath() == id)
                    return pathForBuy;
            }
        }
            return null;
        
            
    }


    public GovermentPath GetGovermentPath(int id)
    {
        if (!paths[id].canBuy)
        {
            foreach (GovermentPath govermentPath in govermentPaths)
            {
                if (govermentPath.GetIdStreetPath() == id)
                    return govermentPath;
            }
        }

        return null;
    }

    //возврат зданий по айдишнику улицы, на которой они находятся (дописать метод нормально)
    public Build[] GetBuildsForThisPath(int idPath)
    {
        List<Build> buildes = new List<Build>();

        foreach (Build build in this.builds)
        {
            if (build.IdStreetPath == idPath)
            {
                buildes.Add(build);
            }
        }
        
        return buildes.ToArray();
    }
}