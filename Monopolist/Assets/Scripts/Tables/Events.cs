namespace SQLite4Unity3d.Tables
{
    public class Events
    {
        [PrimaryKey, AutoIncrement]
        public int IdEvent { get; set; }
        public string NameEvent { get; set; }
        public string Info { get; set; }
        public int Price { get; set; }
    }
}