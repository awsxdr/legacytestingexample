namespace LegacyTestingExample
{
    using System;
    using System.Windows.Forms;

    public class FlightFinder
    {
        private readonly FlightTimeManager _flightTimeManager = new FlightTimeManager();

        public bool CheckAlternatives { get; set; }

        public bool FindFlights(Airport dep, Airport dst, DateTime outDt, DateTime backDt)
        {
            var rf = new ResultsForm();
            string sErr = null;

            // Set initial offsets
            int om = -1, bm = -1;

            bool gotIt = false;
            Flight of = null, bf = null;

            do
            {
                Flight[] ofs, bfs;

                // If we're checking alternatives then we need to adjust the dates by the offsets.
                // If we're not checking then we just ignore the offsets and carry on.
                if (CheckAlternatives)
                {
                    ofs = _flightTimeManager.GetFlightsForDate(outDt.AddDays(om));
                    bfs = _flightTimeManager.GetFlightsForDate(backDt.AddDays(bm));
                }
                else
                {
                    ofs = _flightTimeManager.GetFlightsForDate(outDt);
                    bfs = _flightTimeManager.GetFlightsForDate(backDt);
                }

                // Loop through flights
                for (int i = 0; i < ofs.Length; i++)
                {
                    for (int j = 0; j < bfs.Length; j++)
                    {
                        if (ofs[i].Airline == bfs[j].Airline)
                        {
                            of = ofs[i];
                            bf = bfs[i];
                            gotIt = true;
                            goto gotIt;
                        }
                    }
                }

                ++om;
                if (om > 1)
                {
                    om = -1;
                    ++bm;
                }

            } while (CheckAlternatives && bm <= 1);

            if (!gotIt)
            {
                sErr = "Could not find a flight";
                goto cleanUpAndExit;
            }

gotIt:
            try
            {
                of.ReserveFlight();
                bf.ReserveFlight();
            }
            catch
            {
                sErr = "An exception occurred whilst reserving flights.";
            }
            
            rf.Show();

cleanUpAndExit:

            if (sErr != null)
                MessageBox.Show("There was an error! This message shouldn't be displayed while testing");

            return true;
        }
    }
}
