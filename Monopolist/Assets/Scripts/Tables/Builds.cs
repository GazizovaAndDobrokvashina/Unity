namespace SQLite4Unity3d.Tables
{
    public class Builds
    {
        [PrimaryKey, AutoIncrement]
        public int IdBuild { get; set; }
        public int IdStreetPath{ get; set; }
        public int PriceBuild{ get; set; }
        public bool Enable { get; set; }
    }
}