using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetPath: MonoBehaviour
{
	private int idStreetPath;
	private int idStreetParent;
	private int renta;
	public Vector3 start;
	public Vector3 end;
	public bool isBridge;
	public  int[] neighborsId;
	
	public void StepOnMe()
	{
		
	}

	public void TakeData(StreetPath streetPath)
	{
		this.idStreetParent = streetPath.GetIdStreetParent();
		this.idStreetPath = streetPath.GetIdStreetPath();
		this.renta = streetPath.GetRenta();
		this.start = streetPath.start;
		this.end = streetPath.end;
		this.isBridge = streetPath.isBridge;
	}

	public void GetNeighbors()
	{
		DBwork ds  =Camera.main.GetComponent<DBwork>();
		List<int> neighs = new List<int>();

		StreetPath[] streetPaths = ds.GetAllPaths();
		for(int i = 1; i < streetPaths.Length; i++) {
			if (streetPaths[i].end.Equals(end) || streetPaths[i].start.Equals(start) || streetPaths[i].end.Equals(start) ||
			    streetPaths[i].start.Equals(end))
			{
				neighs.Add(streetPaths[i].idStreetPath);
			}
	}
		neighborsId = neighs.ToArray();
	}

	public int GetIdStreetPath()
	{
		return idStreetPath;
	}
	
	public int GetIdStreetParent()
	{
		return idStreetParent;
	}
	
	public int GetRenta()
	{
		return renta;
	}

	public StreetPath(int idStreetPath, int idStreetParent, int renta, Vector3 start, Vector3 end, bool isBridge)
	{
		this.idStreetPath = idStreetPath;
		this.idStreetParent = idStreetParent;
		this.renta = renta;
		this.start = start;
		this.end = end;
		this.isBridge = isBridge;
	}

	public int[] NeighborsId
	{
		get { return neighborsId; }
	}
}
