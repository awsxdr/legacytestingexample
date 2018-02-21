namespace LegacyTestingExample
{
    using System.Windows.Forms;

    public class DatabaseConnection
    {
        public void ReserveFlight(Flight flight)
        {
            MessageBox.Show(
                $"The method {nameof(DatabaseConnection)}.{nameof(ReserveFlight)} has been called. This shouldn't happen during testing.",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}