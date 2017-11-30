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


    public GovermentPath GetGovermentPath(Event[] events)
    {
        Vector3 start = new Vector3((float) StartX, 0, (float) StartY);
        Vector3 end = new Vector3((float) EndX, 0, (float) EndY);
        return new GovermentPath(IdStreetPath, NamePath, IdStreetParent, Renta, start, end, IsBridge, events);
    }
}