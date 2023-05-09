using System.Collections.Generic;
using System.Linq;
using BaseProject.PageObjects.Common;
using NUnit.Framework;
using Selenium.Utilities;
using TechTalk.SpecFlow;

namespace BaseProject.StepDefinitions.Common
{
	[Binding]
	public class LoginSteps
	{
		private readonly LoginPage _loginPage;
		public LoginSteps ()
		{
			_loginPage = new LoginPage();
		}

		[Given(@"I am in PSC login screen")]
		public void GivenIAmInPscLoginScreen ()
		{
			Assert.AreEqual("ProScript Connect Login", _loginPage.GetLoginPageTitle());
		}

		[When(@"I login to PSC application using below user")]
		public void WhenILoginToPSCApplicationUsingBelowUser (Table table)
		{
			Dictionary<string, string> tableData = table.ToDictionary();
			_loginPage.EnterCredentials(tableData["UserName"], tableData["Password"]);
		}
	}
}