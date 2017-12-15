using UnityEngine;

public class GovermentPath : StreetPath, GovermantBuild
{
    public Event[] events;

    public Event GetRandomEvent()
    {
        return events[Random.Range(0, events.Length )];
    }

    public GovermentPath(int idStreetPath, string namePath, int idStreetParent, int renta, Vector3 start, Vector3 end,
        bool isBridge,
        Event[] events) : base(idStreetPath, namePath, idStreetParent, renta, start, end, isBridge)
    {
        this.events = events;
        base.canBuy = false;
    }

    public void TakeData(GovermentPath govermentPath)
    {
        base.TakeData(govermentPath);
        this.events = govermentPath.events;
    }

    public void StepOnMe(int idPlayer)
    {
        DBwork dBwork = Camera.main.GetComponent<DBwork>();
        Event newEvent = GetRandomEvent();
        dBwork.GetPlayerbyId(idPlayer).Money += newEvent.Price;
    }
    
}