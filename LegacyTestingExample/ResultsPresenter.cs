namespace LegacyTestingExample
{
    public class ResultsPresenter
    {
        private ResultsForm _resultsForm = new ResultsForm();

        public void ShowResults(Flight of, Flight bf)
        {
            _resultsForm.Show();
        }
    }
}