using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Street : MonoBehaviour
{
	private int IdStreet;
	private string NameStreet;
	private string AboutStreet;
	private int[] Paths;


	public Street(int idStreet, string nameStreet, string aboutStreet, int[] paths)
	{
		IdStreet = idStreet;
		NameStreet = nameStreet;
		AboutStreet = aboutStreet;
		Paths = paths;
	}
}
