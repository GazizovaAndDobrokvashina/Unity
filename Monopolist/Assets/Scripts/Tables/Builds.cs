using JetBrains.Annotations;
using SQLite4Unity3d;

public class Builds
{
    [PrimaryKey, AutoIncrement]
    public int IdBuild { get; set; }
    public string NameBuild { get; set; }
    public string AboutBuild { get; set; }
    public int IdStreetPath { get; set; }
    public int PriceBuild { get; set; }
    public bool Enabled { get; set; }
    public double posX { get; set; }
    public double posY { get; set; }

    public Builds()
    {
    }

    public Build getBuild()
    {
        return new Build(IdBuild, NameBuild, AboutBuild, IdStreetPath, PriceBuild, Enabled, posX, posY);
    }

    public Builds(int idBuild, string nameBuild, string aboutBuild, int idStreetPath, int priceBuild, bool enabled, double posX, double posY)
    {
        IdBuild = idBuild;
        NameBuild = nameBuild;
        AboutBuild = aboutBuild;
        IdStreetPath = idStreetPath;
        PriceBuild = priceBuild;
        Enabled = enabled;
        this.posX = posX;
        this.posY = posY;
    }
}