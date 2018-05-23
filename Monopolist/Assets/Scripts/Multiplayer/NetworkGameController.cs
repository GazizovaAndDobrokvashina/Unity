using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class NetworkGameController : Photon.MonoBehaviour
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

    [PunRPC]
    public void UpdateHistory(string text)
    {
        aboutPlayer = text;
        aboutPlayerText.text = text;
    }


    //сброс истории, обновление ссылки на дбворк, бросок кубиков первого игрока
    void Start()
    {
        aboutPlayer = "";
        _dBwork = Camera.main.GetComponent<NetworkDBwork>();
        _gameCanvas = gameObject.GetComponent<NetworkGameCanvas>();

        posFirstDice = firstDice.transform.position;
        posSecondDice = secondDice.transform.position;
        CurrentPlayer = _dBwork.GetPlayer();
        if (PhotonNetwork.isMasterClient && CurrentPlayer!=null ){
        CurrentPlayer.NextStep();
        }
        else
        {
            StartCoroutine(WaitForChanges());
        }
    }

    IEnumerator WaitForChanges()
    {
        while (CurrentPlayer == null && !_dBwork.ready)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log("Trying to get Current in Controller...");
            CurrentPlayer = _dBwork.GetPlayer();
        }

        if (PhotonNetwork.isMasterClient)
        {
            CurrentPlayer.NextStep();
        }
    }
    
    //вывод истории на экран
    void Update()
    {
        if (aboutPlayerText.text != aboutPlayer)
        {
            aboutPlayerText.text = aboutPlayer;
            GetComponent<PhotonView>().RPC("UpdateHistory", PhotonTargets.All, aboutPlayer);
        }
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
        if (CurrentPlayer.isBot)
        {
            if (!CurrentPlayer.IsBankrupt)
            {
                if (CountStepsInAllGame % 10 == 0)
                    CurrentPlayer.Money += salary;
                if (!CurrentPlayer.isInJail())
                {
                    yield return StartCoroutine(Dices());
                }
                else
                {
                    aboutPlayer += "Прямое включение из тюрьмы: Ход игрока " + CurrentPlayer.NickName + "\n";
                }

                CurrentPlayer.NextStep();
                yield return new WaitUntil(() => CurrentPlayer.ready);
                checkPlayer(CurrentPlayer.IdPlayer);
            }

            int idOfNext = _dBwork.GetPlayer().IdPlayer + 1;
            if (idOfNext == _dBwork.GetAllPlayers().Length)
            {
                idOfNext = 1;
            }

            CurrentPlayer = _dBwork.GetPlayerbyId(idOfNext);
            CurrentPlayer.ready = false;
            if (_dBwork.GetPlayerbyId(idOfNext).isBot)
            {
                GetComponent<PhotonView>().RPC("GoNextStepForBot", PhotonTargets.MasterClient, idOfNext);
            }
        }
        else
        {
            checkPlayer(_dBwork.GetPlayer().IdPlayer);
            _dBwork.GetPlayer().SetCurrentStep(false);
            firstDice.gameObject.SetActive(false);
            secondDice.gameObject.SetActive(false);
            _dBwork.GetPlayer().ready = true;
            _dBwork.GetPlayer().SetMaxStep(0);
            nextStepButton.GetComponent<CanvasGroup>().interactable = false;
            if (_dBwork.GetPlayer().IdPlayer == 1)
                CountStepsInAllGame++;

            if (CountStepsInAllGame % 10 == 0)
                _dBwork.GetPlayer().Money += salary;

            int idOfNext = _dBwork.GetPlayer().IdPlayer + 1;
            if (idOfNext == _dBwork.GetAllPlayers().Length)
            {
                idOfNext = 1;
            }

            CurrentPlayer = _dBwork.GetPlayerbyId(idOfNext);
            CurrentPlayer.ready = false;
            if (_dBwork.GetPlayerbyId(idOfNext).isBot)
            {
                GetComponent<PhotonView>().RPC("GoNextStepForBot", PhotonTargets.MasterClient, idOfNext);
            }


            yield return new WaitUntil(() => _dBwork.GetPlayer() == CurrentPlayer);


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

            _dBwork.GetPlayer().NextStep();
            nextStepButton.GetComponent<CanvasGroup>().interactable = true;
            _dBwork.GetPlayer().SetCurrentStep(true);
        }

    }

    [PunRPC]
    void GoNextStepForBot(int playerId)
    {
        CurrentPlayer = _dBwork.GetPlayerbyId(playerId);
        StartCoroutine(GoNextStep());
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

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Debug.Log("DBWORK UPDATES!");
//   if (stream.isWriting)
//   {
//       NetworkStreetPath[] paths = _dBwork.GetAllPaths();
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
//
//       List<NetworkPathForBuy> pathForBuys = _dBwork.GetAllPathForBuys();
//       
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
//       List<NetworkGovermentPath> govermentPaths = _dBwork.GetAllGovermentPaths();
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
//       NetworkStreet[] streets = _dBwork.GetAllStreets();
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
//       NetworkBuild[] builds = _dBwork.GetAllBuilds();
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
//       NetworkPlayer[] players = _dBwork.GetAllPlayers();
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
//       
//       NetworkStreetPath[] paths = new NetworkStreetPath[(int)stream.ReceiveNext()];
//       for (int i = 0; i < paths.Length; i++)
//       {
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
//       List<NetworkPathForBuy> pathForBuys = new List<NetworkPathForBuy>();
//       int count = (int)stream.ReceiveNext();
//
//       Debug.Log("DBWORK UPDATES!");
//       for (int i=0; i < count; i++)
//       {
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
//       List<NetworkGovermentPath> govermentPaths = new List<NetworkGovermentPath>();
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
//       NetworkStreet[] streets = new NetworkStreet[(int)stream.ReceiveNext()];
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
//       NetworkBuild[] builds = new NetworkBuild[(int)stream.ReceiveNext()];
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
//       NetworkPlayer[] players = new NetworkPlayer[(int)stream.ReceiveNext()];
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
//       string nameOfGane = (string) name;
//       PhotonNetwork.room.CustomProperties.TryGetValue("ntown", out name);
//       string nameOfTown = (string) name;
//
//       _dBwork.SetAll(paths,pathForBuys,govermentPaths,streets,builds,players,nameOfGane, nameOfTown);
//       _dBwork.ready = true;
//       
//       Debug.Log("DBWORK UPDATES!");
//   }
        
        if (stream.isWriting)
        {
            
            if (CurrentPlayer != null)
            {
                stream.SendNext(CurrentPlayer.IdPlayer);
            }
            else
            {
                stream.SendNext(0);
            }

            stream.SendNext(CountStepsInAllGame);
        }
        else
        {
            int id = (int) stream.ReceiveNext();
            if (id != 0)
            {
                CurrentPlayer = _dBwork.GetPlayerbyId(id);
            }



            CountStepsInAllGame = (int)stream.ReceiveNext();
        }
    }
    
    
//    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
//    {
//        if (stream.isWriting)
//        {
//            if (CurrentPlayer != null)
//            {
//                stream.SendNext(CurrentPlayer.IdPlayer);
//            }
//            else
//            {
//                stream.SendNext(0);
//            }
//
//            stream.SendNext(CountStepsInAllGame);
//        }
//        else
//        {
//            int id = (int) stream.ReceiveNext();
//            if (id != 0)
//            {
//            CurrentPlayer = _dBwork.GetPlayerbyId(id);
//            }
//
//
//
//        CountStepsInAllGame = (int)stream.ReceiveNext();
//        }
//    }
}