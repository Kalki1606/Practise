using BaseProject.PageObjects.PatientMedicationRecord;
using TechTalk.SpecFlow;

namespace BaseProject.StepDefinitions.PatientMedicationRecord
{
	[Binding]
	public class PatientMedicationrecordSteps
	{
		private readonly PatientMedicationRecordPage _patientMedicationRecordPage;
		public PatientMedicationrecordSteps ()
		{
			_patientMedicationRecordPage = new PatientMedicationRecordPage();
		}

		[When(@"I handle all warnings in PMR page")]
		public void WhenIHandleAllWarningsInPMRPage ()
		{
			_patientMedicationRecordPage.HandleAllWarnings();
		}

		[When(@"I click Patient Information Leaflets button")]
		public void WhenIClickPatientInformationLeafletsButton ()
		{
			_patientMedicationRecordPage.ClickPatientInformationLeafletsButton();
		}

		[When(@"I click (.*) button in PMR")]
		public void WhenIClickButtonInPMR (string buttonName)
		{
			_patientMedicationRecordPage.DoubleClickButtonInPMR(buttonName);
		}

		[When(@"I enter and select (.*) from drug search result")]
		public void WhenIEnterAndSelectAspirinMgSupposFromDrugSearchResult (string drugName)
		{
			_patientMedicationRecordPage.EnterAndSelectDrugFromDrugSearchResult(drugName);
		}
	}
}