using SQLite4Unity3d;

public class Builds
{
    [PrimaryKey, AutoIncrement]
    public int IdBuild { get; set; }

    public int IdStreetPath { get; set; }
    public int PriceBuild { get; set; }
    public bool Enabled { get; set; }

    public Build getBuild()
    {
        return new Build(IdBuild, IdStreetPath, PriceBuild, Enabled);
    }
}