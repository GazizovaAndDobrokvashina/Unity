﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DBwork: MonoBehaviour
{
	private Player[] players;
	private Build[] builds;
	private Street[] streets;
	private StreetPath[] paths;

	private DataService ds;
	

	void Start()
	{
		//DBStart();
		//GetEverithing();
	}

	public void DBStart()
	{
		ds =  new DataService("Monopolist.db");
		if(!ds.IsExist())
			ds.CreateDB();


		/*foreach (Streets streetse in ds.getStreets())
		{
			Debug.Log(streetse.NameStreet + "   " + streetse.AboutStreet);
		}*/
		
	}

	public void SetGameDB(string dbName)
	{
		ds = new DataService(dbName);
		GetEverithing();		
	
	}

	public void GetEverithing()
	{
	
		players = new Player[ds.getPlayers().Count+1];
		builds = new Build[ds.getBuilds().Count+1];
		streets = new Street[ds.getStreets().Count+1];
		paths = new StreetPath[ds.getStreetPaths().Count+1];

		foreach (Streets streetse in ds.getStreets())
		{
			List<StreetPaths> streetPathses = ds.getAllPathsOfStreet(streetse.IdStreet);
			//Debug.Log(streetPathses.Count);
			int[] pathses = new int[streetPathses.Count];

			int k = 0;
			foreach (StreetPaths streetPathse in streetPathses)
			{
				PathsForBuy ifExist = ds.getPathForBuyById(streetPathse.IdStreetPath);
				
				if (ifExist != null)
				{
					List<Builds> buildses = ds.getBuildsOnTheStreet(streetPathse.IdStreetPath);
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
					List<Events> eventses = ds.getEventsOnTheStreet(streetPathse.IdStreetPath);
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

		foreach (Players player in ds.getPlayers())
		{
			players[player.IdPlayer] = player.GetPlayer();
		}
		
	}
	
	

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
		transform.position = new Vector3(5.63f,0.43f,-5.63f);
		transform.localEulerAngles = new Vector3(0,-90,0);
	}

	public StreetPath[] GetAllPaths()
	{
		return paths;
	}

	public void SaveGame()
	{
		
	}

	public void SaveGameAsNewFile(string newName)
	{
		
	}

	public Player GetPlayerbyId(int idPlayer)
	{
		return players[idPlayer];
	}

	public void CreateNewGame(int countOfPlayers, int startMoney)
	{
		
		File.Copy(@"Assets\StreamingAssets\Monopolist.db", @"Assets\SavedGames\Firstgame.db");
		SetGameDB("Firstgame.db");
		GetEverithing();
		players = new Player[countOfPlayers + 1];
		for (int i = 1; i < countOfPlayers + 1; i++)
		{
			Player player = new Player(i, startMoney, false, paths[1].start);
			players[i] = player;
			ds.AddPlayer(player);
			
		}
		
	}

	public Player[] GetAllPlayers()
	{
		return players;
	}

	public void updatePlayer(Player player)
	{
		players[player.IdPlayer] = player;
	}
}
