using BaseProject.PageObjects.Common;
using TechTalk.SpecFlow;

namespace BaseProject.StepDefinitions.Common
{
	[Binding]
	public class SearchSteps
	{
		private readonly SearchPage _searchPage;
		public SearchSteps ()
		{
			_searchPage = new SearchPage();
		}

		[When(@"I search for the patient (.*)")]
		public void WhenISearchForThePatient (string patientName)
		{
			_searchPage.EnterTermInSearchBox(patientName);
		}

		[When(@"I click the create patient button")]
		public void WhenISearchForThePatient ()
		{
			_searchPage.ClickAddPatientButton();
		}

		[When(@"I select the patient (.*) from the patient search results")]
		public void WhenISelectThePatientLuffyFromThePatientSearchResults (string patientName)
		{
			_searchPage.SelectPatientFromSearchResults(patientName);
		}
	}
}