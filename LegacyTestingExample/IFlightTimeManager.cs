using System;

namespace LegacyTestingExample
{
    public interface IFlightTimeManager
    {
        Flight[] GetFlightsForDate(DateTime date);
    }
}