namespace SQLite4Unity3d.Tables
{
    public class Cities
    {
        [PrimaryKey, AutoIncrement]
        public int IdCity { get; set; }
        public string NameCity { get; set; }
        public string AboutSity { get; set; }

    }
}