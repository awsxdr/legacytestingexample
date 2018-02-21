namespace LegacyTestingExample
{
    using System;
    using System.Windows.Forms;

    public class FlightTimeManager : IFlightTimeManager
    {
        public Flight[] GetFlightsForDate(DateTime date)
        {
            MessageBox.Show(
                $"The method {nameof(FlightTimeManager)}.{nameof(GetFlightsForDate)} has been called. This shouldn't happen during testing.",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            return ExternalFlightProvider.GetFlights(date);
        }
    }
}