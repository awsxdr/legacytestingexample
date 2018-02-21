namespace LegacyTestingExample
{
    public class Flight
    {
        public Airport Start { get; set; }
        public Airport End { get; set; }
        public string Airline { get; set; }
        public int Seats { get; set; }

        public void ReserveFlight()
        {
            GlobalStuff.Conn.ReserveFlight(this);
        }
    }
}