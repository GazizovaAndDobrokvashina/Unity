﻿using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class SaveLoad {

	public List<string> loadGamesList()
	{
	
		DirectoryInfo dir = new DirectoryInfo(@"Assets\SavedGames");
		
		List<string> names = new List<string>();
		
		foreach (var item in dir.GetFiles())
		{
			if (!item.Name.Contains(".meta"))
			{
				names.Add(item.Name);
			}
			
		}

		return names;
	}

	public void loadGame(string dbName)
	{
		Camera.main.GetComponent<DBwork>().SetGameDB(dbName);
	}

	public void createNewGame(string gameName, int playerCount)
	{
		
	}
}
