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

    public int IdStreet1
    {
        get { return IdStreet; }
    }

    public string NameStreet1
    {
        get { return NameStreet; }
    }

    public string AboutStreet1
    {
        get { return AboutStreet; }
    }

    public int[] Paths1
    {
        get { return Paths; }
    }
}