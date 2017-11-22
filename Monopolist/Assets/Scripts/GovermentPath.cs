using UnityEngine;

public class GovermentPath :  StreetPath, GovermantBuild
{
    public Event[] events;

    public Event GetRandomEvent()
    {
        return events[Random.Range(0, events.Length-1)];
    }

    public GovermentPath(int idStreetPath, int idStreetParent, int renta, Vector3 start, Vector3 end, bool isBridge, Event[] events) : base(idStreetPath, idStreetParent, renta, start, end, isBridge)
    {
        events = events;
    }
}
