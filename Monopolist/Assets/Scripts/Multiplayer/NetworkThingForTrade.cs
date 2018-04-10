using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class NetworkThingForTrade
{
    //улица, которую хотят продать
    public NetworkPathForBuy PathforTrade { get; set; }

    //стоимость, за котоую хотят продать улицу
    public int Price { get; set; }

    //для какого игрока предназначается улица
    public NetworkPlayer ForWhichPlayer { get; set; }

    //какой игрок продает улицу
    public NetworkPlayer FromWhichPlayer { get; set; }

    //кнструктор
    public NetworkThingForTrade(NetworkPathForBuy pathforTrade, int price, NetworkPlayer forWhichPlayer, NetworkPlayer fromWhichPlayer)
    {
        PathforTrade = pathforTrade;
        Price = price;
        ForWhichPlayer = forWhichPlayer;
        FromWhichPlayer = fromWhichPlayer;
    }
}