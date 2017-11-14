namespace SQLite4Unity3d.Tables
{
    public class Streets
    {
        [PrimaryKey, AutoIncrement]
        public int IdStreet { get; set; }
        public string NameStreet { get; set; }
        public string AboutStreet { get; set; }
        public int[] Paths { get; set; }
    }
}