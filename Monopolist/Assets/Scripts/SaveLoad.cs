using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class SaveLoad
{    
    //загрузка названий файлов из укаанной папки
    public static List<string> loadGamesList(string nameFolder)
    {
#if UNITY_EDITOR
        DirectoryInfo dir = new DirectoryInfo(@"Assets\" + nameFolder);
#else
        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath +"/"+ nameFolder);
#endif
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