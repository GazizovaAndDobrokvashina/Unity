using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GovermentPath : StreetPath, GovermantBuild
{
    //массив событий, доступных на этой улице   
    public Event[] events;

    //ссылка на игровую канву
    private GameCanvas _gameCanvas;

    //создаемм ссылку на текущую канву
    private void Start()
    {
        _gameCanvas = transform.Find("/Canvas").GetComponent<GameCanvas>();
    }

    //выбираем случайное событие 
    public Event GetRandomEvent()
    {
        
        return events[Random.Range(1, events.Length)];
    }

    //конструктор класса
    public GovermentPath(int idStreetPath, string namePath, int idStreetParent, int renta, Vector3 start, Vector3 end,
        bool isBridge, String nameOfPrefab,
        Event[] events) : base(idStreetPath, namePath, idStreetParent, renta, start, end, isBridge, nameOfPrefab)
    {
        this.events = events;
        base.canBuy = false;
    }

    //получить информацию из бд
    public void TakeData(GovermentPath govermentPath)
    {
        base.TakeData(govermentPath);
        this.events = govermentPath.events;
    }

    //вызов событий, если игрок остановился на этом участке
    public void StepOnMe(int idPlayer)
    {
        DBwork dBwork = Camera.main.GetComponent<DBwork>();
        if (idPlayer == 1 && dBwork.GetPlayerbyId(idPlayer).isInJail() )
            return;

        Event newEvent = GetRandomEvent();
        dBwork.GetPlayerbyId(idPlayer).Money += newEvent.Price;

        if (idPlayer == 1)
        {
            _gameCanvas = transform.Find("/Canvas").GetComponent<GameCanvas>();
            _gameCanvas.ShowInfoAboutEvent(newEvent.Name + "\n" + newEvent.Info + "\n" + "Стоимость: " +
                                           newEvent.Price);
        }
    }

    
}