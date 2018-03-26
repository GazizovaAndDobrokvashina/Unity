using UnityEngine;

public class PathForBuy : StreetPath
{
    //идентификатор игрока, которому принадлежит улица
    private int idPlayer;

    //массив зданий, доступных на этой улице
    private int[] builds;

    //стоимость улицы
    private int priceStreetPath;

    //заложена ли улица
    private bool isBlocked;

    //покупка улицы игроком
    public void Buy(Player player)
    {
        idPlayer = player.IdPlayer;
        player.Money -= priceStreetPath;
    }

    //смена владельца при торговле
    public void Trade(int IDplayer)
    {
        idPlayer = IDplayer;
    }

    //конструктор класса
    public PathForBuy(int idPath, string namePath, int idStreetParent, int renta, Vector3 start, Vector3 end,
        int idPlayer, int[] builds,
        int priceStreetPath, bool isBridge, bool isBlocked) : base(idPath, namePath, idStreetParent, renta, start, end,
        isBridge)
    {
        this.idPlayer = idPlayer;
        this.builds = builds;
        this.priceStreetPath = priceStreetPath;
        base.CanBuy = true;
        this.isBlocked = isBlocked;
    }

    //вернуть идентификатор владельца
    public int IdPlayer
    {
        get { return idPlayer; }
        set { idPlayer = value; }
    }

    //вернуть список домов 
    public int[] Builds
    {
        get { return builds; }
    }

    //вернуть стоимость улицы
    public int PriceStreetPath
    {
        get { return priceStreetPath; }
    }

    //координаты начала улицы
    public Vector3 Start
    {
        get { return start; }
    }

    //координаты конца улицы
    public Vector3 End
    {
        get { return end; }
    }

    //является ли мостом улица
    public bool IsBridge
    {
        get { return isBridge; }
    }

    //поличить информацию из дб
    public void TakeData(PathForBuy PathForBuy)
    {
        base.TakeData(PathForBuy);
        this.idPlayer = PathForBuy.IdPlayer;
        this.builds = PathForBuy.Builds;
        this.priceStreetPath = PathForBuy.PriceStreetPath;
    }

    //получить строку с названиями домов на этой улице
    public string GetBuildsName()
    {
        string result = "";
        foreach (int i in builds)
        {
            result += Camera.main.GetComponent<DBwork>().GetBuild(i).NameBuild + "\n";
        }

        return result;
    }

    //снятие ренты с игрока, остановившегося на этом участке и добавление этой суммы владельцу улицы
    public void StepOnMe(int idPlayer)
    {
        DBwork dBwork = Camera.main.GetComponent<DBwork>();

        dBwork.GetPlayerbyId(idPlayer).Money -= renta;
        dBwork.GetPlayerbyId(this.idPlayer).Money += renta;
    }

    //вернуть параметры улицы для дб
    public PathsForBuy GetEntityForBuy()
    {
        return new PathsForBuy(idStreetPath, idPlayer, priceStreetPath);
    }
}