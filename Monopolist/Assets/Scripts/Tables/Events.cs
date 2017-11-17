using SQLite4Unity3d;


    public class Events
    {
        [PrimaryKey, AutoIncrement]
        public int IdEvent { get; set; }
        public string NameEvent { get; set; }
        public string Info { get; set; }
        public int Price { get; set; }
        
        public int IdGovermentPath { get; set; }
    }
