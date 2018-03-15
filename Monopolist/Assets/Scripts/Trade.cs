using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class Trade
{
    public static List<ThingForTrade>[,] things;

    public static void CreateListThings(Player playerFrom, Player playerFor)
    {
        if (things == null)
        {
            things = new List<ThingForTrade>[6, 6];
        }

        if (playerFrom.IdPlayer < playerFor.IdPlayer)
        {
            things[playerFrom.IdPlayer, playerFor.IdPlayer] =
                new List<ThingForTrade>();
        }
        else
        {
            things[playerFor.IdPlayer, playerFrom.IdPlayer] =
                new List<ThingForTrade>();
        }
    }

    public static void AddItemToList(Player playerFrom, Player playerFor, PathForBuy path)
    {
        if (playerFrom.IdPlayer < playerFor.IdPlayer)
        {
            things[playerFrom.IdPlayer, playerFor.IdPlayer].Add(new ThingForTrade(path, 0, playerFor, playerFrom));
        }
        else
        {
            things[playerFor.IdPlayer, playerFrom.IdPlayer].Add(new ThingForTrade(path, 0, playerFor, playerFrom));
        }
    }

    public static void RemoveItemFromList(Player playerFrom, Player playerFor, PathForBuy path)
    {
        if (playerFrom.IdPlayer < playerFor.IdPlayer)
        {
            foreach (ThingForTrade thingForTrade in things[playerFrom.IdPlayer, playerFor.IdPlayer])
            {
                if (thingForTrade.ForWhichPlayer == playerFor && thingForTrade.FromWhichPlayer == playerFrom &&
                    thingForTrade.PathforTrade == path)
                {
                    things[playerFrom.IdPlayer, playerFor.IdPlayer].Remove(thingForTrade);
                    break;
                }
            }
        }
        else
        {
            foreach (ThingForTrade thingForTrade in things[playerFor.IdPlayer, playerFrom.IdPlayer])
            {
                if (thingForTrade.ForWhichPlayer == playerFor && thingForTrade.FromWhichPlayer == playerFrom &&
                    thingForTrade.PathforTrade == path)
                {
                    things[playerFor.IdPlayer, playerFrom.IdPlayer].Remove(thingForTrade);
                    break;
                }
            }
        }
    }

    public static void TradeApply(Player playerFrom, Player playerFor, GameCanvas GC)
    {
        GC.ClearTradeMenu();

        if (playerFrom.IdPlayer < playerFor.IdPlayer)
        {
            foreach (ThingForTrade thingForTrade in things[playerFrom.IdPlayer, playerFor.IdPlayer])
            {
                thingForTrade.PathforTrade.IdPlayer = thingForTrade.ForWhichPlayer.IdPlayer;
            }

            TradeClear(playerFrom, playerFor);
        }
        else
        {
            foreach (ThingForTrade thingForTrade in things[playerFor.IdPlayer, playerFrom.IdPlayer])
            {
                thingForTrade.PathforTrade.IdPlayer = thingForTrade.ForWhichPlayer.IdPlayer;
            }

            TradeClear(playerFor, playerFrom);
        }
    }

    private static void TradeClear(Player playerFrom, Player playerFor)
    {
        if (playerFrom.IdPlayer < playerFor.IdPlayer)
        {
            things[playerFrom.IdPlayer, playerFor.IdPlayer] = new List<ThingForTrade>();
        }
        else
        {
            things[playerFor.IdPlayer, playerFrom.IdPlayer] = new List<ThingForTrade>();
        }
    }
}