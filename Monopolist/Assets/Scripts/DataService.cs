using SQLite4Unity3d;
using UnityEngine;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System.Collections.Generic;
using System.Linq;


public class DataService  {
	
    public SQLiteConnection _connection;
 
    public DataService(string DatabaseName){
#if UNITY_EDITOR
        var dbPath = string.Format(@"Assets\SavedGames\{0}", DatabaseName);
#else
// check if file exists in Application.persistentDataPath
        	var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);
 
        	if (!File.Exists(filepath))
		{
			Debug.Log("Database not in Persistent path");
			// if it doesn't ->
			// open StreamingAssets directory and load the db ->
#if UNITY_ANDROID 
			var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
			while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
			// then save to Application.persistentDataPath
			File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
			var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
			// then save to Application.persistentDataPath
			File.Copy(loadDb, filepath);
#elif UNITY_WP8
			var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
			// then save to Application.persistentDataPath
			File.Copy(loadDb, filepath);
#elif UNITY_WINRT
			var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
			// then save to Application.persistentDataPath
			File.Copy(loadDb, filepath);
#endif
			Debug.Log("Database written");
		}
		var dbPath = filepath;
#endif
        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        Debug.Log("Final PATH: " + dbPath);     
    }

    public void CreateDB()
    {
        CreateTables();

        FullTables();
    }

    public bool IsExist()
    {
        _connection.CreateTable<Builds>();
        return _connection.Table<Builds>().Any();
    }

    private void CreateTables()
    {
        _connection.CreateTable<Builds>();
        _connection.CreateTable<Events>();
        _connection.CreateTable<PathsForBuy>();
        _connection.CreateTable<Players>();
        _connection.CreateTable<Streets>();
        _connection.CreateTable<StreetPaths>();
    }
	

    private void FullTables()
    {
		
        Streets[] streets = new[]
        {
            new Streets {NameStreet = "Street1", AboutStreet = "Желтая улица, короткая часть - парковка"},
            new Streets {NameStreet = "Street2", AboutStreet = "Красная улица"},
            new Streets {NameStreet = "Street3", AboutStreet = "Зеленая улица, короткая часть - парк"},
            new Streets {NameStreet = "Street4", AboutStreet = "Синяя улица"},
            new Streets {NameStreet = "Street5", AboutStreet = "Розовая улица"},
            new Streets {NameStreet = "Street6", AboutStreet = "Фиолетовая улица"},
            new Streets {NameStreet = "Street7", AboutStreet = "Салатовая улица"},
            new Streets {NameStreet = "Street8", AboutStreet = "Коричневая улица"},
            new Streets {NameStreet = "Street9", AboutStreet = "Голубая улица, короткая часть - суд"},
            new Streets {NameStreet = "Street10", AboutStreet = "Оранжевая улица"},
            new Streets {NameStreet = "Street11", AboutStreet = "Бордовая улица, длинная часть = парк"},
        };

        StreetPaths[] pathses = new[]
        {
            new StreetPaths {Renta = 25, NamePath = "Желтая 1", IdStreetParent = 1, StartX = 5.63, StartY = -5.64, EndX = -1.57, EndY = -5.64, IsBridge = false},
            new StreetPaths {Renta = 20, NamePath = "Желтая 2", IdStreetParent = 1, StartX = -1.57, StartY = -5.64, EndX = -5.68, EndY = -5.64, IsBridge = false},
            new StreetPaths {Renta = 20, NamePath = "Красная 1", IdStreetParent = 2, StartX = -5.68, StartY = -5.64, EndX = -5.68, EndY = -1.58, IsBridge = false},
            new StreetPaths {Renta = 25, NamePath = "Красная 2", IdStreetParent = 2, StartX = -5.68, StartY = -1.58, EndX = -5.68, EndY = 5.62, IsBridge = false},
            new StreetPaths {Renta = 20, NamePath = "Зеленая 1", IdStreetParent = 3, StartX = -5.68, StartY = 5.62, EndX = -2.63, EndY = 5.62, IsBridge = false},
            new StreetPaths {Renta = 25, NamePath = "Зеленая 2", IdStreetParent = 3, StartX = -2.63, StartY = 5.62, EndX = 5.63, EndY = 5.62, IsBridge = false},
            new StreetPaths {Renta = 20, NamePath = "Синяя 1", IdStreetParent = 4, StartX = 5.63, StartY = 5.62, EndX = 5.63, EndY = 1.58, IsBridge = false},
            new StreetPaths {Renta = 25, NamePath = "Синяя 2", IdStreetParent = 4, StartX = 5.63, StartY = 1.58, EndX = 5.63, EndY = -5.64, IsBridge = false},
            new StreetPaths {Renta = 15, NamePath = "Розовая", IdStreetParent = 5, StartX = -1.57, StartY = -5.64, EndX = -1.57, EndY = -2.74, IsBridge = true},
            new StreetPaths {Renta = 15, NamePath = "Фиолетовая", IdStreetParent = 6, StartX = -5.68, StartY = -1.58, EndX = -2.68, EndY = -1.58, IsBridge = true},
            new StreetPaths {Renta = 15, NamePath = "Салатовая 1", IdStreetParent = 7, StartX = -2.63, StartY = 5.62, EndX = -2.63, EndY = 2.68, IsBridge = true},
            new StreetPaths {Renta = 15, NamePath = "Коричневая", IdStreetParent = 8, StartX = 5.63, StartY = 1.58, EndX = 2.65, EndY = 1.58, IsBridge = true},
            new StreetPaths {Renta = 25, NamePath = "Голубая 1", IdStreetParent = 9, StartX = 2.65, StartY = -2.74, EndX = -1.57, EndY = -2.74, IsBridge = false},
            new StreetPaths {Renta = 15, NamePath = "Голубая 2", IdStreetParent = 9, StartX = -1.57, StartY = -2.74, EndX = -2.68, EndY = -1.58, IsBridge = false},
            new StreetPaths {Renta = 15, NamePath = "Салатовая 2", IdStreetParent = 7, StartX = -2.68, StartY = -1.58, EndX = -2.63, EndY = 2.68, IsBridge = false},
            new StreetPaths {Renta = 25, NamePath = "Оранжевая", IdStreetParent = 10, StartX = -2.63, StartY = 2.68, EndX = 2.65, EndY = 2.68, IsBridge = false},
            new StreetPaths {Renta = 25, NamePath = "Бордовая 1", IdStreetParent = 11, StartX = 2.65, StartY = 2.68, EndX = 2.65, EndY = 1.58, IsBridge = false},
            new StreetPaths {Renta = 25, NamePath = "Бородовая 2", IdStreetParent = 11, StartX =  2.65, StartY = 1.58, EndX = 2.65, EndY = -2.74, IsBridge = false}
        };

        PathsForBuy[] pathsForBuys = new[]
        {
            new PathsForBuy {IdPathForBuy = 1, IdPlayer = 0, PriceStreetPath = 100},
            new PathsForBuy {IdPathForBuy = 3, IdPlayer = 0, PriceStreetPath = 100},
            new PathsForBuy {IdPathForBuy = 4, IdPlayer = 0, PriceStreetPath = 100},
            new PathsForBuy {IdPathForBuy = 6, IdPlayer = 0, PriceStreetPath = 100},
            new PathsForBuy {IdPathForBuy = 7, IdPlayer = 0, PriceStreetPath = 100},
            new PathsForBuy {IdPathForBuy = 8, IdPlayer = 0, PriceStreetPath = 100},
            new PathsForBuy {IdPathForBuy = 9, IdPlayer = 0, PriceStreetPath = 100},
            new PathsForBuy {IdPathForBuy = 10, IdPlayer = 0, PriceStreetPath = 100},
            new PathsForBuy {IdPathForBuy = 11, IdPlayer = 0, PriceStreetPath = 100},
            new PathsForBuy {IdPathForBuy = 12, IdPlayer = 0, PriceStreetPath = 100},
            new PathsForBuy {IdPathForBuy = 13, IdPlayer = 0, PriceStreetPath = 100},
            new PathsForBuy {IdPathForBuy = 15, IdPlayer = 0, PriceStreetPath = 100},
            new PathsForBuy {IdPathForBuy = 16, IdPlayer = 0, PriceStreetPath = 100},
            new PathsForBuy {IdPathForBuy = 17, IdPlayer = 0, PriceStreetPath = 100}
        };
		
        Builds[] buildses = new[]
        {
            new Builds {NameBuild = "Дом на Желтой 1", AboutBuild = "", Enabled = false, IdStreetPath = 1, PriceBuild = 100, posX = 2.25 , posY = -7},
            new Builds {NameBuild = "Дом на Красной 1", AboutBuild = "", Enabled = false, IdStreetPath = 3, PriceBuild = 100, posX = -7, posY = -3.5},
            new Builds {NameBuild = "Дом на Красной 2", AboutBuild = "", Enabled = false, IdStreetPath = 4, PriceBuild = 100, posX = -7, posY = 2},
            new Builds {NameBuild = "Дом на Зеленой 2", AboutBuild = "", Enabled = false, IdStreetPath = 6, PriceBuild = 100, posX = 1.5, posY = 7},
            new Builds {NameBuild = "Дом на Синей 1", AboutBuild = "", Enabled = false, IdStreetPath = 7, PriceBuild = 100, posX = 7, posY = 4},
            new Builds {NameBuild = "Дом на Синей 2.1", AboutBuild = "", Enabled = false, IdStreetPath = 8, PriceBuild = 100, posX = 7, posY = -1},
            new Builds {NameBuild = "Дом на Синей 2.2", AboutBuild = "", Enabled = false, IdStreetPath = 8, PriceBuild = 100, posX = 7, posY = -3.5},
            new Builds {NameBuild = "Дом на Розовой", AboutBuild = "", Enabled = false, IdStreetPath = 9, PriceBuild = 100, posX = 0, posY = -4},
            new Builds {NameBuild = "Дом на Фиолетовой", AboutBuild = "", Enabled = false, IdStreetPath = 10, PriceBuild = 100, posX = -4, posY = 0},
            new Builds {NameBuild = "Дом на Салатовой 1", AboutBuild = "", Enabled = false, IdStreetPath = 11, PriceBuild = 100, posX = -4, posY = 4.5},
            new Builds {NameBuild = "Дом на Коричневой", AboutBuild = "", Enabled = false, IdStreetPath = 12, PriceBuild = 100, posX = 4.2, posY = 0},
            new Builds {NameBuild = "Дом на Голубой 1", AboutBuild = "", Enabled = false, IdStreetPath = 13, PriceBuild = 100, posX = 0, posY = -1},
            new Builds {NameBuild = "Дои на Салатовая 2", AboutBuild = "", Enabled = false, IdStreetPath = 15, PriceBuild = 100, posX = -1.3, posY = 1},
            new Builds {NameBuild = "Дом на Оранжевой", AboutBuild = "", Enabled = false, IdStreetPath = 16, PriceBuild = 100, posX = 0, posY = 4},
            new Builds {NameBuild = "Дом на Бордовой 1", AboutBuild = "", Enabled = false, IdStreetPath = 17, PriceBuild = 100, posX = 4, posY = 3.35}
			
        };

        Events[] events = new[]
        {
            new Events {IdGovermentPath = 2, Info = "", NameEvent = "Surprize bitch1", Price = 20},
            new Events {IdGovermentPath = 5, Info = "", NameEvent = "Surprize bitch2", Price = 20},
            new Events {IdGovermentPath = 14, Info = "", NameEvent = "Surprize bitch3", Price = 20},
            new Events {IdGovermentPath = 18, Info = "", NameEvent = "Surprize bitch4", Price = 20}
			
        };

        _connection.InsertAll(streets);
        _connection.InsertAll(pathses);
        _connection.InsertAll(pathsForBuys);
        _connection.InsertAll(buildses);
        _connection.InsertAll(events);

    }

    public void AddPlayer(Player player)
    {
        _connection.Insert(player.GetPlayers());
    }

    public List<Builds> getBuilds()
    {
        List<Builds> buildses = new List<Builds>();
        foreach (Builds buildse in _connection.Table<Builds>())
        {
            buildses.Add(buildse);
        }

        return buildses;
    }
	
    public List<Events> getEvents()
    {
        List<Events> eventes = new List<Events>();
        foreach (Events events in _connection.Table<Events>())
        {
            eventes.Add(events);
        }

        return eventes;
    }
	
    public List<PathsForBuy> getPathsForBuy()
    {
        List<PathsForBuy> pathsForBuys = new List<PathsForBuy>();
        foreach (PathsForBuy path in _connection.Table<PathsForBuy>())
        {
            pathsForBuys.Add(path);
        }

        return pathsForBuys;
    }
	
    public List<Players> getPlayers()
    {
        List<Players> playerses = new List<Players>();
        foreach (Players players in _connection.Table<Players>())
        {
            playerses.Add(players);
        }

        return playerses;
    }
	
    public List<StreetPaths> getStreetPaths()
    {
        List<StreetPaths> paths = new List<StreetPaths>();
        foreach (StreetPaths path in _connection.Table<StreetPaths>())
        {
            paths.Add(path);
        }

        return paths;
    }
	
    public List<Streets> getStreets()
    {
        List<Streets> streets = new List<Streets>();
        foreach (Streets street in _connection.Table<Streets>())
        {
            streets.Add(street);
        }

        return streets;
    }

    public Builds getBuildById(int id)
    {
        return _connection.Table<Builds>().FirstOrDefault(x => x.IdBuild == id);
    }
	
    public Events getEventById(int id)
    {
        return _connection.Table<Events>().FirstOrDefault(x => x.IdEvent == id);
    }
	
    public PathsForBuy getPathForBuyById(int id)
    {
        return _connection.Table<PathsForBuy>().FirstOrDefault(x => x.IdPathForBuy == id);
    }
	
    public Players getPlayerById(int id)
    {
        return _connection.Table<Players>().FirstOrDefault(x => x.IdPlayer == id);
    }
	
    public StreetPaths getStreeetPathById(int id)
    {
        return _connection.Table<StreetPaths>().FirstOrDefault(x => x.IdStreetPath == id);
    }
	
    public Streets getStreetById(int id)
    {
        return _connection.Table<Streets>().FirstOrDefault(x => x.IdStreet == id);
    }

    public List<Builds> getBuildsOnTheStreet(int StreetId)
    {
        List<Builds> buildses = new List<Builds>();

        foreach (Builds buildse in _connection.Table<Builds>().Where(x => x.IdStreetPath == StreetId))
        {
            buildses.Add(buildse);
        }

        return buildses;
    }
	
    public List<Events> getEventsOnTheStreet(int GovId)
    {
        List<Events> events = new List<Events>();

        foreach (Events evente in _connection.Table<Events>().Where(x => x.IdGovermentPath == GovId))
        {
            events.Add(evente);
        }

        return events;
    }
	
    public List<PathsForBuy> getAllPathsOfPlayer(int PlayerId)
    {
        List<PathsForBuy> paths = new List<PathsForBuy>();

        foreach (PathsForBuy path in _connection.Table<PathsForBuy>().Where(x => x.IdPlayer == PlayerId))
        {
            paths.Add(path);
        }

        return paths;
    }
	
    public List<StreetPaths> getAllPathsOfStreet(int StreetId)
    {
        List<StreetPaths> paths = new List<StreetPaths>();

        foreach (StreetPaths path in _connection.Table<StreetPaths>().Where(x => x.IdStreetParent == StreetId))
        {
            paths.Add(path);
        }

        return paths;
    }

    public bool UpdateObject(Builds build)
    {
        return  _connection.Update(build)==1;
    }
     	
    public bool UpdateObject(Events events)
    {
        return  _connection.Update( events)==1;
    }
     	
    public bool UpdateObject(PathsForBuy path)
    {
        return  _connection.Update(path)==1;
    }
     	
    public bool UpdateObject(Player player)
    {
        return  _connection.Update(player)==1;
    }
     	
    public bool UpdateObject(StreetPaths path)
    {
        return  _connection.Update(path)==1;
    }
     	
    public bool UpdateObject(Streets street)
    {
        return  _connection.Update(street)==1;
    }
}