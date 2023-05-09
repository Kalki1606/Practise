using System;
using BaseProject.Helpers;
using Selenium.BasePage;

namespace BaseProject.PageObjects.ProscriptConnectMenu
{
	public class ProscriptConnectMenuPage
	{
		private readonly string _proscriptConnectMenuButton = "//*[@AutomationId='mainMenuButton']";
		private readonly string _menuItemsButton            = "//*[@AutomationId='{0} - {1}']";

		public void ClickProscriptConnectMenuButton () => BasePage.Click(_proscriptConnectMenuButton);

		public void ClickMenuButton (string menuName, string categoryName) => BasePage.Click(String.Format(_menuItemsButton, categoryName, menuName));

		public void InitCef () => BasePage.InitCefDriver(ProjectSettings.RemoteDebuggingPort);
	}
}