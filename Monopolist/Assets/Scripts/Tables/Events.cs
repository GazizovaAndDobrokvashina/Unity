using SQLite4Unity3d;


public class Events
{
    [PrimaryKey, AutoIncrement]
    public int IdEvent { get; set; }

    public string NameEvent { get; set; }
    public string Info { get; set; }
    public int Price { get; set; }
    public int IdGovermentPath { get; set; }

    public Events()
    {
    }

    public Event GetEvent()
    {
        return new Event(IdEvent, Info, NameEvent, Price, IdGovermentPath);
    }

    public Events(int idEvent, string nameEvent, string info, int price, int idGovermentPath)
    {
        IdEvent = idEvent;
        NameEvent = nameEvent;
        Info = info;
        Price = price;
        IdGovermentPath = idGovermentPath;
    }
}