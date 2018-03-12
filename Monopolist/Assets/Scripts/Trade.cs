using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class Trade
{
    public static ThingForTrade[,] things;

    public static void CreateListThings(Player playerFrom, Player playerfor, List<PathForBuy> pathsForTrade, int price)
    {
        things[playerFrom.IdPlayer, playerfor.IdPlayer] =
            new ThingForTrade(pathsForTrade, price, playerfor, playerFrom);
    }


    public static void TradeApply(Player playerFrom, Player playerfor)
    {

        ThingForTrade thing = things[playerFrom.IdPlayer, playerfor.IdPlayer];
        List<PathForBuy> paths = thing.PathforTrade;

        if (paths != null)
        {
            foreach (var path in paths)
            {
                path.Trade(thing.ForWhichPlayer.IdPlayer);
            }
        }

        thing.FromWhichPlayer.Money += thing.Price;
        
        TradeClear(playerFrom, playerfor);
    }

    public static void TradeClear(Player playerFrom, Player playerfor)
    {
        things[playerFrom.IdPlayer, playerfor.IdPlayer] = null;
    }
    
    
}