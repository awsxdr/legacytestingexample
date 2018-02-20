namespace LegacyTestingExample
{
    using System;
    using System.Windows.Forms;

    public class FlightFinder
    {
        private readonly FlightTimeManager _flightTimeManager = new FlightTimeManager();

        public bool FindFlights(Airport dep, Airport dst, DateTime outDt, DateTime backDt)
        {
            var rf = new ResultsForm();
            string sErr = null;

            do
            {
                goto cleanUpAndExit;
            } while (true);

            rf.Show();

cleanUpAndExit:

            if (sErr != null)
                MessageBox.Show("There was an error! This message shouldn't be displayed while testing");

            return true;
        }
    }
}
