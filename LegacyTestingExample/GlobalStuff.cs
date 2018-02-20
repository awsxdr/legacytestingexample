namespace LegacyTestingExample
{
    public static class GlobalStuff
    {
        static GlobalStuff()
        {
            _conn = new DatabaseConnection();
        }

        private static readonly DatabaseConnection _conn;
        public static DatabaseConnection Conn => _conn;
    }
}