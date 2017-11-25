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
}