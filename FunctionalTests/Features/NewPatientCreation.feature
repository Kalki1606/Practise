Feature: NewPatientCreation

Creating New Patient Record in the PSC Application

@Regression @PatientCreation
Scenario: Verifying Patient is created with all Mandatory Fields
	Given I am in PSC login screen
	When I login to PSC application using below user
		| UserName   | Password | NACS Code |
		| Supervisor | supe     | FA391     |
	And I search patient with
		| SurName |
		| Patient |
	And I search for patient Patient, UserTest from the patient search results
	When I click the Add Patient button
	Then I should navigate to Add New Patient Screen
	And I enter the mandatory fileds
		| First Name | Sur Name | DOB        | Gender | Street Name | Post Code | Exemption Type | MiddleName |
		| UserTest   | Patient  | 12-03-1952 | Male   | High Street | WD17 1DS  | A              | Q          |
	When I click on Save Button
	Then I verify created patient is listed in the database
	
@Regression
Scenario: Verifying New Patient is created with all Fields
	Given I am in PSC login screen
	When I login to PSC application using below user
		| UserName   | Password | NACS Code |
		| Supervisor | supe     | FA391     |
	And I search patient with
		| SurName |
		| Patient |
	And I search for patient Patient, UserTest Q from the patient search results
	And I click the Add Patient button
	Then I should navigate to Add New Patient Screen
	And I enter the details of the patient
		| Title | First Name | Middle Name | Sur Name | NHS No | Patient No | Preffered Name | DOB        | Gender | House Number | Street Name | City | Town | Post Code | Home No | Mobile No | Work No | Fax No | Email            | Exemption Type | Ethinicity | Prescribers | Prescriber Name              | Prescriber Org Name          | Note                | JoinNursingHome   | HomeName        |
		| Dr    | UserTest   | Q           | Patient  |        | 2345       | Mark           | 10-07-1958 | Male   | 5612111      | ParWay      | City | town | A9A 9AA   | 123     | 1233      | 432     | 43212  | unknown@test.com | A              | Indian     | Dentist     | Test PrescribingOrganisation | Test PrescribingOrganisation | Patient has Allergy | Join Nursing Home | TestNursingHome |
	When I click on Save Button
	And I handle all warnings in PMR page
	Then I verify created patient is listed in the database

@Regression
Scenario: Verifying Patient is created direclty in Db
	Given I am in PSC login screen
	When I login to PSC application using below user
		| UserName   | Password | NACS Code |
		| Supervisor | supe     | FA391     |
	And I search patient with
		| SurName |
		| Patient |
	And I search for patient Patient, UserTest from the patient search results
	And I create a new patient record using the below details
		| Gender | Sur Name | First Name | DOB        | Street Name | Post Code | Exemption Type |
		| M      | Patient  | UserTest   | 04-01-1941 | High Street | WD17 1DS  | C              | 
	Then I verify created patient is listed in the database
	