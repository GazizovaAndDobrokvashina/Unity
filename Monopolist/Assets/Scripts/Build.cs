using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build : MonoBehaviour
{
    private int idBuild;
    private string nameBuild;
    private string aboutBuild;
    private int idStreetPath;
    private int priceBuild;
    private bool enable;
    private Vector3 place;
    
    public void build(Player player)
    {
        player.Money -= priceBuild;
        enabled = true;
        gameObject.SetActive(true);
    }

    public Build(int idBuild, string nameBuild, string aboutBuild, int idStreetPath, int priceBuild, bool enable, double posX, double posY)
    {
        this.idBuild = idBuild;
        this.idStreetPath = idStreetPath;
        this.priceBuild = priceBuild;
        this.enable = enable;
        this.nameBuild = nameBuild;
        this.aboutBuild = aboutBuild;
        this.place = new Vector3((float)posX, 0, (float)posY);
    }

    public void TakeData(Build build)
    {
        idBuild = build.IdBuild;
        nameBuild = build.nameBuild;
        aboutBuild = build.aboutBuild;
        idStreetPath = build.IdStreetPath;
        priceBuild = build.PriceBuild;
        enable = build.Enable;
        place = build.Place;
    }

    public Vector3 Place
    {
        get { return place; }
    }

    public int IdBuild
    {
        get { return idBuild; }
    }

    public int IdStreetPath
    {
        get { return idStreetPath; }
    }

    public int PriceBuild
    {
        get { return priceBuild; }
    }

    public bool Enable
    {
        get { return enable; }
    }

    public string NameBuild
    {
        get { return nameBuild; }
    }

    public string AboutBuild
    {
        get { return aboutBuild; }
    }
}