using BaseProject.Helpers;
using Selenium.BasePage;
using TechTalk.SpecFlow;
using Utilities;

namespace BaseProject.Hooks
{
	[Binding]
	public class CommonHooks
	{
		private readonly ProcessManager _processManager;

		public CommonHooks ()
		{
			_processManager = new ProcessManager();
		}

		/// <summary>
		/// Kill all the ProScript Connect process if running
		/// Initialises the Windows Driver and bring the ProScript Connect applicaiton in the foreground.
		/// </summary>
		/// <returns></returns>

		[BeforeScenario(Order = 1)]
		public void InitialiseDriver ()
		{
			BasePage.InitWinAppDriver(
				ProjectSettings.WindowsApplicationDriverUrl,
				ProjectSettings.ApplicationPath,
				ProjectSettings.ApplicationDirectory,
				ProjectSettings.ApplicationTopLevelWindow);
			_processManager.SetForegroundProcess("ProScriptConnect.Client");
		}

		/// <summary>
		/// Kills the ProScript Connect client and Cef Client processes
		/// </summary>
		/// <returns></returns>

		[AfterScenario(Order = 1)]
		public void KillProScriptConnectProcess ()
		{
			_processManager.KillProcess("cefclient");
			_processManager.KillProcess("ProScriptConnect.Client");
		}
	}
}
