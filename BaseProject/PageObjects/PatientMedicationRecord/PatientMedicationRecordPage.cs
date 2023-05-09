using System;
using Selenium.BasePage;

namespace BaseProject.PageObjects.PatientMedicationRecord
{
	public class PatientMedicationRecordPage
	{
		private readonly string _patientWarningsWindowText        = "[Patient Warnings]";
		private readonly string _confirmButton                    = "[F10 - Confirm]";
		private readonly string _patientInformationLeafletsButton = "//*[@AutomationId='PatientInformationLeafletsButton']";
		private readonly string _elementsInPMR                    = "[{0}]";
		private readonly string _searchTextBox                    = "//*[@AutomationId='searchTextBox']";

		public void HandleAllWarnings ()
		{
			bool isWarningPresent = true;
			while (isWarningPresent)
			{
				if (BasePage.IsElementPresent(_patientWarningsWindowText, 20))
					BasePage.Click(_confirmButton);
				else
					isWarningPresent = false;
			}
		}

		public void ClickPatientInformationLeafletsButton () => BasePage.Click(_patientInformationLeafletsButton);

		public void ClickButtonInPMR (string buttonName) => BasePage.Click(String.Format(_elementsInPMR, buttonName));

		public void DoubleClickButtonInPMR (string buttonName) => BasePage.DoubleClick(String.Format(_elementsInPMR, buttonName));

		public void EnterAndSelectDrugFromDrugSearchResult (string drugName)
		{
			BasePage.SetText(_searchTextBox, drugName);
			if (BasePage.IsElementPresent(String.Format(_elementsInPMR, drugName)))
				BasePage.Click(String.Format(_elementsInPMR, drugName));
			else
				throw new Exception(String.Format("The Drug {0} is not found the search results", drugName));
		}
	}
}