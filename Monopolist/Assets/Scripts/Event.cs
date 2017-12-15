public class Event
{
    private int id;
    private string info;
    private string name;
    private int price;
    private int idGovermentPath;

    public Event(int id, string info, string name, int price, int idGovermentPath)
    {
        this.id = id;
        this.info = info;
        this.name = name;
        this.price = price;
        this.idGovermentPath = idGovermentPath;
    }

    public int Id
    {
        get { return id; }
    }

    public string Info
    {
        get { return info; }
    }

    public string Name
    {
        get { return name; }
    }

    public int Price
    {
        get { return price; }
    }

    public int IdGovermentPath
    {
        get { return idGovermentPath; }
    }

    public Events getEntity()
    {
        return new Events(id, name, info, price, idGovermentPath);
    }
}