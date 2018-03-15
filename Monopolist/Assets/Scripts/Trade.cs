using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class Trade
{
    //список торговых предложений от одного игрока другому, где индексы в массиве - ID игрока
    public static List<ThingForTrade>[,] things;

    //создание списка предложений
    public static void CreateListThings(Player playerFrom, Player playerFor)
    {
        //если вдруг массив не инициализирован
        if (things == null)
        {
            //захардкоджено, исправить; размерность количество игроков на половину (?) количества игроков
            things = new List<ThingForTrade>[6, 6];
        }

        //инициализация списка предложений между конкретными игроками
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

    //добавление в список предложений товара (дописать сюда денежку)
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

    //удаление из списка предложений товара
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

    //применение результата торговли к игровым объектам
    public static void TradeApply(Player playerFrom, Player playerFor, GameCanvas GC)
    {
        //очистка канвы торговли
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

    //очистка списка предложений между конкретными игроками
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