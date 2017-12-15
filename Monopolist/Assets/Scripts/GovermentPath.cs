using UnityEngine;

public class GovermentPath : StreetPath, GovermantBuild
{
    public Event[] events;

    private GameCanvas _gameCanvas;
    private void Start()
    {
        _gameCanvas = transform.Find("/Canvas").GetComponent<GameCanvas>();
    }
    
    public Event GetRandomEvent()
    {
        return events[Random.Range(1, events.Length )];
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
        if(idPlayer==1 && dBwork.GetPlayerbyId(idPlayer).isInJail())
            return;
              
        Event newEvent = GetRandomEvent();       
        dBwork.GetPlayerbyId(idPlayer).Money += newEvent.Price;
        
        if (idPlayer == 1)
        {
            _gameCanvas = dBwork.GetPlayerbyId(idPlayer).GetGameCanvas();
            _gameCanvas.ShowInfoAboutEvent(newEvent.Name + "\n" + newEvent.Info + "\n" + "Стоимость: " + newEvent.Price);
        }
    }

    public void GoToJail(int idPlayer, GameCanvas canv)
    {
        DBwork dBwork = Camera.main.GetComponent<DBwork>();
        Event newEvent = events[0];
        
        Debug.Log(newEvent.Name);
        canv.ShowInfoAboutEvent(newEvent.Name + "\n" + newEvent.Info);
        dBwork.GetPlayerbyId(idPlayer).InJail(3);
        
        dBwork.GetPlayerbyId(idPlayer).Money += newEvent.Price;
    }
    
}