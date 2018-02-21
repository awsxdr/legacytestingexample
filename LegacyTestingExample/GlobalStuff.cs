namespace LegacyTestingExample
{
    public static class GlobalStuff
    {
        public static IDatabaseConnection Conn { get; } = new DatabaseConnection();
    }
}