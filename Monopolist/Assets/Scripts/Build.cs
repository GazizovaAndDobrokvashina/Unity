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
    
    public void build()
    {
        enabled = true;
    }

    public Build(int idBuild, string nameBuild, string aboutBuild, int idStreetPath, int priceBuild, bool enable)
    {
        this.idBuild = idBuild;
        this.idStreetPath = idStreetPath;
        this.priceBuild = priceBuild;
        this.enable = enable;
        this.nameBuild = nameBuild;
        this.aboutBuild = aboutBuild;
    }

    public void TakeData(Build build)
    {
        idBuild = build.IdBuild;
        nameBuild = build.nameBuild;
        aboutBuild = build.aboutBuild;
        idStreetPath = build.IdStreetPath;
        priceBuild = build.PriceBuild;
        enable = build.Enable;
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