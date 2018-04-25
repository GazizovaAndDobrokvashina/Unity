using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkBuild : Photon.MonoBehaviour
{
    //идентификатор здания
    private int idBuild;

    //название здания
    private string nameBuild;

    //дополнительная информация о здании
    private string aboutBuild;

    //идентификатор улицы, на которой стоит здание
    private int idStreetPath;

    //стоимость посторйки здания
    private int priceBuild;

    //построено ли уже здание 
    private bool enable;

    //координаты на карте 
    private Vector3 place;

    //построить здание
    public void build(NetworkPlayer player)
    {
        player.Money -= priceBuild;
        enable = true;
        gameObject.SetActive(true);
    }

    //конструктор класса
    public NetworkBuild(int idBuild, string nameBuild, string aboutBuild, int idStreetPath, int priceBuild, bool enable,
        double posX, double posY)
    {
        this.idBuild = idBuild;
        this.idStreetPath = idStreetPath;
        this.priceBuild = priceBuild;
        this.enable = enable;
        this.nameBuild = nameBuild;
        this.aboutBuild = aboutBuild;
        this.place = new Vector3((float) posX, 0, (float) posY);
    }

    //заполнить данные о здании из бд
    public void TakeData(NetworkBuild build)
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

    //предоставить информацию о здании для бд
    public Builds getEntity()
    {
        return new Builds(idBuild, nameBuild, aboutBuild, idStreetPath, priceBuild, enable, place.x, place.z);
    }
}