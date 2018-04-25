using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lobby : Photon.MonoBehaviour
{

	public MainMenu menuScript;


	private void Start()
	{
		menuScript = gameObject.GetComponent<MainMenu>();
	}
	
//начать новую игру
	public void StartNewNetworkGame()
	{
		
		Camera.main.GetComponent<DBwork>()
			.CreateNewGame(3, 1500, "ABCDE", true, "TRON", "eblan");
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
			{"ngame","ABCDE"},
			{"ntown","TRON"}
		};
		options.CustomRoomProperties = ht;
		options.CustomRoomPropertiesForLobby = new string[] {"ngame", "ntown"};
        
		PhotonNetwork.CreateRoom("ABCDE", options, TypedLobby.Default);

		SceneManager.LoadScene("GameNetwork", LoadSceneMode.Single);
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

	
}
