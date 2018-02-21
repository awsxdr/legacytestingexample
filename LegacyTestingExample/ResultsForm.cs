using System.Windows.Forms;

namespace LegacyTestingExample
{
    public partial class ResultsForm : Form
    {
        public ResultsForm()
        {
            InitializeComponent();
        }

        private void ResultsForm_Load(object sender, System.EventArgs e)
        {
            MessageBox.Show(
                $"The ResultsForm window has been displayed. This shouldn't happen during testing.",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        public void ShowResults(Flight of, Flight bf)
        {
            this.Show();
        }
    }
}
