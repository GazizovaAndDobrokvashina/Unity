﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetPath : MonoBehaviour
{    
    //айди части улицы
    protected int idStreetPath;
    //айди родительской улицы
    protected int idStreetParent;
    //плата других игроков владельцу этой части улицы
    protected int renta;
    //координаты начала улицы
    public Vector3 start;
    //координаты конца улицы
    public Vector3 end;
    //является ли мостом часть улицы
    public bool isBridge;
    //список соседей улицы
    public int[] neighborsId;

    public string namePath;

    public bool canBuy;
    
    public string NamePath
    {
        get { return namePath; }
    }

    public bool CanBuy
    {
        get { return canBuy; }
        set { canBuy = value; }
    }

    //если игрок наступает на эту часть улицы
    public void StepOnMe(int idPlayer)
    {
    }
    
    //
    public void TakeData(StreetPath streetPath)
    {
        this.idStreetParent = streetPath.GetIdStreetParent();
        this.idStreetPath = streetPath.GetIdStreetPath();
        this.renta = streetPath.GetRenta();
        this.start = streetPath.start;
        this.end = streetPath.end;
        this.isBridge = streetPath.isBridge;
        this.namePath = streetPath.namePath;
        this.CanBuy = streetPath.canBuy;
    }
    
    //нахождение сосених частей улиц с этой
    public void GetNeighbors()
    {
        DBwork ds = Camera.main.GetComponent<DBwork>();
        List<int> neighs = new List<int>();

        StreetPath[] streetPaths = ds.GetAllPaths();
        for (int i = 1; i < streetPaths.Length; i++)
        {
            if (streetPaths[i].end.Equals(end) || streetPaths[i].start.Equals(start) ||
                streetPaths[i].end.Equals(start) ||
                streetPaths[i].start.Equals(end))
            {
                neighs.Add(streetPaths[i].idStreetPath);
            }
        }
        neighborsId = neighs.ToArray();
        
    }
    
    //возврат Айдишника этой части улицы
    public int GetIdStreetPath()
    {
        return idStreetPath;
    }
    
    //Возврат айдишника родительской улицы
    public int GetIdStreetParent()
    {
        return idStreetParent;
    }
    
    //возврат ренты на этой части улицы
    public int GetRenta()
    {
        return renta;
    }
    
    //конструктор 
    public StreetPath(int idStreetPath, string namePath, int idStreetParent, int renta, Vector3 start, Vector3 end, bool isBridge)
    {
        this.idStreetPath = idStreetPath;
        this.idStreetParent = idStreetParent;
        this.renta = renta;
        this.start = start;
        this.end = end;
        this.isBridge = isBridge;
        this.namePath = namePath;
    }
    
    //возврат айдишников соседей
    public int[] NeighborsId
    {
        get { return neighborsId; }
    }

    public StreetPaths getEntity()
    {
        return new StreetPaths(idStreetPath, idStreetParent, namePath, renta, start.x, end.x, start.z, end.z, isBridge);
    }
}