using System;
using System.Data.SqlClient;
using System.Threading;
using FrameworkCapabilities.Utilities.DatabaseOperations;
using Selenium.BasePage;

namespace BaseProject.PageObjects.Common
{
    public class NewPatientCreationPage
    {
        SqlHelpers helper = new SqlHelpers();

        private readonly string _searchInput                      = "//*[@AutomationId='patientSearchTextBox']";
        private readonly string _patientSearchresultsText         = "//*[@AutomationId='resultsListView']//*[@Name='{0}']";
        private readonly string _f1AddPatient                     ="[F1 - Add Patient]";
        private readonly string _addNewPatientScreen              ="[Add New Patient]";
        private readonly string _title                            ="//*[@AutomationId='titleComboBox']";
        private readonly string _surnameTextBox                   = "//*[@AutomationId='surnameTextBox']";
        private readonly string _firstnameTextBox                 ="//*[@AutomationId='givenNameTextBox']";
        private readonly string _middlenameTextBox                ="//*[@AutomationId='middleNameTextBox']";
        private readonly string _dob                              ="//*[@AutomationId='PART_TextBox']";
        public readonly string _gender                            ="//*[@AutomationId='genderComboBox']";
        private readonly string _nhsnumber                        ="//*[@AutomationId='nhsNoTextBox']";
        private readonly string _patientNoTextBox                 ="//*[@AutomationId='patientNumberTextBox']";
        // private readonly string _prefferedName                 = "//*[@Name='Preferred Name']//following-sibiling::*[@ClassName='TextBox']/";
        //*[@Name                                                 ='Preferred Name']//*[@ClassName='TextBox']"; PART_ContentHost
        private readonly string _houseNoTextBox                   ="//*[@AutomationId='houseNoTextBox']";
        private readonly string _streetTextBox                    ="//*[@AutomationId='streetTextBox']";
        private readonly string _cityTextBox                      ="//*[@AutomationId='cityTextBox']";
        private readonly string _countryTextBox                   ="//*[@AutomationId='countyTextBox']";
        private readonly string _postCodeTextBox                  ="//*[@AutomationId='postCodeTextBox']";
        private readonly string _homeNoTextBox                    ="//*[@AutomationId='homeNoTextBox']";
        private readonly string _workNoTextBox                    ="//*[@AutomationId='workNoTextBox']";
        private readonly string _emailTextBox                     ="//*[@AutomationId='emailTextBox']";
        private readonly string _mobileNoTextBox                  ="//*[@AutomationId='mobileNoTextBox']";
        private readonly string _faxNoTextBox                     ="//*[@AutomationId='faxNoTextBox']";
        private readonly string _exemptionComboBox                ="//*[@AutomationId='exemptionComboBox']";
        private readonly string _ethinicity                       = "[F12 - Add]";
        private readonly string _ethinicityRadioButtonSelect      = "//*[@Name='White']//*[@ClassName='RadioButton']//*['{0}']";
        private readonly string _ethinicityOkButton               = "[F10 - OK]";
        private readonly string _prescriber                       = "[Add]";
        private readonly string _prescriberSelect                 = "//* [@ClassName='MenuItem']//*[@Name='{0}']";
        private readonly string _prescriberNameSelect             ="//*[@Name='{0}']";
        private readonly string _prescriberNameSelectOkButton     = "[F10 - Select]";
        private readonly string _prescriberOrganisation           = "[Alt+G Select Prescribing Organisation]";
        private readonly string _prescriberOrganisationNameSelect ="//*[@Name='{0}']";
        private readonly string _prescriberOrgSelectOkButton      = "[F10 - Select]";
        private readonly string _addNotes                         ="//*[@Name='_Notes']//*[@AutomationId='FocusItem']";
        private readonly string _joinNursingHome                  ="[F9 - Join Nursing Home]";
        private readonly string _chooseNursingHome                ="//*[@ClassName='ContextMenu']//*[@Name='{0}']";
        private readonly string _nursingNameSelect                ="//*[@AutomationId='searchUserControl']//*[@ClassName='BaseDataGrid']//*[@Name='{0}']";
        private readonly string _yesButton                        = "//*[@AutomationId='_Yes']";
        /*  private readonly string _addingNotes                  = "[Add Note]";
          private readonly string _carers                         = "[Alt+F9 - Add]";
          private readonly string _carerCancelButton              ="[Cancel]";
          private readonly string _carerSearchresultsText        = "//*[@ClassName='ProScriptConnectSearchView']//*[@ClassName='BaseDataGrid']//*[@Name='{0}']";
          private readonly string _certificateTextBox             ="//*[@AutomationId='certificateTextBox']";
         */
        private readonly string username                          = "Admin";
        private readonly string password                          ="admin1234";
        private readonly string dataSource                        =".\\Pharmacye14";
        private readonly string initialCatalog                    ="ProScriptConnect";
        private readonly string integratedSecurity                ="True";
        private readonly string connectionTimeout                 ="45";
        private readonly string _F10Save                          = "[F10 - Save]";

        public void EnterTermInSearchBox (string searchTerm) => BasePage.SetText(_searchInput, searchTerm, ElementWaitState.PRESENT, 10);
        public void SelectPatientFromSearchResults (string patientName)
        {
            try
            {
                if (BasePage.IsElementPresent(String.Format(_patientSearchresultsText, patientName), 15))
                {
                    BasePage.DoubleClick(String.Format(_patientSearchresultsText, patientName));
                }
            }
            catch
            {
                throw new Exception("Unable to find the patient. Check whether the patient name is spelled right/or Listed in DB");
            }
        }
        public void EnterFirstName (string firstName)              => BasePage.SetText(_firstnameTextBox, firstName, ElementWaitState.PRESENT, 10);
        public void EnterSurName (string surName)                  => BasePage.SetText(_surnameTextBox, surName, ElementWaitState.PRESENT, 10);
        public void EnterDob (string dob)                          => BasePage.SetText(_dob, dob, ElementWaitState.PRESENT, 10);
        public void EnterGender (string gender)                    => BasePage.SetText(_gender, gender, ElementWaitState.PRESENT, 10);
        public void EnterStreetName (string streetName)            => BasePage.SetText(_streetTextBox, streetName, ElementWaitState.PRESENT, 10);
        public void EnterPostCode (string postCode)                => BasePage.SetText(_postCodeTextBox, postCode, ElementWaitState.PRESENT, 10);
        public void EnterExemption (string exemptionType)          => BasePage.SetText(_exemptionComboBox, exemptionType, ElementWaitState.PRESENT, 10);
        public void EnterTitle (string title)                      => BasePage.SetText(_title, title, ElementWaitState.PRESENT, 10);
        public void EnterMiddleName (string middleName)            => BasePage.SetText(_middlenameTextBox, middleName, ElementWaitState.PRESENT, 10);
        public void EnterNhsNumber (string nhsNo)                  => BasePage.SetText(_nhsnumber, nhsNo, ElementWaitState.PRESENT, 10);
        public void EnterPatientNumber (string patientNo)          => BasePage.SetText(_patientNoTextBox, patientNo, ElementWaitState.PRESENT, 10);
        // public void EnterPrefferedNumber (string preferredName) => BasePage.SetText(_prefferedName, preferredName, ElementWaitState.PRESENT, 10);
        public void EnterHouseNumber (string houseNo)              => BasePage.SetText(_houseNoTextBox, houseNo, ElementWaitState.PRESENT, 10);
        public void EnterStreet (string streetName)                => BasePage.SetText(_streetTextBox, streetName, ElementWaitState.PRESENT, 10);
        public void EnterCity (string city)                        => BasePage.SetText(_cityTextBox, city, ElementWaitState.PRESENT, 10);
        public void EnterHomeNumber (string homeNo)                => BasePage.SetText(_homeNoTextBox, homeNo, ElementWaitState.PRESENT, 10);
        public void EnterMobileNumber (string mobileNo)            => BasePage.SetText(_mobileNoTextBox, mobileNo, ElementWaitState.PRESENT, 10);
        public void EnterWorkNumber (string workNo)                => BasePage.SetText(_workNoTextBox, workNo, ElementWaitState.PRESENT, 10);
        public void EnterFaxNumber (string faxNo)                  => BasePage.SetText(_faxNoTextBox, faxNo, ElementWaitState.PRESENT, 10);
        public void EnterEmail (string email)                      => BasePage.SetText(_emailTextBox, email, ElementWaitState.PRESENT, 10);
        public void EnterExemptionType (string exemptionType)      => BasePage.SetText(_exemptionComboBox, exemptionType, ElementWaitState.PRESENT, 10);
        public void ClickEthinicity (string ethinicity)            => BasePage.Click(_ethinicity);
        public void ClickF1AddPatientButton ()                     => BasePage.Click(_f1AddPatient);
        public void ClickF10SaveButton ()                          => BasePage.Click(_F10Save);
        public void AddNewPatientScreenValidation ()
        {
            string title = BasePage.GetText(_addNewPatientScreen);
            if (title.Equals("Add New Patient"))
            {
                Console.WriteLine("Navigated to Add Patient Screen");
            }
            else
            {
                Console.WriteLine("Error,uanble to navigate to Add Patient Screen");
            }
        }

        public void EnterAllFields (string title, string firstName, string surName, string middleName, string nhsNo, string patientNo, string preferredName,
           string dob, string gender, string houseNo, string streetName, string city, string town, string postCode, string homeNo, string mobileNo, string workNo,
            string faxNo, string email, string exemptionType, string ethinicity, string prescribers, string prescriberName, string prescriberOrgName, string note, string joinnursinghome, string homename)
        {
            EnterTitle(title);
            EnterMandatoryPatientInfo(firstName, surName, middleName, dob, gender, streetName, postCode, exemptionType);
            EnterNhsNumber(nhsNo);
            EnterPatientNumber(patientNo);
            EnterCity(city);
            BasePage.SetText(_countryTextBox, town);
            EnterHouseNumber(houseNo);
            EnterMobileNumber(mobileNo);
            EnterFaxNumber(faxNo);
            // EnterWorkNumber(workNo);
            // BasePage.SetText(_prefferedName, preferredName);
            //BasePage.SetText(_homeNoTextBox, homeNo);
            EnterEmail(email);
            ClickEthinicity(ethinicity);
            BasePage.Click(String.Format(_ethinicityRadioButtonSelect, ethinicity));
            BasePage.Click(_ethinicityOkButton);
            BasePage.Click(_prescriber);
            BasePage.Click(String.Format(_prescriberSelect, prescribers));
            BasePage.Click(String.Format(_prescriberNameSelect, prescriberName));
            BasePage.Click(_prescriberNameSelectOkButton);
            BasePage.Click(_prescriberOrganisation);
            BasePage.Click(String.Format(_prescriberOrganisationNameSelect, prescriberOrgName));
            BasePage.Click(_prescriberOrgSelectOkButton);
            BasePage.SetText(_addNotes, note);
            //Thread.Sleep(2000);           
            // BasePage.Click(_carers);
            // BasePage.Click(_carerCancelButton);
            JoiningNursingHome(joinnursinghome, homename);
        }

        public void EnterMandatoryPatientInfo (string FirstName, string SurName, string MiddleName, string DOB, string Gender, string StreetName, string PostCode, string ExemptionType)
        {
            try
            {
                //FirstName
                if (FirstName != null)
                {
                    BasePage.SetText(_firstnameTextBox, FirstName);
                    Console.WriteLine("The firstName is entered as " + FirstName);
                }
                else if (FirstName == null)
                {

                    // _firstnameTextBox
                }
                else
                {
                    Console.WriteLine("The firstName could not be updated as " + FirstName);
                }
                //Surname
                if (SurName != null)
                {
                    BasePage.SetText(_surnameTextBox, SurName, ElementWaitState.PRESENT, 10);
                    Console.WriteLine("The surName is entered as " + SurName);
                }
                else if (SurName == null)
                {
                    //
                }
                else
                {
                    Console.WriteLine("The surName could not be updated as " + SurName);
                }
                //MiddleName
                if (MiddleName != null)
                {
                    BasePage.SetText(_middlenameTextBox, MiddleName, ElementWaitState.PRESENT, 10);
                    Console.WriteLine("The MiddleName is entered as " + MiddleName);
                }
                else if (MiddleName == null)
                {
                    //
                }
                else
                {
                    Console.WriteLine("The MiddleName could not be updated as " + MiddleName);
                }
                //DOB
                if (DOB != null)
                {
                    BasePage.SetText(_dob, DOB, ElementWaitState.PRESENT, 10);
                    Console.WriteLine("The dateOfBirth_Edit is entered as " + DOB);
                }
                else if (DOB == null)
                {
                    //
                }
                else
                {
                    Console.WriteLine("The dateOfBirth_Edit could not be updated");
                }

                if (Gender != null)
                {
                    BasePage.SetText(_gender, Gender, ElementWaitState.PRESENT, 10);
                    Console.WriteLine("The Gender is selected as " + Gender);
                }
                else if (Gender == null)
                {
                    //
                }
                else
                {
                    Console.WriteLine("The gender could not be updated ");
                }

                if (StreetName != null)
                {
                    BasePage.SetText(_streetTextBox, StreetName, ElementWaitState.PRESENT, 10);
                    Console.WriteLine("The Gender is selected as " + StreetName);
                }
                else if (StreetName == null)
                {
                    //
                }
                else
                {
                    Console.WriteLine("The StreetName could not be updated ");
                }
                if (PostCode != null)
                {
                    BasePage.SetText(_postCodeTextBox, PostCode, ElementWaitState.PRESENT, 10);
                    Console.WriteLine("The PostCode is entered as " + PostCode);
                }
                else if (PostCode == null)
                {
                    //
                }
                else
                {
                    Console.WriteLine("The postCode could not be updated");
                }
                if (ExemptionType != null)
                {
                    BasePage.SetText(_exemptionComboBox, ExemptionType, ElementWaitState.PRESENT, 10);
                    Console.WriteLine("The ExemptionType is entered as " + ExemptionType);
                }
                else if (ExemptionType == null)
                {
                    //
                }
                else
                {
                    Console.WriteLine("The exemptionType could not be updated");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The values could not be updated " + e.Message);
            }
        }

        public bool ValidatePatientInDb ()
        {
            string SurName           ="Patient";
            string FirstName         ="UserTest";
            //string MiddleName         ="Q";
            int DBCount              =0;
            bool name                = false;
            SqlConnection sc         = helper.GetSqlConnection(dataSource, initialCatalog, integratedSecurity,connectionTimeout,username,password);
            string checkPatientCount = "Select COUNT(*) AS COUNT from dbo.Patient where Deleted != 1 and SurName = '" + SurName + "' and GivenName = '" + FirstName + "';";

            SqlCommand cmd           = new SqlCommand(checkPatientCount);
            cmd.Connection = sc;
            sc.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                DBCount = int.Parse(rdr["Count"].ToString());
            }
            if (DBCount >= 1)
            {
                Console.WriteLine("The record was  added to the Dataabse");
                name = true;
            }
            else
            {
                Console.WriteLine("The record was not added to the Dataabse and adding creating the data now");
            }
            return name;
        }

        public int PatientCountCheck ()
        {
            string SurName           ="Patient";
            string FirstName         ="UserTest";
            int DBCount              =0 ;
            SqlConnection sc         = helper.GetSqlConnection(dataSource, initialCatalog, integratedSecurity,connectionTimeout,username,password);
            string checkPatientCount = "Select COUNT(*) AS COUNT from dbo.Patient where Deleted != 1 and SurName = '" + SurName + "' and GivenName = '" + FirstName + "';";
            SqlCommand cmd           = new SqlCommand(checkPatientCount);
            cmd.Connection = sc;
            sc.Open();
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DBCount = int.Parse(rdr["Count"].ToString());
            }
            return DBCount;
        }

        public void PatientCreationInDB (string gender, string surName, string givenName, string dob, string street, string postCode, string exemptionType)
        {
            string ethinicity         ="";
            string PrescriberFirstname="";
            string PrescriberSurName  ="";
            string nursingHomeName    ="";
            string NACSCode           ="FA391";
            string nhsNumber          ="";
            string middleName="Q";
            SqlConnection sc         = helper.GetSqlConnection(dataSource, initialCatalog, integratedSecurity,connectionTimeout,username,password);
            string spPatientCreation =
  @"begin tran                                       
                                        declare @Patient table(PatientId int)                                        
                                        declare @PharmacyId int =(Select top 1 PharmacyId from ProScriptConnect.dbo.Pharmacy where NationalPracticeCode = '" + NACSCode + @"')
                                       
                                                                     
                                        declare @EthnicityId int = (select  EthnicityId from [ProScriptConnect].[dbo].[Ethnicity] where Description ='" + ethinicity + @"')
                                        declare @PresciberId int = (select [PrescriberId] from [ProScriptConnect].[dbo].[Prescriber] where Forename = '" + PrescriberFirstname + "' and Surname ='" + PrescriberSurName + @"' and PharmacyId = @PharmacyId)
                                        declare @PrescribingOrganisationId int = (select [PrescribingOrganisationId] from [ProScriptConnect].[dbo].[PrescribingOrganisation] where Name = 'Test PrescribingOrganisation'  and  PharmacyId = @PharmacyId)
                                        declare @NursingHomeId int = (SELECT NursingHomeId FROM [ProScriptConnect].[NursingHome].[NursingHome] where PharmacyId = @PharmacyId and Name = '" + nursingHomeName + @"')
                                       Insert [ProScriptConnect].[dbo].[Address] (Postcode,NumberAndStreet) values ('" + postCode + "','" + street + @"')
                                        declare @AddressId int = (Select MAX (AddressId) from [ProScriptConnect].[dbo].[Address])
INSERT [ProScriptConnect].[dbo].[Patient] (PatientGuid,PharmacyId,DateAdded,Title,Sex,Surname,CallingName,GivenName,MiddleNames,
                                                      DateOfBirth,Homeless,ETPEnabled,SendSMS, PatientTypeId,Deleted,DummyPatient,IsMdsPrinted,MurPrintLabelsWhenSnooze,
                                                      AutoShowCounsellingNotes,ExemptionCode,AddressId,EthnicityId,NursingHomeId) 
                                                             output inserted.PatientId into @Patient
                                                      VALUES (newid(), @PharmacyId, getdate(),'','" + gender + "','" + surName + "','','" + givenName + "','" + middleName + "',convert(datetime, '" + dob +
  @"'), 0, 0, 0, 1, 0,0,0,0,0,'" + exemptionType + @"',@AddressId,@EthnicityId,@NursingHomeId)
                                        declare @PatId int = (select PatientId from @Patient)
                                        declare @PrescriberPrescribingOrgId int = (SELECT top 1 PrescriberPrescribingOrganisationId 
                                                                                                                                                      FROM       [ProScriptConnect].[dbo].[PrescriberPrescribingOrganisation] PPO
                                                                                                                                                      WHERE @PrescribingOrganisationId = PPO.PrescribingOrganisationId
                                                                                                                                                      AND @PresciberId = PPO.PrescriberId
                                                                                                                                                      AND PPO.Deleted <> 1)
                                        INSERT [ProScriptConnect].[dbo].[PatientPrescriber]([PatientId], [PrescriberPrescribingOrganisationId], [PatientPrescriberTypeId])
                                        VALUES (@PatId,1,1)
                                        INSERT [ProScriptConnect].[dbo].[PatientIdentifier] (PatientId,PatientIdentifierTypeId, Value)
                                        VALUES (@PatId,1,'" + nhsNumber + @"')
                                        Commit";
            helper.ExecuteQuery(sc, spPatientCreation);
            int count = PatientCountCheck();
            if (count == 1)
            {
                Console.Write("Patient is created successfully in database");
            }
            else
            {
                Console.Write("Unable to create a patient in database");
            }
        }
        public void DeleteDataFromDb (string SurNameCreate)
        {
            SqlConnection sc= helper.GetSqlConnection(dataSource, initialCatalog, integratedSecurity,connectionTimeout,username,password);
            string deletedQuery = "Update dbo.Patient set Deleted = '1' where Surname = '" + SurNameCreate + "'";
            helper.ExecuteQuery(sc, deletedQuery);
        }
        public void JoiningNursingHome (string choosingHome, string nameSelect)
        {
            BasePage.Click(_joinNursingHome);
            BasePage.Click(String.Format(_chooseNursingHome, choosingHome));
            BasePage.Click(String.Format(_nursingNameSelect, nameSelect));
            BasePage.Click(_prescriberOrgSelectOkButton);
            BasePage.Click(_yesButton);
        }
    }
}