using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ThingForTrade
{
    public List<PathForBuy> PathforTrade { get; set; }

    public int Price { get; set; }

    public Player ForWhichPlayer { get; set; }

    public Player FromWhichPlayer { get; set; }

    public ThingForTrade(List<PathForBuy> pathforTrade, int price, Player forWhichPlayer, Player fromWhichPlayer)
    {
        PathforTrade = pathforTrade;
        Price = price;
        ForWhichPlayer = forWhichPlayer;
        FromWhichPlayer = fromWhichPlayer;
    }
}