﻿using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class SaveLoad
{    
    //загрузка названий файлов из укаанной папки
    public static List<string> loadGamesList(string nameFolder)
    {
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath +"/"+ nameFolder);
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
    
    //загрузка игры
    public static void loadGame(string dbName)
    {
        Camera.main.GetComponent<DBwork>().SetGameDB(dbName);
    }

}