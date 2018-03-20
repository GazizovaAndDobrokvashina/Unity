using SQLite4Unity3d;
using UnityEngine;


public class PathsForBuy
{
    [PrimaryKey]
    public int IdPathForBuy { get; set; }

    public int IdPlayer { get; set; }
    public int PriceStreetPath { get; set; }

    public bool isBlocked { get; set; }

    public PathsForBuy()
    {
    }

    public PathForBuy GetPathForBuy(StreetPaths streetPaths, int[]builds)
    {
        Vector3 start = new Vector3((float) streetPaths.StartX, 0, (float) streetPaths.StartY);
        Vector3 end = new Vector3((float) streetPaths.EndX, 0, (float) streetPaths.EndY);
        return new PathForBuy(IdPathForBuy, streetPaths.NamePath, streetPaths.IdStreetParent, streetPaths.Renta, start, end, IdPlayer, builds,
            PriceStreetPath, streetPaths.IsBridge, isBlocked );
    }

    public PathsForBuy(int idPathForBuy, int idPlayer, int priceStreetPath)
    {
        IdPathForBuy = idPathForBuy;
        IdPlayer = idPlayer;
        PriceStreetPath = priceStreetPath;
    }
}