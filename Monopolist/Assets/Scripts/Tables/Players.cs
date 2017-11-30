using SQLite4Unity3d;
using UnityEngine;

public class Players
{
    [PrimaryKey, AutoIncrement]
    public int IdPlayer { get; set; }

    public string NickName { get; set; }
    public int Money { get; set; }
    public double CoordinateX { get; set; }
    public double CoordinateY { get; set; }
    public bool IsBankrupt { get; set; }

    public Player GetPlayer()
    {
        Vector3 position = new Vector3((float) CoordinateX, 0, (float) CoordinateY);
        return new Player(IdPlayer,NickName, Money, IsBankrupt, position);
    }
}