using JetBrains.Annotations;
using SQLite4Unity3d;

public class GameData
{
    [PrimaryKey, AutoIncrement]
    public int IdGame { get; set; }
    public string NameGame { get; set; }
    public string NameTown { get; set; }
    public int Steps { get; set; }
    public bool isOnline { get; set; }
    public int CurrentPlayerId { get; set; }

    public GameData()
    {
    }

    public GameData(string nameGame, string nameTown, int steps, bool isOnline, int currentPlayerId)
    {
        NameGame = nameGame;
        NameTown = nameTown;
        Steps = steps;
        this.isOnline = isOnline;
        CurrentPlayerId = currentPlayerId;
    }

    public GameData(int idGame, string nameGame, string nameTown, int steps, bool isOnline, int currentPlayerId)
    {
        IdGame = idGame;
        NameGame = nameGame;
        NameTown = nameTown;
        Steps = steps;
        this.isOnline = isOnline;
        CurrentPlayerId = currentPlayerId;
    }
}