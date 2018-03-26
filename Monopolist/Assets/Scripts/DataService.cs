using SQLite4Unity3d;
using UnityEngine;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System.Collections.Generic;
using System.Linq;


public class DataService
{
    public SQLiteConnection _connection;

    public DataService(string CityName, string DataPath)
    {
        var dbPath = string.Format(DataPath + CityName);
        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
    }

    public DataService(string DatabaseName)
    {
#if UNITY_EDITOR
        var dbPath = string.Format(@"Assets\SavedGames\{0}", DatabaseName);
        //var dbPath = string.Format(Application.persistentDataPath+@"\SavedGames\{0}", DatabaseName);
#else
// check if file exists in Application.persistentDataPath
        	var filepath = string.Format("{0}/SavedGames/{1}", Application.persistentDataPath, DatabaseName);
 
        	if (!File.Exists(filepath))
		{
			Debug.Log("Database not in Persistent path");
			// if it doesn't ->
			// open StreamingAssets directory and load the db ->
#if UNITY_ANDROID 
			var loadDb =
 new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
			while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
			// then save to Application.persistentDataPath
			File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
			var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
			// then save to Application.persistentDataPath
			File.Copy(loadDb, filepath);
#elif UNITY_WP8
			var loadDb =
 Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
			// then save to Application.persistentDataPath
			File.Copy(loadDb, filepath);
#elif UNITY_WINRT
			var loadDb =
 Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
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
        Streets[] streets = Cities.GetStreets(City.Tron);

        StreetPaths[] pathses = Cities.GetStreetPaths(City.Tron);

        PathsForBuy[] pathsForBuys = Cities.GetPathForBuys(City.Tron);

        Builds[] buildses = Cities.GetBuildses(City.Tron);

        Events[] events = Cities.GetEvents(City.Tron);

        _connection.InsertAll(streets);
        _connection.InsertAll(pathses);
        _connection.InsertAll(pathsForBuys);
        _connection.InsertAll(buildses);
        _connection.InsertAll(events);
    }

    public void AddPlayer(Player player)
    {
        _connection.Insert(player.getEntity());
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
        return _connection.Update(build) == 1;
    }

    public bool UpdateObject(Events events)
    {
        return _connection.Update(events) == 1;
    }

    public bool UpdateObject(PathsForBuy path)
    {
        return _connection.Update(path) == 1;
    }

    public bool UpdateObject(Players player)
    {
        return _connection.Update(player) == 1;
    }

    public bool UpdateObject(StreetPaths path)
    {
        return _connection.Update(path) == 1;
    }

    public bool UpdateObject(Streets street)
    {
        return _connection.Update(street) == 1;
    }
}