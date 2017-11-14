namespace SQLite4Unity3d.Tables
{
    public class PathsForBuy
    {
        [PrimaryKey, AutoIncrement]
        public int IdPathForBuy { get; set; }
        public int IdPlayer { get; set; }
        public int PriceStreetPath { get; set; }
    }
}