using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBwork: MonoBehaviour
{
	private int[][] players;
	private int[][] builds;
	private int[][] streets;

	private DataService ds;

	public void DBStart()
	{
		ds =  new DataService("Monopolist.db");
		if(!ds.IsExist())
			ds.CreateDB();
		
		
	}

	void Start()
	{
		DBStart();
	}
}
