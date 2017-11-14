using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;


public class StreetPaths {

	[PrimaryKey, AutoIncrement]
 	public int IdStreetPath{ get; set; }
 	public int IdStreetParent{ get; set; }
 	public int Renta{ get; set; }
 	public double StartX { get; set; }
 	public double EndX { get; set; }
 	public double StartY { get; set; }
 	public double EndY { get; set; }
 }
