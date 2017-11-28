using SQLite4Unity3d;


public class Streets
{
    [PrimaryKey, AutoIncrement]
    public int IdStreet { get; set; }

    public string NameStreet { get; set; }
    public string AboutStreet { get; set; }

    public Street GetStreet(int[] paths)
    {
        return new Street(IdStreet, NameStreet, AboutStreet, paths);
    }
}