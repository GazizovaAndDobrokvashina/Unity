using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

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

    //список участков, доступных для продажи
    private List<PathForBuy> pathForBuys;

    //список участков, принадлежащих городу
    private List<GovermentPath> govermentPaths;

    //ссылка на DataService
    private DataService dataService;

    //список всех путей между улицами
    private Ways ways;

    private List<string> names;

    //название текущего города
    private string nameOfTown;

    //название игры
    private string nameOfGane;


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
        names.Add("Жасмин");
        names.Add("Джаник");
        names.Add("Марфа");
        names.Add("Бадигулжамал");
        names.Add("Дурия");
        names.Add("Антуанетта");
        names.Add("Каламкас");
        names.Add("Еркежан");
        names.Add("Рапунцель");
        names.Add("Жумабике");
        names.Add("Екакий");
        names.Add("Петруша");
        names.Add("Белоснежка");
        names.Add("Ньярлатотеп");
        names.Add("Орозалы");
        names.Add("Бабаназар");
        names.Add("Орозалы Бабаназарович");
        names.Add("Гундермурд Сигурдфлордбрадсен");
    }

    public void SetGameDB(string dbName)
    {
        dataService = new DataService(dbName);
        GetEverithing();
        nameOfTown = dbName.Substring(0, dbName.IndexOf("_"));
        nameOfGane = dbName.Substring(dbName.IndexOf("_") + 1);
    }

    //получить здание по идентификатору
    public Build GetBuild(int id)
    {
        return builds[id];
    }

    //получить монополию по её идентификатору
    public Street getStreetById(int id)
    {
        return streets[id];
    }

    //принадлежат ли все участки в монополии одному игроку
    public bool isAllPathsMine(int buildId, int playerId)
    {
        foreach (int i in getStreetById(GetPathById(GetBuild(buildId).IdStreetPath).GetIdStreetParent()).Paths)
        {
            if (GetPathById(i).canBuy && GetPathForBuy(i).IdPlayer != playerId)
                return false;
        }

        return true;
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
                    }

                    paths[streetPathse.IdStreetPath] = ifExist.GetPathForBuy(streetPathse, buildes);
                    pathForBuys.Add(ifExist.GetPathForBuy(streetPathse, buildes));
                }
                else
                {
                    List<Events> eventses = dataService.getEventsOnTheStreet(streetPathse.IdStreetPath);
                    Event[] events = new Event[eventses.Count];

                    int j = 0;
                    if (!streetse.AboutStreet.Contains("суд"))
                    {
                        events = new Event[eventses.Count + 1];
                        j++;
                    }

                    foreach (Events eventse in eventses)
                    {
                        events[j] = eventse.GetEvent();
                        j++;
                    }

                    paths[streetPathse.IdStreetPath] = streetPathse.GetGovermentPath(events);
                    govermentPaths.Add(streetPathse.GetGovermentPath(events));
                }

                pathses[k] = streetPathse.IdStreetPath;
                k++;
            }

            streets[streetse.IdStreet] = streetse.GetStreet(pathses);
        }

        foreach (Players player in dataService.getPlayers())
        {
            players[player.IdPlayer] = player.GetPlayer();
        }

        players[0] = new Player(0, "никто", 0, true, true, Vector3.zero);
        streets[0] = new Street(0, "", "", new int[1]);
        paths[0] = new StreetPath(0, "", 0, 0, Vector3.zero, Vector3.zero, false);
        builds[0] = new Build(0, "", "", 0, 0, false, 0, 0);
    }


    //помещение камеры в поле DontDestroyOnLoad для перенесения информации из главного меню в саму игру
    private void Awake()
    {
#if UNITY_EDITOR
        Directory.CreateDirectory(@"Assets\SavedGames");
        Directory.CreateDirectory(@"Assets\StreamingAssets");
#else
        Directory.CreateDirectory(Application.persistentDataPath + @"\SavedGames");
        Directory.CreateDirectory(Application.persistentDataPath + @"\StreamingAssets");
#endif

        DontDestroyOnLoad(gameObject);
        transform.position = new Vector3(5.63f, 0.43f, -5.63f);
        transform.localEulerAngles = new Vector3(0, -90, 0);
    }

    //возврат массива частей улиц
    public StreetPath[] GetAllPaths()
    {
        return paths;
    }

    //получить все здания в игре
    public Build[] GetAllBuilds()
    {
        return builds;
    }

    //сохранение игры
    public void SaveGame()
    {
        for (int i = 1; i < players.Length; i++)
        {
            dataService.UpdateObject(players[i].getEntity());
        }

        for (int i = 1; i < streets.Length; i++)
        {
            dataService.UpdateObject(streets[i].getEntity());
        }

        for (int i = 1; i < paths.Length; i++)
        {
            dataService.UpdateObject(paths[i].getEntity());
        }

        for (int i = 1; i < pathForBuys.Count; i++)
        {
            dataService.UpdateObject(pathForBuys[i].GetEntityForBuy());
        }

        for (int i = 1; i < builds.Length; i++)
        {
            dataService.UpdateObject(builds[i].getEntity());
        }
    }

    //сохранение игры как новый файл
    public void SaveGameAsNewFile(string newName)
    {
        SaveGame();
        string currentGame;
        string newGame;
#if UNITY_EDITOR
        currentGame = @"Assets\SavedGames\" + nameOfTown + "_" + nameOfGane;
        newGame = @"Assets\SavedGames\" + nameOfTown + "_" + newName + ".db";
        //DirectoryInfo dir = new DirectoryInfo(@"Assets\SavedGames\" +  );
#else
        currentGame = Application.persistentDataPath +@"/SavedGames/"+ nameOfTown + "_" + nameOfGane ;
        newGame = Application.persistentDataPath +@"/SavedGames/"+ nameOfTown + "_" + newName + ".db";
        //DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath +"/SavedGames/"+ nameFolder);
#endif

        File.Copy(currentGame, newGame, true);
    }

    //возвращение игрока по его айдишнику
    public Player GetPlayerbyId(int idPlayer)
    {
        return players[idPlayer];
    }

    //Создание новой игры (дописать для онлайна и разых городов)
    public void CreateNewGame(int countOfPlayers, int startMoney, string NameOfGame, bool online, string nameOfTown,
        string nickName)
    {
        
#if UNITY_EDITOR
        File.Copy(@"Assets\Resources\" + nameOfTown, @"Assets\SavedGames\" + nameOfTown + "_" + NameOfGame + "_0.db");
#else
        File.Copy(Application.persistentDataPath + @"\Resources\" + nameOfTown, Application.persistentDataPath +@"/SavedGames/" + nameOfTown + "_" + NameOfGame + "_0.db");
#endif
        
//        if (string.IsNullOrEmpty(nameOfTown))
//        {
//            nameOfTown = "Monopolist.db";
//        }
//
//        if (NameOfGame.Length != 0 && !NameOfGame.EndsWith(".db"))
//        {
//            dataService = new DataService(nameOfTown + "_" + NameOfGame + ".db");
//            nameOfGane = NameOfGame + ".db";
//        }
//        else if (NameOfGame.Length != 0 && NameOfGame.EndsWith(".db"))
//        {
//            dataService = new DataService(nameOfTown + "_" + NameOfGame);
//            nameOfGane = NameOfGame;
//        }
//        else
//        {
//            dataService = new DataService(nameOfTown + "_Firstgame.db");
//            nameOfGane = "Firstgame.db";
//        }
//
//        if (!dataService.IsExist())
//            dataService.CreateDB(nameOfTown);
        
        dataService = new DataService(nameOfTown + "_" + NameOfGame + "_0.db");
        nameOfGane = NameOfGame + "_0.db";

        GetEverithing();
        players = new Player[countOfPlayers + 1];
        for (int i = 1; i < countOfPlayers + 1; i++)
        {
            Player player;
            if (i == 1)
                player = new Player(i, nickName, startMoney, false, false,
                    MapBuilder.GetCenter(paths[1].start, paths[1].end));
            else
                player = new Player(i, names[Random.Range(0, names.Count)], startMoney, false, true,
                    MapBuilder.GetCenter(paths[1].start, paths[1].end));
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

    //получить список идентификаторов улиц, на которые можно пойти
    public List<int> GetPossibleEnds(int startId, int steps)
    {
        List<int> queue = new List<int>();

        for (int i = 1; i < paths.Length; i++)
        {
            if (ways.Queues[startId, i].Count == steps)
                queue.Add(i);
        }


        return queue;
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

    //обновить данные о здании
    public void updateBuild(Build build)
    {
        builds[build.IdBuild] = build;
    }

    //обновить данные о части улицы
    public void updatePath(StreetPath path)
    {
        paths[path.GetIdStreetPath()] = path;
    }

    //обновить данные об участке для продажи
    public void updatePath(PathForBuy path)
    {
        paths[path.GetIdStreetPath()] = path;
        for (int i = 0; i < pathForBuys.Count; i++)
        {
            if (pathForBuys[i].GetIdStreetPath() == path.GetIdStreetPath())
                pathForBuys[i] = path;
        }
    }

    //возврат части улицы по её айдишнику
    public StreetPath GetPathById(int id)
    {
        return paths[id];
    }

    //получить участок для продажи
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

    //получить участок, принадлежащий городу
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

        int[] builds = GetPathForBuy(idPath).Builds;
        
        foreach (int i in builds)
        {
            
                buildes.Add(this.builds[i]);
            
        }

        return buildes.ToArray();
    }

    //получить список участков, принадлежащих конкретному игроку
    public List<int> GetMyPathes(int id)
    {
        List<int> res = new List<int>();

        foreach (PathForBuy pathForBuy in pathForBuys)
        {
            if (pathForBuy.IdPlayer == id)
            {
                res.Add(pathForBuy.GetIdStreetPath());
            }
        }

        return res;
    }

    //получить список участков, принадлежащих конкретной монополии
    public List<StreetPath> GetPathsOfStreet(int id)
    {
        List<StreetPath> paths = new List<StreetPath>();
        int[] pathes = streets[id].Paths;
        for (int i = 0; i < pathes.Length; i++)
        {
            paths.Add(this.paths[pathes[i]]);
        }

        return paths;
    }
}