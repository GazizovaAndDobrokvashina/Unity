using UnityEngine;

public class PathForBuy : StreetPath
{
    private int idPlayer;
    private int[] builds;
    private int priceStreetPath;

    public void StepOnMe()
    {
    }

    public void Buy()
    {
    }

    public PathForBuy(int idPath, string namePath, int idStreetParent, int renta, Vector3 start, Vector3 end, int idPlayer, int[] builds,
        int priceStreetPath, bool isBridge) : base(idPath, namePath, idStreetParent, renta, start, end, isBridge)
    {
        this.idPlayer = idPlayer;
        this.builds = builds;
        this.priceStreetPath = priceStreetPath;
    }
}