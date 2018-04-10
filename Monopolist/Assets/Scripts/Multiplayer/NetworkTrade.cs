using System.Collections.Generic;

public static class NetworkTrade
{
    //список торговых предложений от одного игрока другому, где индексы в массиве - ID игрока
    public static List<NetworkThingForTrade>[,] things;

    //создание списка предложений
    public static void CreateListThings(NetworkPlayer playerFrom, NetworkPlayer playerFor)
    {
        //если вдруг массив не инициализирован
        if (things == null)
        {
            //захардкоджено, исправить; размерность количество игроков на половину (?) количества игроков
            things = new List<NetworkThingForTrade>[10, 10];
        }

        //инициализация списка предложений между конкретными игроками
        if (playerFrom.IdPlayer < playerFor.IdPlayer)
        {
            things[playerFrom.IdPlayer, playerFor.IdPlayer] =
                new List<NetworkThingForTrade>();
        }
        else
        {
            things[playerFor.IdPlayer, playerFrom.IdPlayer] =
                new List<NetworkThingForTrade>();
        }
    }

    //добавление в список предложений товара 
    public static void AddItemToList(NetworkPlayer playerFrom, NetworkPlayer playerFor, NetworkPathForBuy path)
    {
        if (playerFrom.IdPlayer < playerFor.IdPlayer)
        {
            things[playerFrom.IdPlayer, playerFor.IdPlayer].Add(new NetworkThingForTrade(path, 0, playerFor, playerFrom));
        }
        else
        {
            things[playerFor.IdPlayer, playerFrom.IdPlayer].Add(new NetworkThingForTrade(path, 0, playerFor, playerFrom));
        }
    }

    //Добавление денег в список
    public static void AddMoneyToList(NetworkPlayer playerFrom, NetworkPlayer playerFor, int price)
    {
        if (playerFrom.IdPlayer < playerFor.IdPlayer)
        {
            things[playerFrom.IdPlayer, playerFor.IdPlayer].Add(new NetworkThingForTrade(null, price, playerFor, playerFrom));
        }
        else
        {
            things[playerFor.IdPlayer, playerFrom.IdPlayer].Add(new NetworkThingForTrade(null, price, playerFor, playerFrom));
        }
    }

    //удаление из списка предложений товара
    public static void RemoveItemFromList(NetworkPlayer playerFrom, NetworkPlayer playerFor, NetworkPathForBuy path)
    {
        if (playerFrom.IdPlayer < playerFor.IdPlayer)
        {
            foreach (NetworkThingForTrade thingForTrade in things[playerFrom.IdPlayer, playerFor.IdPlayer])
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
            foreach (NetworkThingForTrade thingForTrade in things[playerFor.IdPlayer, playerFrom.IdPlayer])
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
    public static void TradeApply(NetworkPlayer playerFrom, NetworkPlayer playerFor, NetworkGameCanvas GC,
        int moneyFromFirstPlayer,
        int moneyFromSecondPlayer)
    {
        //доавление денег, которые игроки зотят передать друг другу
        AddMoneyToList(playerFrom, playerFor, moneyFromFirstPlayer);
        AddMoneyToList(playerFor, playerFrom, moneyFromSecondPlayer);

        //очистка канвы торговли
        GC.ClearTradeMenu();

        if (playerFrom.IdPlayer < playerFor.IdPlayer)
        {
            foreach (NetworkThingForTrade thingForTrade in things[playerFrom.IdPlayer, playerFor.IdPlayer])

                if (thingForTrade.PathforTrade != null)
                {
                    thingForTrade.PathforTrade.IdPlayer = thingForTrade.ForWhichPlayer.IdPlayer;
                }
                else
                {
                    thingForTrade.ForWhichPlayer.Money += thingForTrade.Price;
                    thingForTrade.FromWhichPlayer.Money -= thingForTrade.Price;
                }
        }
        else if (playerFrom.IdPlayer > playerFor.IdPlayer)
        {
            foreach (NetworkThingForTrade thingForTrade in things[playerFor.IdPlayer, playerFrom.IdPlayer])
                if (thingForTrade.PathforTrade != null)
                {
                    thingForTrade.PathforTrade.IdPlayer = thingForTrade.ForWhichPlayer.IdPlayer;
                }
                else
                {
                    thingForTrade.ForWhichPlayer.Money += thingForTrade.Price;
                    thingForTrade.FromWhichPlayer.Money -= thingForTrade.Price;
                }
        }
        //если игрок закладывает
        else
        {
            //для каждой улице в списке
            foreach (NetworkThingForTrade thingForTrade in things[playerFor.IdPlayer, playerFrom.IdPlayer])
            {
                if (thingForTrade.PathforTrade != null)
                {
                    BlockStreetPath(thingForTrade.PathforTrade, playerFor);
                }
            }
        }

        TradeClear(playerFor, playerFrom);
    }

    //очистка списка предложений между конкретными игроками
    private static void TradeClear(NetworkPlayer playerFrom, NetworkPlayer playerFor)
    {
        if (playerFrom.IdPlayer < playerFor.IdPlayer)
        {
            things[playerFrom.IdPlayer, playerFor.IdPlayer] = new List<NetworkThingForTrade>();
        }
        else
        {
            things[playerFor.IdPlayer, playerFrom.IdPlayer] = new List<NetworkThingForTrade>();
        }
    }

    //блокировка улицы и возмещение её полной стоимости вместе с построенными зданиями игроку
    private static void BlockStreetPath(NetworkPathForBuy pathForBuy, NetworkPlayer ownerPlayer)
    {
        if (!pathForBuy.IsBlocked)
        {
            pathForBuy.IsBlocked = true;
            int sum = 0;
            sum += pathForBuy.PriceStreetPath;
            ownerPlayer.Money += sum;
        }

        else
        {
            UnBlockStreetPath(pathForBuy, ownerPlayer);
        }
    }

    //разблокировка улицы при выплате полной стоимости
    private static void UnBlockStreetPath(NetworkPathForBuy pathForBuy, NetworkPlayer ownerPlayer)
    {
        if (pathForBuy.IsBlocked && ownerPlayer.Money >= pathForBuy.PriceStreetPath)
        {
            pathForBuy.IsBlocked = false;
            ownerPlayer.Money -= pathForBuy.PriceStreetPath;
        }
    }
}