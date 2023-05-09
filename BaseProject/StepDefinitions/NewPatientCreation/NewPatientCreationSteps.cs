using BaseProject.PageObjects.Common;
using NUnit.Framework;
using Selenium.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace BaseProject.StepDefinitions.CreatePatient
{
    [Binding]
    public class NewPatientCreationSteps
    {
        private readonly NewPatientCreationPage _newPatientCreation;
        public NewPatientCreationSteps ()
        {
            _newPatientCreation = new NewPatientCreationPage();
        }

        [When(@"I search patient with")]
        public void WhenISearchPatientWith (Table table)
        {
            Dictionary<string, string> tableData = table.ToDictionary();
            _newPatientCreation.DeleteDataFromDb(tableData["SurName"]);
            _newPatientCreation.EnterTermInSearchBox(tableData["SurName"]);
        }

        [When(@"I search for patient(.*) from the patient search results")]
        public void WhenISearchForPatientMarkMichelPFromThePatientSearchResults (string patientName)
        {
            _newPatientCreation.SelectPatientFromSearchResults(patientName);
        }

        [When(@"I click the Add Patient button")]
        public void WhenIClickTheAddPatientButton ()
        {
            _newPatientCreation.ClickF1AddPatientButton();
        }

        [Then(@"I should navigate to Add New Patient Screen")]
        public void ThenIShouldNavigateToAddNewPatientScreen ()
        {
            _newPatientCreation.AddNewPatientScreenValidation();
        }

        [Then(@"I enter the mandatory fileds")]
        public void ThenIEnterTheMandatoryFileds (Table table)
        {
            Dictionary<string, string> tableData = table.ToDictionary();
            _newPatientCreation.EnterMandatoryPatientInfo(tableData["First Name"], tableData["Sur Name"], tableData["MiddleName"], tableData["DOB"], tableData["Gender"],
                tableData["Street Name"], tableData["Post Code"], tableData["Exemption Type"]);            
        }

        [Then(@"I enter the details of the patient")]
        public void ThenIEnterTheDetailsOfThePatient (Table table)
        {
            Dictionary<string, string> tableData = table.ToDictionary();
            _newPatientCreation.EnterAllFields(tableData["Title"], tableData["First Name"], tableData["Sur Name"], tableData["Middle Name"], tableData["NHS No"],
                 tableData["Patient No"], tableData["Preffered Name"], tableData["DOB"], tableData["Gender"], tableData["House Number"], tableData["Street Name"],
                 tableData["City"], tableData["Town"], tableData["Post Code"], tableData["Home No"], tableData["Mobile No"],
                 tableData["Work No"], tableData["Fax No"], tableData["Email"], tableData["Exemption Type"], tableData["Ethinicity"], tableData["Prescribers"], tableData["Prescriber Name"],
                 tableData["Prescriber Org Name"], tableData["Note"],tableData["JoinNursingHome"], tableData["HomeName"]);           
        }

        [When(@"I create a new patient record using the below details")]
        public void WhenICreateANewPatientRecordUsingTheBelowDetails (Table table)
        {
            Dictionary<string, string> tableData = table.ToDictionary();
            _newPatientCreation.PatientCreationInDB(tableData["Gender"], tableData["Sur Name"], tableData["First Name"], tableData["DOB"],
                tableData["Street Name"], tableData["Post Code"], tableData["Exemption Type"]);           
        }

        [When(@"I click on Save Button")]
        public void WhenIClickOnSaveButton ()
        {
            _newPatientCreation.ClickF10SaveButton();
        }

        [Then(@"I verify created patient is listed in the database")]
        public void ThenIVerifyCreatedPatientIsListedInTheDatabase ()
        {
            Assert.True(_newPatientCreation.ValidatePatientInDb());
        }
    }
}
