using UnityEngine;

public class PathForBuy : StreetPath
{
    [SerializeField] private int idPlayer;
    private int[] builds;
    private int priceStreetPath;

    public void StepOnMe()
    {
    }

    public void Buy(Player player)
    {
        //Debug.Log("Buying it");
        idPlayer = player.IdPlayer;
        player.Money -= priceStreetPath;
    }
    
    public void Trade(int IDplayer)
    {
        idPlayer = IDplayer;
       
    }

    public PathForBuy(int idPath, string namePath, int idStreetParent, int renta, Vector3 start, Vector3 end,
        int idPlayer, int[] builds,
        int priceStreetPath, bool isBridge) : base(idPath, namePath, idStreetParent, renta, start, end, isBridge)
    {
        this.idPlayer = idPlayer;
        this.builds = builds;
        this.priceStreetPath = priceStreetPath;
        base.CanBuy = true;
    }

    public int IdPlayer
    {
        get { return idPlayer; }
    }

    public int[] Builds
    {
        get { return builds; }
    }

    public int PriceStreetPath
    {
        get { return priceStreetPath; }
    }

    public Vector3 Start
    {
        get { return start; }
    }

    public Vector3 End
    {
        get { return end; }
    }

    public bool IsBridge
    {
        get { return isBridge; }
    }

    public void TakeData(PathForBuy PathForBuy)
    {
        base.TakeData(PathForBuy);
        this.idPlayer = PathForBuy.IdPlayer;
        this.builds = PathForBuy.Builds;
        this.priceStreetPath = PathForBuy.PriceStreetPath;
    }

    public string GetBuildsName()
    {
        string result = "";
        foreach (int i in builds)
        {
            result += Camera.main.GetComponent<DBwork>().GetBuild(i).NameBuild + "\n";
        }
        return result;
    }

    public void StepOnMe(int idPlayer)
    {
        DBwork dBwork = Camera.main.GetComponent<DBwork>();

        dBwork.GetPlayerbyId(idPlayer).Money -= renta;
        dBwork.GetPlayerbyId(this.idPlayer).Money += renta;
    }

    public PathsForBuy GetEntityForBuy()
    {
        return new PathsForBuy(idStreetPath, idPlayer, priceStreetPath);
    }
}