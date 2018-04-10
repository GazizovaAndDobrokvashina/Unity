using System;
using UnityEngine;
using Random = UnityEngine.Random;
    public class NetworkGovermentPath : NetworkStreetPath
    {
        //массив событий, доступных на этой улице   
        public Event[] events;

        //ссылка на игровую канву
        private NetworkGameCanvas _gameCanvas;

        //выбираем случайное событие 
        public Event GetRandomEvent()
        {
        
            return events[Random.Range(1, events.Length)];
        }

        //конструктор класса
        public NetworkGovermentPath(int idStreetPath, string namePath, int idStreetParent, int renta, Vector3 start, Vector3 end,
            bool isBridge, String nameOfPrefab,
            Event[] events) : base(idStreetPath, namePath, idStreetParent, renta, start, end, isBridge, nameOfPrefab)
        {
            this.events = events;
            base.canBuy = false;
        }

        //получить информацию из бд
        public void TakeData(NetworkGovermentPath govermentPath)
        {
            base.TakeData(govermentPath);
            this.events = govermentPath.events;
        }

        //вызов событий, если игрок остановился на этом участке
        public void StepOnMe(int idPlayer)
        {
            NetworkDBwork dBwork = Camera.main.GetComponent<NetworkDBwork>();
            if (idPlayer == 1 && dBwork.GetPlayerbyId(idPlayer).isInJail() )
                return;

            Event newEvent = GetRandomEvent();
            dBwork.GetPlayerbyId(idPlayer).Money += newEvent.Price;

            if (idPlayer == 1)
            {
                if (_gameCanvas == null)
                {
                    _gameCanvas = dBwork.GetNetworkGameCanvas();
                }

                _gameCanvas.ShowInfoAboutEvent(newEvent.Name + "\n" + newEvent.Info + "\n" + "Стоимость: " +
                                               newEvent.Price);
            }
        }

    }
