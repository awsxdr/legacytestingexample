namespace LegacyTestingExample
{
    using System;
    using System.Windows.Forms;

    public static class ExternalFlightProvider
    {
        public static Flight[] GetFlights(DateTime date)
        {
            MessageBox.Show(
                $"The method {nameof(ExternalFlightProvider)}.{nameof(GetFlights)} has been called. This shouldn't happen during testing.");

            return new Flight[0];
        }
    }
}