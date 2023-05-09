Feature: Patient Information Leaflets
    Patient Information Leaflets is used to provide the patients with medical articles to get better knowledge of Drugs and Diseases.

@Regression @PatientInformationLeaflet
  Scenario: Verifying Patient Information Leaflets in ProScript Connect Main menu
  #Test Case Id: 119413
    Given I am in PSC login screen
     When I login to PSC application using below user
      | UserName   | Password | NACS Code |
      | Supervisor | supe     | FA391     |
      And I click Proscript Connect menu 
      And I click Patient Information Leaflets under UTILITIES category in main menu
      And I invoke cef browser
     Then I verify Information Leaflets is opened
     When I enter Asthma in search bar
     Then I verify the search results is displayed

@Regression @PatientInformationLeaflet
  Scenario: Verifying Patient Information Leaflets in PMR page
  #Test Case Id: 119408
    Given I am in PSC login screen
     When I login to PSC application using below user
      | UserName   | Password | NACS Code |
      | Supervisor | supe     | FA391     |
      And I search for the patient MONKEY
      And I select the patient MONKEY, Luffy D from the patient search results
      And I handle all warnings in PMR page
      And I click Patient Information Leaflets button
      And I invoke cef browser
     Then I verify Information Leaflets is opened
     When I enter Asthma in search bar
     Then I verify the search results is displayed

@Regression @PatientInformationLeaflet
  Scenario: Verifying Patient Information Leaflets in Drug search view
  #Test Case Id: 119408
  Given I am in PSC login screen
     When I login to PSC application using below user
      | UserName   | Password | NACS Code |
      | Supervisor | supe     | FA391     |
      And I search for the patient MONKEY
      And I select the patient MONKEY, Luffy D from the patient search results
      And I handle all warnings in PMR page
      And I click ENTER - Add New Item button in PMR
      And I enter and select Aspirin 150mg suppos from drug search result
      And I click Patient Information Leaflets button
      And I invoke cef browser
     Then I verify Information Leaflets is opened

@Regression @PatientInformationLeaflet
  Scenario: Verifying Patient Information Leaflets in OverDue Scripts
  #Test Case Id: 119408
  Given I am in PSC login screen
     When I login to PSC application using below user
      | UserName   | Password | NACS Code |
      | Supervisor | supe     | FA391     |
      And I click Proscript Connect menu 
      And I click Overdue Scripts under PMR & DISPENSING category in main menu
      And I click Patient Information Leaflets button
      And I invoke cef browser
     Then I verify Information Leaflets is opened
     When I enter Asthma in search bar
     Then I verify the search results is displayed

@Regression @PatientInformationLeaflet
  Scenario: Verifying Patient Information Leaflets in ETP
  #Test Case Id: 119408
  Given I am in PSC login screen
     When I login to PSC application using below user
      | UserName   | Password | NACS Code |
      | Supervisor | supe     | FA391     |
      And I click Proscript Connect menu 
      And I click ETP under PMR & DISPENSING category in main menu
      And I click Patient Information Leaflets button
      And I invoke cef browser
     Then I verify Information Leaflets is opened
     When I enter Asthma in search bar
     Then I verify the search results is displayed

@Regression @PatientInformationLeaflet
  Scenario: Verifying Patient Information Leaflets in RMS Manager
  #Test Case Id: 119408
  Given I am in PSC login screen
     When I login to PSC application using below user
      | UserName   | Password | NACS Code |
      | Supervisor | supe     | FA391     |
      And I click Proscript Connect menu 
      And I click Repeat Management (RMS) under MANAGEMENT category in main menu
      And I click Patient Information Leaflets button
      And I invoke cef browser
     Then I verify Information Leaflets is opened
     When I enter Asthma in search bar
     Then I verify the search results is displayed
