using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mapinfo 
{
	private int[][] players;
	private int[][] builds;
	private int[][] streets;
	
	public static Mapinfo current;

	public Mapinfo()
	{
		current = this;
	}
}
