using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class SaveLoad
{
    public static List<string> loadGamesList(string nameFolder)
    {
        //DirectoryInfo dir = new DirectoryInfo(@"Assets\SavedGames");
        DirectoryInfo dir = new DirectoryInfo(@"Assets\" + nameFolder);
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

    public static void loadGame(string dbName)
    {
        Camera.main.GetComponent<DBwork>().SetGameDB(dbName);
    }

    public void createNewGame(string gameName, int playerCount)
    {
    }
}