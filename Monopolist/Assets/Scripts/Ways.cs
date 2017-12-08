using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ways
{
    //массив очередей, где индексы это начальная и конечная точка, а элемент это кратчайшая очередб вершин между ними
    private Queue<int>[,] _queues;

    //конструктор, создает пути, если они ещё не созданы по массиву улиц, и загружает уже готовый массив, если таковой имеется
    public Ways(String nameOfTown, StreetPath[] streetPaths)
    {
        //прописать подгрузку с файла

        createWays(streetPaths);
    }

    //подсчёт путей в заданном массиве частей улиц
    public void createWays(StreetPath[] streetPaths)
    {
        _queues = new Queue<int>[streetPaths.Length, streetPaths.Length];

        foreach (StreetPath path in streetPaths)
        {
            if (path.GetIdStreetPath() == 0)
                continue;
            _queues[path.GetIdStreetPath(), path.GetIdStreetPath()] = new Queue<int>();

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
    

    //возвращение массива очередей улиц
    public Queue<int>[,] Queues
    {
        get { return _queues; }
    }
}