using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using FileMode = System.IO.FileMode;
using UnityEngine;

public class Ways
{
    //массив очередей, где индексы это начальная и конечная точка, а элемент это кратчайшая очередб вершин между ними
    private Queue<int>[,] _queues;

    //конструктор, создает пути, если они ещё не созданы по массиву улиц, и загружает уже готовый массив, если таковой имеется
    public Ways(String nameOfTown, StreetPath[] streetPaths)
    {
        

        if (!GetWays(nameOfTown))
        {
            createWays(streetPaths);

            SaveWays(nameOfTown);
        }
    }
    
    public Ways(String nameOfTown, NetworkStreetPath[] streetPaths)
    {
        

        if (!GetWays(nameOfTown))
        {
            createWays(streetPaths);

            SaveWays(nameOfTown);
        }
    }    

    //подсчёт путей в заданном массиве частей улиц
    public void createWays(StreetPath[] streetPaths)
    {
        _queues = new Queue<int>[streetPaths.Length, streetPaths.Length];

        foreach (StreetPath path in streetPaths)
        {
            _queues[path.GetIdStreetPath(), path.GetIdStreetPath()] = new Queue<int>();
            if (path.GetIdStreetPath() == 0)
                continue;

            foreach (int i in path.NeighborsId)
            {
                if ((path.isBridge && path.start.Equals(streetPaths[i].start)) ||
                    path.end.Equals(streetPaths[i].start) ||
                    (streetPaths[i].isBridge && path.end.Equals(streetPaths[i].end)))
                {
                    _queues[path.GetIdStreetPath(), i] = new Queue<int>();
                    _queues[path.GetIdStreetPath(), i].Enqueue(i);
                }
            }
        }

        for (int i = 1; i < streetPaths.Length; i++)
        {
            for (int j = 1; j < streetPaths.Length; j++)
            {
                for (int k = 1; k < streetPaths.Length; k++)
                {
                    if ((_queues[j, k] == null && _queues[j, i] != null && _queues[i, k] != null) ||
                        (_queues[j, k] != null && _queues[j, i] != null && _queues[i, k] != null &&
                         _queues[j, k].Count > _queues[j, i].Count + _queues[i, k].Count))
                    {
                        _queues[j, k] = new Queue<int>();
                        foreach (int i1 in _queues[j, i].ToArray())
                        {
                            _queues[j, k].Enqueue(i1);
                        }

                        foreach (int i1 in _queues[i, k].ToArray())
                        {
                            _queues[j, k].Enqueue(i1);
                        }
                    }
                }
            }
        }
    }
    
    public void createWays(NetworkStreetPath[] streetPaths)
    {
        _queues = new Queue<int>[streetPaths.Length, streetPaths.Length];

        foreach (NetworkStreetPath path in streetPaths)
        {
            _queues[path.GetIdStreetPath(), path.GetIdStreetPath()] = new Queue<int>();
            if (path.GetIdStreetPath() == 0)
                continue;

            foreach (int i in path.NeighborsId)
            {
                if ((path.isBridge && path.start.Equals(streetPaths[i].start)) ||
                    path.end.Equals(streetPaths[i].start) ||
                    (streetPaths[i].isBridge && path.end.Equals(streetPaths[i].end)))
                {
                    _queues[path.GetIdStreetPath(), i] = new Queue<int>();
                    _queues[path.GetIdStreetPath(), i].Enqueue(i);
                }
            }
        }

        for (int i = 1; i < streetPaths.Length; i++)
        {
            for (int j = 1; j < streetPaths.Length; j++)
            {
                for (int k = 1; k < streetPaths.Length; k++)
                {
                    if ((_queues[j, k] == null && _queues[j, i] != null && _queues[i, k] != null) ||
                        (_queues[j, k] != null && _queues[j, i] != null && _queues[i, k] != null &&
                         _queues[j, k].Count > _queues[j, i].Count + _queues[i, k].Count))
                    {
                        _queues[j, k] = new Queue<int>();
                        foreach (int i1 in _queues[j, i].ToArray())
                        {
                            _queues[j, k].Enqueue(i1);
                        }

                        foreach (int i1 in _queues[i, k].ToArray())
                        {
                            _queues[j, k].Enqueue(i1);
                        }
                    }
                }
            }
        }
    }


    //загрузка очередей улиц
    private bool GetWays(string nameOfTown)
    {
#if UNITY_EDITOR
        if (!Directory.Exists(@"Assets\Ways\"))
        {
            Directory.CreateDirectory(@"Assets\Ways\");
        }

        if (!File.Exists(@"Assets\Ways\" + nameOfTown + ".ways"))
        {
            return false;
        }
#else
        if (!Directory.Exists(Application.persistentDataPath +"/Ways/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath +"/Ways/");
        }
        if (!File.Exists(Application.persistentDataPath +"/Ways/" + nameOfTown + ".ways"))
        {
            return false;
        }

#endif

#if UNITY_EDITOR
        FileStream dir = new FileStream(@"Assets\Ways\" + nameOfTown + ".ways", FileMode.Open, FileAccess.Read);
#else
        FileStream dir =
 new FileStream(Application.persistentDataPath +"/Ways/"+ nameOfTown + ".ways", FileMode.Open, FileAccess.Read);
#endif
        using (dir)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<int>[][]));
            convertToWays((List<int>[][]) serializer.Deserialize(dir));
        }

        return true;
    }


    //возвращение массива очередей улиц
    public Queue<int>[,] Queues
    {
        get { return _queues; }
    }


    //сохранение массива очередей улиц
    private void SaveWays(string nameOfTown)
    {
#if UNITY_EDITOR

        FileStream dir = new FileStream(@"Assets\Ways\" + nameOfTown + ".ways", FileMode.Create, FileAccess.Write);
#else
        FileStream dir =
 new FileStream(Application.persistentDataPath +"/Ways/"+ nameOfTown + ".ways", FileMode.Create, FileAccess.Write);
#endif
        List<int>[][] list = convertToList();
        using (dir)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<int>[][]));
            serializer.Serialize(dir, list);
        }
    }

    //преобразовать в очереди в листы
    private List<int>[][] convertToList()
    {
        List<int>[][] list = new List<int>[_queues.GetLongLength(1)][];

        for (int i = 0; i < _queues.GetLongLength(1); i++)
        {
            list[i] = new List<int>[_queues.GetLongLength(1)];
            if (i == 0)
                continue;
            for (int j = 0; j < _queues.GetLongLength(1); j++)
            {
                if (j == 0)
                    continue;
                list[i][j] = new List<int>(_queues[i, j]);
            }
        }

        return list;
    }

    //преобразовать листы в очереди
    private void convertToWays(List<int>[][] list)
    {
        _queues = new Queue<int>[list.Length, list.Length];
        for (int i = 1; i < _queues.GetLongLength(1); i++)
        {
            for (int j = 1; j < _queues.GetLongLength(1); j++)
            {
                _queues[i, j] = new Queue<int>();
                foreach (int i1 in list[i][j])
                {
                    _queues[i, j].Enqueue(i1);
                }
            }
        }
    }
}