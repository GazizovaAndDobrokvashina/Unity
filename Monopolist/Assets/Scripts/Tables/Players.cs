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
    public bool IsBot { get; set; }

    public Player GetPlayer()
    {
        Vector3 position = new Vector3((float) CoordinateX, 0, (float) CoordinateY);
        return new Player(IdPlayer,NickName, Money, IsBankrupt, IsBot, position);
    }

    public NetworkPlayer GetNetworkPlayer()
    {
        Vector3 position = new Vector3((float) CoordinateX, 0, (float) CoordinateY);
        return new NetworkPlayer(IdPlayer,NickName, Money, IsBankrupt, IsBot, position);
    }

    public Players(int idPlayer, string nickName, int money, double coordinateX, double coordinateY, bool isBankrupt, bool isBot)
    {
        IdPlayer = idPlayer;
        NickName = nickName;
        Money = money;
        CoordinateX = coordinateX;
        CoordinateY = coordinateY;
        IsBankrupt = isBankrupt;
        IsBot = isBot;
    }

    public Players()
    {
    }

}