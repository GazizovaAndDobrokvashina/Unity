using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DBwork: MonoBehaviour
{
	private Player[] players;
	private Build[] builds;
	private Street[] streets;

	private DataService ds;
	

	void Start()
	{
		DBStart();
	}

	public void DBStart()
	{
		ds =  new DataService("Monopolist.db");
		if(!ds.IsExist())
			ds.CreateDB();


		/*foreach (Streets streetse in ds.getStreets())
		{
			Debug.Log(streetse.NameStreet + "   " + streetse.AboutStreet);
		}*/
		
	}

	public void SetGameDB(string dbName)
	{
		ds = new DataService(dbName);
		
	
	}

	private void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
	}

	
	
	
}
