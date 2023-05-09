using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
namespace Utilities
{
	//
	// Summary: Contains methods to access the local system processes
	//  
	public class ProcessManager
	{
		//
		// Summary:
		//     Starts the process using the process location
		//
		// Parameters:
		//   processPath:
		//     The absolute path of the process
		public void StartProcess (string applicationDirectory, string applicationPath)
		{
			ProcessStartInfo processStartInfo = new ProcessStartInfo
			{
				UseShellExecute = true,
				CreateNoWindow = true,
				WorkingDirectory = applicationDirectory,
				FileName = applicationPath
			};
			Process.Start(processStartInfo);
		}

		//
		// Summary:
		//     Kills the process using the process name
		//
		// Parameters:
		//   processName:
		//     The friendly name of the process.
		public void KillProcess(string processName)
		{
			Process[] windowsProcess = Process.GetProcessesByName(processName);
			for (int i = 0; i < windowsProcess.Length; i++)
			{
				windowsProcess[i].Kill();
			}
		}

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		//
		// Summary:
		//     Minimizes the mentioned process
		//
		// Parameters:
		//   processName:
		//     The friendly name of the process.
		public void MinimizeProcess(string processName)
		{
			Process[] windowsProcess = Process.GetProcessesByName(processName);
			for (int i = 0; i < windowsProcess.Length; i++)
			{
				ShowWindow(windowsProcess[i].MainWindowHandle, 6);
			}
		}

		[DllImport("USER32.DLL")]
		static extern bool SetForegroundWindow(IntPtr hWnd);
		//
		// Summary:
		//     Bring the process to the foreground
		//
		// Parameters:
		//   processName:
		//     The friendly name of the process.
		public void SetForegroundProcess(string processName)
		{
			Process[] windowsProcess = Process.GetProcessesByName(processName);
			for (int i = 0; i < windowsProcess.Length; i++)
			{
				SetForegroundWindow(windowsProcess[i].MainWindowHandle);
			}
		}
	}

}
