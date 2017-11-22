using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build : MonoBehaviour
{
	private int idBuild;

	private int idStreetPath;

	private int priceBuild;

	private bool enable;

	public void build()
	{
		
	}

	public Build(int idBuild, int idStreetPath, int priceBuild, bool enable)
	{
		this.idBuild = idBuild;
		this.idStreetPath = idStreetPath;
		this.priceBuild = priceBuild;
		this.enable = enable;
	}
}
