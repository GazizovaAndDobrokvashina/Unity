using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Street : MonoBehaviour
{
    private int idStreet;
    private string nameStreet;
    private string aboutStreet;
    private int[] paths;


    public Street(int idStreet, string nameStreet, string aboutStreet, int[] paths)
    {
        this.idStreet = idStreet;
        this.nameStreet = nameStreet;
        this.aboutStreet = aboutStreet;
        this.paths = paths;
    }

    public int IdStreet1
    {
        get { return idStreet; }
    }

    public string NameStreet1
    {
        get { return nameStreet; }
    }

    public string AboutStreet1
    {
        get { return aboutStreet; }
    }

    public int[] Paths1
    {
        get { return paths; }
    }

    public Streets getEntity()
    {
        return new Streets(idStreet, nameStreet, aboutStreet);
    }
}