using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    //ссылка на DataService
    private DataService dataService;

    //список всех путей между улицами
    private Ways ways;

    //название текущего города
    private string nameOfTown;

    void Start()
    {
        //DBStart();
        //GetEverithing();
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
                }

                pathses[k] = streetPathse.IdStreetPath;
            }

            streets[streetse.IdStreet] = streetse.GetStreet(pathses);
        }

        foreach (Players player in dataService.getPlayers())
        {
            players[player.IdPlayer] = player.GetPlayer();
        }
        players[0] = new Player(0, 0, true, Vector3.zero);
        streets[0] = new Street(0, "", "", new int[1]);
        paths[0] = new StreetPath(0, 0, 0, Vector3.zero, Vector3.zero, false);
        builds[0] = new Build(0, 0, 0, false);
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
    public void CreateNewGame(int countOfPlayers, int startMoney, string NameOfGame, bool online, string nameOfTown)
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
            Player player = new Player(i, startMoney, false, MapBuilder.GetCenter(paths[1].start, paths[1].end));
            players[i] = player;
            dataService.AddPlayer(player);
        }

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

    //обновить данные о части улицы
    public void updatePath(StreetPath path)
    {
        paths[path.GetIdStreetPath()] = path;
    }

    //возврат части улицы по её айдишнику
    public StreetPath GetPathById(int id)
    {
        return paths[id];
    }

    //возврат зданий по айдишнику улицы, на которой они находятся (дописать метод нормально)
    public Build[] GetBuildsForThisPath(int idPath)
    {
        return builds;
    }
}