public class Event
{
    //идентификатор события
    private int id;

    //информация о событии
    private string info;

    //название события
    private string nameE;

    //стоимость события
    private int price;

    //идентификатор улицы, на которой это событие может произойти
    private int idGovermentPath;

    //конструктор класса
    public Event(int id, string info, string nameE, int price, int idGovermentPath)
    {
        this.id = id;
        this.info = info;
        this.nameE = nameE;
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
        get { return nameE; }
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
        return new Events(id, nameE, info, price, idGovermentPath);
    }
}