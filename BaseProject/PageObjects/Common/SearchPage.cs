using System;
using Selenium.BasePage;

namespace BaseProject.PageObjects.Common
{
	public class SearchPage
	{
		private readonly string _searchInput              = "//*[@AutomationId='patientSearchTextBox']";
		private readonly string _addPatientButton         = "[F1 - Add Patient]";
		private readonly string _patientSearchresultsText = "//*[@AutomationId='resultsListView']//*[@Name='{0}']";

		public void EnterTermInSearchBox (string searchTerm) => BasePage.SetText(_searchInput, searchTerm, ElementWaitState.PRESENT, 10);

		public void ClickAddPatientButton () => BasePage.Click(_addPatientButton);

		public void SelectPatientFromSearchResults (string patientName)
		{
			if (BasePage.IsElementPresent(String.Format(_patientSearchresultsText, patientName), 15))
			{
				BasePage.DoubleClick(String.Format(_patientSearchresultsText, patientName));
			}
			else
			{
				throw new Exception("Unable to find the patient. Check whether the patient name is spelled right");
			}
		}
	}
}