namespace LegacyTestingExample
{
    using System;

    public class FlightTimeManager
    {
        public Flight[] GetFlightsForDate(DateTime date)
        {
            return ExternalFlightProvider.GetFlights(date);
        }
    }
}