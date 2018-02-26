using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class SaveLoad
{
    private static string _nameFolder;
    
    //загрузка названий файлов из укаанной папки
    public static List<string> loadGamesList(string nameFolder)
    {
        _nameFolder = nameFolder;
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

    //удаление игры
    public static void deleteGame(string dbName)
    {
#if UNITY_EDITOR
        DirectoryInfo dir = new DirectoryInfo(@"Assets\" + _nameFolder);
#else
        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath +"/"+ _nameFolder);
#endif
        Debug.Log(dir + " " +dbName);
        foreach (var item in dir.GetFiles())
        {
            
            if (item.Name.Equals(dbName))
            {
                
                Debug.Log("Нашел!");
                item.Delete();
                break;
            }
        }
    }

}