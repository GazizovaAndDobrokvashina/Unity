using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToDoList
{
     /**
     * 1. дописать выключение кнопки зданий и информации о ней, если покидаем улицу (пока что корявый вариант)
     * 2. дописать создане новой игры в дбворке для онлайна и разных городов
     * 3. написать переключение между камерами на кнопочку + смена режима между орто и перспективой
     * 4. Дописать методы в  GameCanvas, когда появятся здания и допишутся методы, которые нужно здесь вызвать (торговля, покупки, здания и прочее)
     * 5. прописать сохранение и загрузку путей в SaveLoad(?) и Ways
     * 6. сделать удаление сохраненных игр
     * 7. сделать регулировку громкости
     * 8. докомментировать MouseController
     * 9. сделать отображение списка улиц только нашего игрока
     * 10. прописать для ботов мухлёж и в целом более продивнутый интеллект
     * 11. торговля между игроками
     * 12. прописать фризы при открытии меню паузы
     * 13. прописать на какие улицы можно сходить за раз при виде от первого лица и от третьего
     * 14. добавить проверку на запрет прохождения одной улицы дважды за один ход
      * 15. вернуть на место количество выпадающих ходов в плеере.
     */
     
     
     
//     Streets[] streets = new[]
//        {
//            new Streets {NameStreet = "Яблочная", AboutStreet = "3 части"},
//            new Streets {NameStreet = "Томатная", AboutStreet = "3 части"},
//            new Streets {NameStreet = "Мандариновая", AboutStreet = "3 части, первая парк"},
//            new Streets {NameStreet = "Морская", AboutStreet = "3 части"},
//            new Streets {NameStreet = "Сретенка", AboutStreet = "2 части, парк и суд"},
//            new Streets {NameStreet = "БэтУлица", AboutStreet = "3 части"},
//            new Streets {NameStreet = "Баклажановая", AboutStreet = "3 части"},
//            new Streets {NameStreet = "Орная", AboutStreet = "2 части"},
//            new Streets {NameStreet = "Горная", AboutStreet = "2 части"},
//            new Streets {NameStreet = "Виноградная, AboutStreet = "3 части"},
//            new Streets {NameStreet = "Праздничная", AboutStreet = "3 части"},
//            new Streets {NameStreet = "Угольная", AboutStreet = "2 части"},
//            new Streets {NameStreet = "Апельсиновая", AboutStreet = "3 части"},
//            new Streets {NameStreet = "Ночная", AboutStreet = "4 части"},
//            new Streets {NameStreet = "Единороговая", AboutStreet = "3 части"},
//            new Streets {NameStreet = "Седановая", AboutStreet = "3 части"},
//            new Streets {NameStreet = "Транспортная", AboutStreet = "3 части"},
//            new Streets {NameStreet = "Коммунальная", AboutStreet = "2 части"}
//        };
//
//        StreetPaths[] pathses = new[]
//        {
//            new StreetPaths {Renta = 25, NamePath = "Желтая 1", IdStreetParent = 1, StartX = 5.63, StartY = -5.64, EndX = -1.57, EndY = -5.64, IsBridge = false},
//            new StreetPaths {Renta = 20, NamePath = "Желтая 2", IdStreetParent = 1, StartX = -1.57, StartY = -5.64, EndX = -5.68, EndY = -5.64, IsBridge = false},
//            new StreetPaths {Renta = 20, NamePath = "Красная 1", IdStreetParent = 2, StartX = -5.68, StartY = -5.64, EndX = -5.68, EndY = -1.58, IsBridge = false},
//            new StreetPaths {Renta = 25, NamePath = "Красная 2", IdStreetParent = 2, StartX = -5.68, StartY = -1.58, EndX = -5.68, EndY = 5.62, IsBridge = false},
//            new StreetPaths {Renta = 20, NamePath = "Зеленая 1", IdStreetParent = 3, StartX = -5.68, StartY = 5.62, EndX = -2.63, EndY = 5.62, IsBridge = false},
//            new StreetPaths {Renta = 25, NamePath = "Зеленая 2", IdStreetParent = 3, StartX = -2.63, StartY = 5.62, EndX = 5.63, EndY = 5.62, IsBridge = false},
//            new StreetPaths {Renta = 20, NamePath = "Синяя 1", IdStreetParent = 4, StartX = 5.63, StartY = 5.62, EndX = 5.63, EndY = 1.58, IsBridge = false},
//            new StreetPaths {Renta = 25, NamePath = "Синяя 2", IdStreetParent = 4, StartX = 5.63, StartY = 1.58, EndX = 5.63, EndY = -5.64, IsBridge = false},
//            new StreetPaths {Renta = 15, NamePath = "Розовая", IdStreetParent = 5, StartX = -1.57, StartY = -5.64, EndX = -1.57, EndY = -2.74, IsBridge = true},
//            new StreetPaths {Renta = 15, NamePath = "Фиолетовая", IdStreetParent = 6, StartX = -5.68, StartY = -1.58, EndX = -2.68, EndY = -1.58, IsBridge = true},
//            new StreetPaths {Renta = 15, NamePath = "Салатовая 1", IdStreetParent = 7, StartX = -2.63, StartY = 5.62, EndX = -2.63, EndY = 2.68, IsBridge = true},
//            new StreetPaths {Renta = 15, NamePath = "Коричневая", IdStreetParent = 8, StartX = 5.63, StartY = 1.58, EndX = 2.65, EndY = 1.58, IsBridge = true},
//            new StreetPaths {Renta = 25, NamePath = "Голубая 1", IdStreetParent = 9, StartX = 2.65, StartY = -2.74, EndX = -1.57, EndY = -2.74, IsBridge = false},
//            new StreetPaths {Renta = 15, NamePath = "Голубая 2", IdStreetParent = 9, StartX = -1.57, StartY = -2.74, EndX = -2.68, EndY = -1.58, IsBridge = false},
//            new StreetPaths {Renta = 15, NamePath = "Салатовая 2", IdStreetParent = 7, StartX = -2.68, StartY = -1.58, EndX = -2.63, EndY = 2.68, IsBridge = false},
//            new StreetPaths {Renta = 25, NamePath = "Оранжевая", IdStreetParent = 10, StartX = -2.63, StartY = 2.68, EndX = 2.65, EndY = 2.68, IsBridge = false},
//            new StreetPaths {Renta = 25, NamePath = "Бордовая 1", IdStreetParent = 11, StartX = 2.65, StartY = 2.68, EndX = 2.65, EndY = 1.58, IsBridge = false},
//            new StreetPaths {Renta = 25, NamePath = "Бородовая 2", IdStreetParent = 11, StartX =  2.65, StartY = 1.58, EndX = 2.65, EndY = -2.74, IsBridge = false}
//        };
//
//        PathsForBuy[] pathsForBuys = new[]
//        {
//            new PathsForBuy {IdPathForBuy = 1, IdPlayer = 0, PriceStreetPath = 100},
//            new PathsForBuy {IdPathForBuy = 3, IdPlayer = 0, PriceStreetPath = 100},
//            new PathsForBuy {IdPathForBuy = 4, IdPlayer = 0, PriceStreetPath = 100},
//            new PathsForBuy {IdPathForBuy = 6, IdPlayer = 0, PriceStreetPath = 100},
//            new PathsForBuy {IdPathForBuy = 7, IdPlayer = 0, PriceStreetPath = 100},
//            new PathsForBuy {IdPathForBuy = 8, IdPlayer = 0, PriceStreetPath = 100},
//            new PathsForBuy {IdPathForBuy = 9, IdPlayer = 0, PriceStreetPath = 100},
//            new PathsForBuy {IdPathForBuy = 10, IdPlayer = 0, PriceStreetPath = 100},
//            new PathsForBuy {IdPathForBuy = 11, IdPlayer = 0, PriceStreetPath = 100},
//            new PathsForBuy {IdPathForBuy = 12, IdPlayer = 0, PriceStreetPath = 100},
//            new PathsForBuy {IdPathForBuy = 13, IdPlayer = 0, PriceStreetPath = 100},
//            new PathsForBuy {IdPathForBuy = 15, IdPlayer = 0, PriceStreetPath = 100},
//            new PathsForBuy {IdPathForBuy = 16, IdPlayer = 0, PriceStreetPath = 100},
//            new PathsForBuy {IdPathForBuy = 17, IdPlayer = 0, PriceStreetPath = 100}
//        };
//		
//        Builds[] buildses = new[]
//        {
//            new Builds {NameBuild = "Дом на Желтой 1", AboutBuild = "", Enabled = false, IdStreetPath = 1, PriceBuild = 100, posX = 2.25 , posY = -7},
//            new Builds {NameBuild = "Дом на Красной 1", AboutBuild = "", Enabled = false, IdStreetPath = 3, PriceBuild = 100, posX = -7, posY = -3.5},
//            new Builds {NameBuild = "Дом на Красной 2", AboutBuild = "", Enabled = false, IdStreetPath = 4, PriceBuild = 100, posX = -7, posY = 2},
//            new Builds {NameBuild = "Дом на Зеленой 2", AboutBuild = "", Enabled = false, IdStreetPath = 6, PriceBuild = 100, posX = 1.5, posY = 7},
//            new Builds {NameBuild = "Дом на Синей 1", AboutBuild = "", Enabled = false, IdStreetPath = 7, PriceBuild = 100, posX = 7, posY = 4},
//            new Builds {NameBuild = "Дом на Синей 2.1", AboutBuild = "", Enabled = false, IdStreetPath = 8, PriceBuild = 100, posX = 7, posY = -1},
//            new Builds {NameBuild = "Дом на Синей 2.2", AboutBuild = "", Enabled = false, IdStreetPath = 8, PriceBuild = 100, posX = 7, posY = -3.5},
//            new Builds {NameBuild = "Дом на Розовой", AboutBuild = "", Enabled = false, IdStreetPath = 9, PriceBuild = 100, posX = 0, posY = -4},
//            new Builds {NameBuild = "Дом на Фиолетовой", AboutBuild = "", Enabled = false, IdStreetPath = 10, PriceBuild = 100, posX = -4, posY = 0},
//            new Builds {NameBuild = "Дом на Салатовой 1", AboutBuild = "", Enabled = false, IdStreetPath = 11, PriceBuild = 100, posX = -4, posY = 4.5},
//            new Builds {NameBuild = "Дом на Коричневой", AboutBuild = "", Enabled = false, IdStreetPath = 12, PriceBuild = 100, posX = 4.2, posY = 0},
//            new Builds {NameBuild = "Дом на Голубой 1", AboutBuild = "", Enabled = false, IdStreetPath = 13, PriceBuild = 100, posX = 0, posY = -1},
//            new Builds {NameBuild = "Дои на Салатовая 2", AboutBuild = "", Enabled = false, IdStreetPath = 15, PriceBuild = 100, posX = -1.3, posY = 1},
//            new Builds {NameBuild = "Дом на Оранжевой", AboutBuild = "", Enabled = false, IdStreetPath = 16, PriceBuild = 100, posX = 0, posY = 4},
//            new Builds {NameBuild = "Дом на Бордовой 1", AboutBuild = "", Enabled = false, IdStreetPath = 17, PriceBuild = 100, posX = 4, posY = 3.35}
//			
//        };
//
//        Events[] events = new[]
//        {
//            new Events {IdGovermentPath = 2, Info = "", NameEvent = "Surprize :/", Price = 20},
//            new Events {IdGovermentPath = 5, Info = "", NameEvent = "Surprize :3", Price = 20},
//            new Events {IdGovermentPath = 14, Info = "", NameEvent = "You're catched bad gay", Price = -20},
//            new Events {IdGovermentPath = 14, Info = "", NameEvent = "Surprize :)", Price = 20},
//            new Events {IdGovermentPath = 18, Info = "", NameEvent = "Surprize :0", Price = 20}
//			
//        };
}