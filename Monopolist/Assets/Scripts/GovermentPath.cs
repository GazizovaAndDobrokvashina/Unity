using UnityEngine;

public class GovermentPath :  StreetPath, GovermantBuild
{
    public Event[] events;

    public Event GetRandomEvent()
    {
        return events[Random.Range(0, events.Length-1)];
    }
}
