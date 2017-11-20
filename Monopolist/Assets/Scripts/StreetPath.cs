using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetPath: MonoBehaviour
{
	private int idStreetPath;
	private int idStreetParent;
	private int renta;
	public Vector2 start;
	public Vector2 end;

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
}
