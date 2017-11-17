using SQLite4Unity3d;

    public class Players
    {
        [PrimaryKey, AutoIncrement]
        public int IdPlayer { get; set; }
        public string NickName { get; set; }
        public int Money { get; set; }
        public  double CoordinateX { get; set; }
        public double CoordinateY { get; set; }
    }
