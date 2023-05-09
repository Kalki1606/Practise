using System.Threading;
using BaseProject.Helpers;
using Selenium.BasePage;

namespace BaseProject.PageObjects.Common
{
	public class LoginPage
	{
		private readonly string _usernameInput = "//*[@AutomationId='userTextBox']";
		private readonly string _passwordInput = "//*[@AutomationId='passwordBox']";
		private readonly string _loginButton   = "[F10 - Login]";

		public string GetLoginPageTitle () => BasePage.GetTitle();
		public void EnterCredentials (string userName, string password)
		{
			BasePage.SetText(_usernameInput, userName);
			BasePage.SetText(_passwordInput, password);
			BasePage.Click(_loginButton);
			BasePage.InitWinAppDriver(ProjectSettings.WindowsApplicationDriverUrl, ProjectSettings.ApplicationPath, ProjectSettings.ApplicationDirectory, "ProScript Connect");
		}
	}
}