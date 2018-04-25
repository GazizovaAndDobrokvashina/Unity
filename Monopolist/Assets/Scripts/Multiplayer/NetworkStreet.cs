using UnityEngine.Networking;
using UnityEngine;

public class NetworkStreet :Photon.MonoBehaviour
{
    //идентификатор монополии
    private int idStreet;

    //название монополии
    private string nameStreet;

    //о монополии
    private string aboutStreet;

    //массив участков, которые входят в монополию
    private int[] paths;

    //получить информацию о монополии из бд
    public NetworkStreet(int idStreet, string nameStreet, string aboutStreet, int[] paths)
    {
        this.idStreet = idStreet;
        this.nameStreet = nameStreet;
        this.aboutStreet = aboutStreet;
        this.paths = paths;
    }


    public int IdStreet
    {
        get { return idStreet; }
    }

    public string NameStreet
    {
        get { return nameStreet; }
    }

    //получить дополнительную информацию о монополии
    public string AboutStreet
    {
        get { return aboutStreet; }
    }

    //получить массив участков, принадлежащих этой монополии
    public int[] Paths
    {
        get { return paths; }
    }

    //получить информацию о монополии
    public Streets getEntity()
    {
        return new Streets(idStreet, nameStreet, aboutStreet);
    }
}