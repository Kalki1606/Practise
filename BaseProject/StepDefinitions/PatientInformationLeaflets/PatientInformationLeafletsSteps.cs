using BaseProject.PageObjects.PatientInformationLeaflets;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace BaseProject.StepDefinitions.PatientInformationLeaflets
{
	[Binding]
	public class PatientInformationLeafletsSteps
	{
		private readonly PatientInformationLeafletsPage _patientInformationLeafletsPage;
		public PatientInformationLeafletsSteps ()
		{
			_patientInformationLeafletsPage = new PatientInformationLeafletsPage();
		}

		[Then(@"I verify Information Leaflets is opened")]
		public void ThenIVerifyInformationLeafletsIsOpened ()
		{
			Assert.AreEqual("Information Leaflets", _patientInformationLeafletsPage.GetPatientInformationLeafletsTitle());
		}

		[When(@"I enter (.*) in search bar")]
		public void WhenIEnterAsthmaInSearchBar (string searchTerm)
		{
			_patientInformationLeafletsPage.EnterSearchText(searchTerm);
		}

		[Then(@"I verify the search results is displayed")]
		public void ThenIVerifyTheSearchResultsIsDisplayed ()
		{
			Assert.True(_patientInformationLeafletsPage.VerifySearchResults());
		}
	}
}