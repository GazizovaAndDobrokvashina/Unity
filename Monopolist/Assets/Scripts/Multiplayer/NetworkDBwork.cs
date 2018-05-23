using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetworkDBwork : Photon.MonoBehaviour
{
    //массив игроков
    private NetworkPlayer[] players;

    //массив зданий
    private NetworkBuild[] builds;

    //масссив улиц
    private NetworkStreet[] streets;

    //массив частей улиц
    private NetworkStreetPath[] paths;

    //список участков, доступных для продажи
    private List<NetworkPathForBuy> pathForBuys;

    //список участков, принадлежащих городу
    private List<NetworkGovermentPath> govermentPaths;

    //ссылка на DataService
    private DataService dataService;

    //список всех путей между улицами
    private Ways ways;

    private List<string> names;

    //название текущего города
    private string nameOfTown;

    //название игры
    private string nameOfGane;

    private NetworkGovermentPath court;

    private NetworkGameCanvas _gameCanvas;

    //игрок, на чьей стороне скрипт находится
    private NetworkPlayer player;

    public bool ready;


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

    public void SetAll(NetworkStreetPath[] paths, List<NetworkPathForBuy> pathForBuys,
        List<NetworkGovermentPath> govermentPaths, NetworkStreet[] streets, NetworkBuild[] builds,
        NetworkPlayer[] players, string NameOfGane, string NameOfTown)
    {
        this.paths = paths;
        this.pathForBuys = pathForBuys;
        this.govermentPaths = govermentPaths;
        this.streets = streets;
        this.builds = builds;
        this.players = players;
        this.nameOfGane = NameOfGane;
        this.nameOfTown = NameOfTown;
    }
    
    public NetworkPlayer GetPlayer()
    {
        if (player == null && players != null)
        {
            foreach (NetworkPlayer networkPlayer in players)
            {
                if (networkPlayer.IdPlayer!=0 && !networkPlayer.isBot && networkPlayer.transform!=null && networkPlayer.transform.GetComponentInChildren<Camera>()!=null)
                    player = networkPlayer;
            }
        }
        return player;
    }
    

    public void SetGameCanvas(NetworkGameCanvas gameCanvas)
    {
        _gameCanvas = gameCanvas;
    }

    
    public NetworkGameCanvas GetNetworkGameCanvas()
    {
        return _gameCanvas;
    }
    
    public void SetGameDB(string dbName)
    {
        dataService = new DataService(dbName, true);
        GetEverithing();
        nameOfTown = dbName.Substring(0, dbName.IndexOf("_"));
        nameOfGane = dbName.Substring(dbName.IndexOf("_") + 1);
    }

    //получить здание по идентификатору
    public NetworkBuild GetBuild(int id)
    {
        return builds[id];
    }

    //получить монополию по её идентификатору
    public NetworkStreet getStreetById(int id)
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

    public NetworkGovermentPath getCourt()
    {
        return court;
    }

    public void SetCourt(NetworkGovermentPath court)
    {
        this.court = court;
    }

    //заполнение массивов игроков, улиц, частей улиц и зданий, исходя из данных в базе данных
    public void GetEverithing()
    {
        players = new NetworkPlayer[dataService.getPlayers().Count + 1];
        builds = new NetworkBuild[dataService.getBuilds().Count + 1];
        streets = new NetworkStreet[dataService.getStreets().Count + 1];
        paths = new NetworkStreetPath[dataService.getStreetPaths().Count + 1];
        pathForBuys = new List<NetworkPathForBuy>();
        govermentPaths = new List<NetworkGovermentPath>();

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
                        builds[buildse.IdBuild] = buildse.getNetworkBuild();
                        buildes[i] = buildse.IdBuild;
                        i++;
                    }

                    paths[streetPathse.IdStreetPath] = ifExist.GetNetworkPathForBuy(streetPathse, buildes);
                    pathForBuys.Add(ifExist.GetNetworkPathForBuy(streetPathse, buildes));
                }
                else
                {
                    List<Events> eventses = dataService.getEventsOnTheStreet(streetPathse.IdStreetPath);
                    Event[] events = new Event[eventses.Count];

                    
                    int j = 0;
                    if (!streetPathse.NameOfPrefab.Equals("Court"))
                    {
                        events = new Event[eventses.Count + 1];
                        j++;
                    }

                    foreach (Events eventse in eventses)
                    {
                        events[j] = eventse.GetEvent();
                        j++;
                    }

                    paths[streetPathse.IdStreetPath] = streetPathse.GetNetworkGovermentPath(events);
                    if (streetPathse.NameOfPrefab.Equals("Court"))
                    {
                        court = streetPathse.GetNetworkGovermentPath(events);
                    }

                    govermentPaths.Add(streetPathse.GetNetworkGovermentPath(events));
                }

                pathses[k] = streetPathse.IdStreetPath;
                k++;
            }

            streets[streetse.IdStreet] = streetse.GetNetworkStreet(pathses);
        }

        foreach (Players player in dataService.getPlayers())
        {
            players[player.IdPlayer] = player.GetNetworkPlayer();
        }

        players[0] = new NetworkPlayer();
        streets[0] = new NetworkStreet(0, "", "", new int[1]);
        paths[0] = new NetworkStreetPath(0, "", 0, 0, Vector3.zero, Vector3.zero, false, "");
        builds[0] = new NetworkBuild(0, "", "", 0, 0, false, 0, 0);
    }


    //помещение камеры в поле DontDestroyOnLoad для перенесения информации из главного меню в саму игру
    private void Awake()
    {
#if UNITY_EDITOR
        Directory.CreateDirectory(@"Assets\SavedGames\Network");
        Directory.CreateDirectory(@"Assets\StreamingAssets");
#else
        Directory.CreateDirectory(Application.persistentDataPath + @"\SavedGames\Network");
        Directory.CreateDirectory(Application.persistentDataPath + @"\StreamingAssets");
#endif

        DontDestroyOnLoad(gameObject);
        transform.position = new Vector3(5.63f, 0.43f, -5.63f);
        transform.localEulerAngles = new Vector3(0, -90, 0);
        
        
    }

    //возврат массива частей улиц
    public NetworkStreetPath[] GetAllPaths()
    {
        return paths;
    }

    //получить все здания в игре
    public NetworkBuild[] GetAllBuilds()
    {
        return builds;
    }

    public List<NetworkPathForBuy> GetAllPathForBuys()
    {
        return pathForBuys;
    }

    public List<NetworkGovermentPath> GetAllGovermentPaths()
    {
        return govermentPaths;
    }

    public NetworkStreet[] GetAllStreets()
    {
        return streets;
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
        currentGame = @"Assets\SavedGames\Network\" + nameOfTown + "_" + nameOfGane;
        newGame = @"Assets\SavedGames\Network\" + nameOfTown + "_" + newName + ".db";
        //DirectoryInfo dir = new DirectoryInfo(@"Assets\SavedGames\" +  );
#else
        currentGame = Application.persistentDataPath +@"/SavedGames/Network/"+ nameOfTown + "_" + nameOfGane ;
        newGame = Application.persistentDataPath +@"/SavedGames/Network/"+ nameOfTown + "_" + newName + ".db";
        //DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath +"/SavedGames/"+ nameFolder);
#endif

        File.Copy(currentGame, newGame, true);
    }

    //возвращение игрока по его айдишнику
    public NetworkPlayer GetPlayerbyId(int idPlayer)
    {
        return players[idPlayer];
    }

    //Создание новой игры (дописать для онлайна и разых городов)
    public void CreateNewGame(int countOfPlayers, int startMoney, string NameOfGame, bool online, string nameOfTown,
        string nickName)
    {
        
#if UNITY_EDITOR
        File.Copy(@"Assets\StreamingAssets\" + nameOfTown, @"Assets\SavedGames\Network\" + nameOfTown + "_" + NameOfGame + ".db");
#else
        File.Copy(Application.persistentDataPath + @"\StreamingAssets\" + nameOfTown, Application.persistentDataPath +@"/SavedGames/Network/" + nameOfTown + "_" + NameOfGame + ".db");
#endif
        
        dataService = new DataService(nameOfTown + "_" + NameOfGame + ".db", true);
        nameOfGane = NameOfGame + ".db";

        GetEverithing();
        players = new NetworkPlayer[countOfPlayers + 1];
        
        for (int i = 1; i < countOfPlayers + 1; i++)
        {
            NetworkPlayer player;
            if (i == 1)
                player = new NetworkPlayer(i, nickName, startMoney, false, false,
                    MapBuilder.GetCenter(paths[1].start, paths[1].end));
            else
                player = new NetworkPlayer(i, names[Random.Range(0, names.Count)], startMoney,false, true,
                    MapBuilder.GetCenter(paths[1].start, paths[1].end));
            players[i] = player;
            dataService.AddPlayer(player);
            
            
        }

        GetEverithing();

        this.nameOfTown = nameOfTown;
    }

    public void OnSceneLoad()
    {
        if (SceneManager.GetActiveScene().name.Equals("GameNetwork"))
        {
            
            if (PhotonNetwork.isMasterClient)
            {
                int id1 = PhotonNetwork.AllocateViewID();
                
                gameObject.AddComponent<PhotonView>().viewID = id1;
                //gameObject.GetComponent<PhotonView>().ObservedComponents = new List<Component> {this};
                
                //gameObject.GetComponent<PhotonView>().synchronization = ViewSynchronization.UnreliableOnChange;
            }
            else
            {
                ready = false;
                gameObject.AddComponent<PhotonView>().viewID = 1001;
                //gameObject.GetComponent<PhotonView>().ObservedComponents = new List<Component> {this};

                //gameObject.GetComponent<PhotonView>().synchronization = ViewSynchronization.UnreliableOnChange;
            }
            if (!PhotonNetwork.isMasterClient)
            {
                PhotonView photonView = gameObject.GetComponent<PhotonView>();
                //photonView.RPC("SendDBwork", PhotonTargets.MasterClient);
            }
        }
        else
        {
            if (gameObject.GetComponent<PhotonView>() != null)
            {
                Destroy(gameObject.GetComponent<PhotonView>());
            }
        }
    }
    
//    [PunRPC]
//    void SendDBwork()
//    {
//        Debug.Log("HEY ");
//        PhotonView photonView = gameObject.GetComponent<PhotonView>();
//        photonView.RPC("GetDBwork", PhotonTargets.Others);
//    }
//
//    [PunRPC]
//    void GetDBwork()
//    {
//        Debug.Log("Got ");
////        this.players = db.players;
////        this.streets = db.streets;
////        this.paths = db.paths;
//    }

    //возврат массива игроков
    public NetworkPlayer[] GetAllPlayers()
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
    public void updatePlayer(NetworkPlayer player)
    {
        players[player.IdPlayer] = player;
    }

    //обновить данные о здании
    public void updateBuild(NetworkBuild build)
    {
        builds[build.IdBuild] = build;
    }

    //обновить данные о части улицы
    public void updatePath(NetworkStreetPath path)
    {
        paths[path.GetIdStreetPath()] = path;
    }

    //обновить данные об участке для продажи
    public void updatePath(NetworkPathForBuy path)
    {
        paths[path.GetIdStreetPath()] = path;
        for (int i = 0; i < pathForBuys.Count; i++)
        {
            if (pathForBuys[i].GetIdStreetPath() == path.GetIdStreetPath())
                pathForBuys[i] = path;
        }
    }

    //возврат части улицы по её айдишнику
    public NetworkStreetPath GetPathById(int id)
    {
        return paths[id];
    }

    //получить участок для продажи
    public NetworkPathForBuy GetPathForBuy(int id)
    {
        if (paths[id].canBuy)
        {
            foreach (NetworkPathForBuy pathForBuy in pathForBuys)
            {
                if (pathForBuy.GetIdStreetPath() == id)
                    return pathForBuy;
            }
        }

        return null;
    }

    //получить участок, принадлежащий городу
    public NetworkGovermentPath GetGovermentPath(int id)
    {
        if (!paths[id].canBuy)
        {
            foreach (NetworkGovermentPath govermentPath in govermentPaths)
            {
                if (govermentPath.GetIdStreetPath() == id)
                    return govermentPath;
            }
        }

        return null;
    }

    //возврат зданий по айдишнику улицы, на которой они находятся (дописать метод нормально)
    public NetworkBuild[] GetBuildsForThisPath(int idPath)
    {
        List<NetworkBuild> buildes = new List<NetworkBuild>();

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

        foreach (NetworkPathForBuy pathForBuy in pathForBuys)
        {
            if (pathForBuy.IdPlayer == id)
            {
                res.Add(pathForBuy.GetIdStreetPath());
            }
        }

        return res;
    }

    //получить список участков, принадлежащих конкретной монополии
    public List<NetworkStreetPath> GetPathsOfStreet(int id)
    {
        List<NetworkStreetPath> paths = new List<NetworkStreetPath>();
        int[] pathes = streets[id].Paths;
        for (int i = 0; i < pathes.Length; i++)
        {
            paths.Add(this.paths[pathes[i]]);
        }

        return paths;
    }
    
//    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
//{
//    Debug.Log("DBWORK UPDATES!");
//   if (stream.isWriting)
//   {
//       
//       stream.SendNext(paths.Length);
//       
//       foreach (NetworkStreetPath networkStreetPath in paths)
//       {
//           stream.SendNext(networkStreetPath.GetIdStreetPath());
//           stream.SendNext(networkStreetPath.GetIdStreetParent());
//           stream.SendNext(networkStreetPath.GetRenta());
//           stream.SendNext(networkStreetPath.start);
//           stream.SendNext(networkStreetPath.end);
//           stream.SendNext(networkStreetPath.isBridge);
//           stream.SendNext(networkStreetPath.NeighborsId);
//           stream.SendNext(networkStreetPath.NamePath);
//           stream.SendNext(networkStreetPath.CanBuy);
//           stream.SendNext(networkStreetPath.GetNameOfPrefab());
//
//       }
//        stream.SendNext(pathForBuys.Count);
//       foreach (NetworkPathForBuy pathForBuy in pathForBuys)
//       {
//           stream.SendNext(pathForBuy.GetIdStreetPath());
//           stream.SendNext(pathForBuy.IdPlayer);
//           stream.SendNext(pathForBuy.Builds);
//           stream.SendNext(pathForBuy.PriceStreetPath);
//           stream.SendNext(pathForBuy.IsBlocked);
//       }
//       
//       stream.SendNext(govermentPaths.Count);
//
//       foreach (NetworkGovermentPath govermentPath in govermentPaths)
//       {
//           stream.SendNext(govermentPath.GetIdStreetPath());
//           Event[] events = govermentPath.events;
//           stream.SendNext(events.Length);
//           foreach (Event eve in events)
//           {
//               if (eve != null)
//               {
//                   stream.SendNext(eve.Id);
//                   stream.SendNext(eve.Info);
//                   stream.SendNext(eve.Name);
//                   stream.SendNext(eve.Price);
//               }
//               else
//               {
//                   stream.SendNext(0);
//               }
//               
//           }
//       }
//       
//       stream.SendNext(streets.Length);
//
//       foreach (NetworkStreet street in streets)
//       {
//           stream.SendNext(street.IdStreet);
//           stream.SendNext(street.NameStreet);
//           stream.SendNext(street.AboutStreet);
//           stream.SendNext(street.Paths);
//       }
//       
//       stream.SendNext(builds.Length);
//       foreach (NetworkBuild build in builds)
//       {
//           stream.SendNext(build.IdBuild);
//           stream.SendNext(build.NameBuild);
//           stream.SendNext(build.AboutBuild);
//           stream.SendNext(build.IdStreetPath);
//           stream.SendNext(build.PriceBuild);
//           stream.SendNext(build.Enable);
//           stream.SendNext(build.Place);
//       }
//       
//       stream.SendNext(players.Length);
//       foreach (NetworkPlayer networkPlayer in players)
//       {
//           stream.SendNext(networkPlayer.IdPlayer);
//           stream.SendNext(networkPlayer.ViewId);
//           stream.SendNext(networkPlayer.NickName);
//           stream.SendNext(networkPlayer.Money);
//           stream.SendNext(networkPlayer.MaxSteps);
//           stream.SendNext(networkPlayer.CurrentSteps);
//           stream.SendNext(networkPlayer.IsBankrupt);
//           stream.SendNext(networkPlayer.Destination);
//           stream.SendNext(networkPlayer.CurrentStreetPath.GetIdStreetPath());
//           stream.SendNext(networkPlayer.isBot);
//
//       }
//   }
//   else
//   {
//       Debug.Log("DBWORK UPDATES!");
//       paths = new NetworkStreetPath[(int)stream.ReceiveNext()];
//       for (int i = 0; i < paths.Length; i++)
//       {
//       Debug.Log("DBWORK UPDATES!");
//           int pathId = (int)stream.ReceiveNext();
//           int parentId = (int) stream.ReceiveNext();
//           int renta = (int) stream.ReceiveNext();
//           Vector3 start = (Vector3) stream.ReceiveNext();
//           Vector3 end = (Vector3) stream.ReceiveNext();
//           bool isBridge = (bool) stream.ReceiveNext();
//           int[] neighbours = (int[]) stream.ReceiveNext();
//           string namePath = (string) stream.ReceiveNext();
//           bool canBuy = (bool) stream.ReceiveNext();
//           string nameOfPref = (string) stream.ReceiveNext();
//           
//           paths[i] = new NetworkStreetPath(pathId,namePath, parentId, renta, start, end, isBridge, nameOfPref);
//           paths[i].neighborsId = neighbours;
//           paths[i].CanBuy = canBuy;
//       }
//       
//       pathForBuys = new List<NetworkPathForBuy>();
//       int count = (int)stream.ReceiveNext();
//
//       Debug.Log("DBWORK UPDATES!");
//       for (int i=0; i < count; i++)
//       {
//           Debug.Log("DBWORK UPDATES!");
//           int id = (int) stream.ReceiveNext();
//           NetworkStreetPath path = paths[id];
//           int owmerId = (int) stream.ReceiveNext();
//           int[] buildes = (int[]) stream.ReceiveNext();
//           int price = (int) stream.ReceiveNext();
//           bool isBlocked = (bool) stream.ReceiveNext();
//           
//           pathForBuys.Add(new NetworkPathForBuy(id,path.NamePath,path.GetIdStreetParent(), path.GetRenta(), path.start, path.end, owmerId, buildes, price, path.isBridge,path.GetNameOfPrefab(),isBlocked));
//       }
//       
//       
//
//       count = (int) stream.ReceiveNext();
//
//       for (int i=0; i < count; i++)
//       {
//           int id = (int) stream.ReceiveNext();
//           NetworkStreetPath path = paths[id];
//
//           int countEvent = (int) stream.ReceiveNext();
//           Event[] eves = new Event[countEvent];
//           for (int j = 0; j < countEvent; j++)
//           {
//               int eId = (int) stream.ReceiveNext();
//               if (eId == 0)
//               {
//                   eves[i] = null;
//               }
//               else
//               {
//                   string eInfo = (string) stream.ReceiveNext();
//                   string eName = (string) stream.ReceiveNext();
//                   int ePrice = (int) stream.ReceiveNext();
//                   eves[i] = new Event(eId, eInfo, eName, ePrice, id);
//               }
//           }
//           
//           
//           govermentPaths.Add(new NetworkGovermentPath(id,path.NamePath,path.GetIdStreetParent(), path.GetRenta(), path.start, path.end, path.isBridge,path.GetNameOfPrefab(),eves));
//       }
//       
//       streets = new NetworkStreet[(int)stream.ReceiveNext()];
//       for (int i = 0; i < streets.Length; i++)
//       {
//           int stId = (int)stream.ReceiveNext();
//           string stName = (string)stream.ReceiveNext();
//           string stAbout = (string)stream.ReceiveNext();
//           int[] stPaths = (int[])stream.ReceiveNext();
//           
//           streets[i] = new NetworkStreet(stId,stName,stAbout,stPaths);
//       }
//       
//       builds = new NetworkBuild[(int)stream.ReceiveNext()];
//       
//       for (int i = 0; i < builds.Length; i++)
//       {
//           int bId = (int)stream.ReceiveNext();
//           string bName = (string)stream.ReceiveNext();
//           string bAbout = (string)stream.ReceiveNext();
//           int bstID = (int)stream.ReceiveNext();
//           int bPrice = (int)stream.ReceiveNext();
//           bool ben = (bool)stream.ReceiveNext();
//           Vector3 bplace = (Vector3) stream.ReceiveNext();
//           
//           builds[i] = new NetworkBuild(bId,bName,bAbout,bstID,bPrice,ben,bplace.x, bplace.z);
//       }
//       
//       players = new NetworkPlayer[(int)stream.ReceiveNext()];
//       for (int i = 0; i < players.Length; i++)
//       {
//           int plId = (int)stream.ReceiveNext(); 
//           int plVId = (int)stream.ReceiveNext();
//           string plName = (string) stream.ReceiveNext();
//           int plMoney = (int) stream.ReceiveNext();
//           bool plBank = (bool)stream.ReceiveNext();
//           Vector3 plDest = (Vector3)stream.ReceiveNext();
//           NetworkStreetPath
//               plPath = pathForBuys[(int) stream.ReceiveNext()];
//           bool plIsBot = (bool) stream.ReceiveNext();
//           
//           players[i] = new NetworkPlayer(plId,plName,plMoney,plBank,plIsBot,plDest);
//           players[i].ViewId = plVId;
//           
//       }
//
//       object name;
//       PhotonNetwork.room.CustomProperties.TryGetValue("ngame", out name);
//       nameOfGane = (string) name;
//       PhotonNetwork.room.CustomProperties.TryGetValue("ntown", out name);
//       nameOfTown = (string) name;
//
//       ready = true;
//       
//       Debug.Log("DBWORK UPDATES!");
//   }
//}
}