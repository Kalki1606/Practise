using System;
using Selenium.BasePage;

namespace BaseProject.PageObjects.PatientInformationLeaflets
{
	public class PatientInformationLeafletsPage
	{

		private string SearchInput        = "//*[@data-testid='search-box-input']";
		private string NoResultsFoundText = "//div[contains(text(),'No results were found')]";

		public string GetPatientInformationLeafletsTitle ()
		{
			BasePage.IsElementPresent(SearchInput, 10);
			return BasePage.GetTitle();
		}

		public void EnterSearchText (string searchText) => BasePage.SetText(SearchInput, searchText);

		public Boolean VerifySearchResults ()
		{
			if (BasePage.IsElementPresent(NoResultsFoundText, 5))
			{
				if (BasePage.GetCountOfElements(NoResultsFoundText) == 2)
					return false;
			}
			return true;
		}

		public string GetSearchTerm () => BasePage.GetText(SearchInput);
	}
}