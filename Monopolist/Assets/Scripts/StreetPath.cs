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
}
