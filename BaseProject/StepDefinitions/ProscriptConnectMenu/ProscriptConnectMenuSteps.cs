using BaseProject.PageObjects.ProscriptConnectMenu;
using TechTalk.SpecFlow;

namespace BaseProject.StepDefinitions.Proscript_Connect_Menu
{
	[Binding]
	public class ProscriptConnectMenuSteps
	{
		private readonly ProscriptConnectMenuPage _proscriptConnectMenuPage;
		public ProscriptConnectMenuSteps ()
		{
			_proscriptConnectMenuPage = new ProscriptConnectMenuPage();
		}

		[When(@"I click Proscript Connect menu")]
		public void WhenIClickProscriptConnectMenu ()
		{
			_proscriptConnectMenuPage.ClickProscriptConnectMenuButton();
		}

		[When(@"I click (.*) under (PMR & DISPENSING|PRESCRIBERS|PATIENTS|Training|MANAGEMENT|UTILITIES|REPORTS|CONTROL PANEL|SECURITY) category in main menu")]
		public void WhenIClickMenuNameFromCategoryInMainMenu (string menuName, string categoryName)
		{
			_proscriptConnectMenuPage.ClickMenuButton(menuName, categoryName);
		}

		[When(@"I invoke cef browser")]
		public void WhenIInvokeCefBrowser ()
		{
			_proscriptConnectMenuPage.InitCef();
		}
	}
}