using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;


public class StreetPaths
{
    [PrimaryKey, AutoIncrement]
    public int IdStreetPath { get; set; }

    public int IdStreetParent { get; set; }
    public string NamePath { get; set; }
    public int Renta { get; set; }
    public double StartX { get; set; }
    public double EndX { get; set; }
    public double StartY { get; set; }
    public double EndY { get; set; }
    public bool IsBridge { get; set; }
    public string NameOfPrefab { get; set; }


    public GovermentPath GetGovermentPath(Event[] events)
    {
        Vector3 start = new Vector3((float) StartX, 0, (float) StartY);
        Vector3 end = new Vector3((float) EndX, 0, (float) EndY);
        return new GovermentPath(IdStreetPath, NamePath, IdStreetParent, Renta, start, end, IsBridge, NameOfPrefab, events);
    }
    
    public NetworkGovermentPath GetNetworkGovermentPath(Event[] events)
    {
        Vector3 start = new Vector3((float) StartX, 0, (float) StartY);
        Vector3 end = new Vector3((float) EndX, 0, (float) EndY);
        return new NetworkGovermentPath(IdStreetPath, NamePath, IdStreetParent, Renta, start, end, IsBridge, NameOfPrefab, events);
    }


    public StreetPaths(int idStreetPath, int idStreetParent, string namePath, int renta, double startX, double endX, double startY, double endY, bool isBridge, string nameOfPrefab)
    {
        IdStreetPath = idStreetPath;
        IdStreetParent = idStreetParent;
        NamePath = namePath;
        Renta = renta;
        StartX = startX;
        EndX = endX;
        StartY = startY;
        EndY = endY;
        IsBridge = isBridge;
        NameOfPrefab = nameOfPrefab;
    }

    public StreetPaths()
    {
    }
}