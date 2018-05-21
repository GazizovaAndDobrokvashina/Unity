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
		PhotonNetwork.ConnectUsingSettings("0.0");
		StartCoroutine(WaitForConnection());
	}
	
//начать новую игру
	public void StartNewNetworkGame()
	{
		string nameG = "ABCDE" + (int) Time.time;
		Camera.main.GetComponent<NetworkDBwork>()
			.CreateNewGame(3, 1500, nameG, true, "TRON", "eblan");
		if (Trade.things == null)
		{
			Trade.things = new List<ThingForTrade>[3, 3];
		}
		RoomOptions options = new RoomOptions();
		options.IsOpen = true;
		options.IsVisible = true;
		options.MaxPlayers = (byte)3;
		ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable
		{
			{"ngame",nameG},
			{"ntown","TRON"}
		};
		options.CustomRoomProperties = ht;
		options.CustomRoomPropertiesForLobby = new string[] {"ngame", "ntown"};
        
		
		PhotonNetwork.CreateRoom(nameG, options, TypedLobby.Default);

		StartCoroutine(OpenScene());

//		SceneManager.LoadScene("GameNetwork", LoadSceneMode.Single);

//		Camera.main.GetComponent<NetworkDBwork>()
//			.CreateNewGame(menuScript.CountOfPlayers, menuScript.StartMoney, menuScript.NewNameGame, true, menuScript.NameTownForNewGame, menuScript.NamePlayer);
//		if (Trade.things == null)
//		{
//			Trade.things = new List<ThingForTrade>[menuScript.CountOfPlayers, menuScript.CountOfPlayers];
//		}
//		RoomOptions options = new RoomOptions();
//		options.IsOpen = true;
//		options.IsVisible = true;
//		options.MaxPlayers = (byte)menuScript.CountOfPlayers;
//		ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable
//		{
//			{"ngame", menuScript.NewNameGame},
//			{"ntown", menuScript.NameTownForNewGame}
//		};
//		options.CustomRoomProperties = ht;
//		options.CustomRoomPropertiesForLobby = new string[] {"ngame", "ntown"};
//        
//		PhotonNetwork.CreateRoom(menuScript.NewNameGame, options, TypedLobby.Default);
//
//		SceneManager.LoadScene("GameNetwork", LoadSceneMode.Single);
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
