using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ThingForTrade
{
    //улица, которую хотят продать
    public PathForBuy PathforTrade { get; set; }

    //стоимость, за котоую хотят продать улицу
    public int Price { get; set; }

    //для какого игрока предназначается улица
    public Player ForWhichPlayer { get; set; }

    //какой игрок продает улицу
    public Player FromWhichPlayer { get; set; }

    //кнструктор
    public ThingForTrade(PathForBuy pathforTrade, int price, Player forWhichPlayer, Player fromWhichPlayer)
    {
        PathforTrade = pathforTrade;
        Price = price;
        ForWhichPlayer = forWhichPlayer;
        FromWhichPlayer = fromWhichPlayer;
    }
}