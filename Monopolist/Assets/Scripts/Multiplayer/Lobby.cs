using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lobby : Photon.PunBehaviour
{
    public MainMenu menuScript;

    public CanvasGroup networkMenu;

    private void Awake()
    {
        menuScript = gameObject.GetComponent<MainMenu>();
        networkMenu.interactable = false;
    }

    public void ConnectToServer()
    {
        PhotonNetwork.autoJoinLobby = true;
        PhotonNetwork.ConnectUsingSettings("0.0");
        StartCoroutine(WaitForConnection());
    }

    public RoomInfo[] getRooms()
    {
        return PhotonNetwork.GetRoomList();
    }

    public void ConnectToRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }
    
//начать новую игру
    public void StartNewNetworkGame(int countOfPlayers,int startMoney,string newNameGame, bool online,string nameTownForNewGame,string namePlayer)
    {
        string nameG = newNameGame;
        Camera.main.GetComponent<NetworkDBwork>()
            .CreateNewGame(countOfPlayers, startMoney, nameG, online, nameTownForNewGame, namePlayer);
        if (Trade.things == null)
        {
            Trade.things = new List<ThingForTrade>[countOfPlayers + 1, countOfPlayers + 1];
        }

        RoomOptions options = new RoomOptions();
        options.IsOpen = true;
        options.IsVisible = true;
        options.MaxPlayers = (byte) countOfPlayers;
        ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable
        {
            {"ngame", nameG},
            {"ntown", nameTownForNewGame}
        };
        options.CustomRoomProperties = ht;
        options.CustomRoomPropertiesForLobby = new string[] {"ngame", "ntown"};


        PhotonNetwork.CreateRoom(nameG, options, TypedLobby.Default);

        StartCoroutine(OpenScene());


    }

    IEnumerator OpenScene()
    {
        int i = 0;
        while (!PhotonNetwork.isMasterClient && i < 10)
        {
            Debug.Log("Connecting...");
            i++;
            yield return new WaitForSeconds(0.5f);
        }

        if (i == 10)
        {
            Debug.LogError("Connecting failed!");
            yield break;
        }

        Debug.Log("PhotonNetwork : Loading Level : " + PhotonNetwork.room.PlayerCount);
        PhotonNetwork.LoadLevel("GameNetwork");
    }

    IEnumerator WaitForConnection()
    {
        int i = 0;
        while (!PhotonNetwork.connected && i < 10)
        {
            Debug.Log("Connecting...");
            i++;
            yield return new WaitForSeconds(0.5f);
        }

        if (i == 10)
        {
            Debug.LogError("Connecting failed!");
            yield break;
        }
        else
        {
            networkMenu.interactable = true;
        }
    }

    public void JoinRoom()
    {
        
        PhotonNetwork.JoinRandomRoom();
    }


    public override void OnConnectedToMaster()
    {
        Debug.Log("DemoAnimator/Launcher: OnConnectedToMaster() was called by PUN");
    }


    public override void OnDisconnectedFromPhoton()
    {
        Debug.LogWarning("DemoAnimator/Launcher: OnDisconnectedFromPhoton() was called by PUN");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("We load the 'Room for 1' ");


        // #Critical
        // Load the Room Level. 
        PhotonNetwork.LoadLevel("GameNetwork");
    }


    public override void OnJoinedLobby()
    {
        Debug.Log("DemoAnimator/Launcher: OnJoinedLobby() called by PUN. Now this client is in a room.");
    }
}