using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Runtime.InteropServices;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using System.Collections.ObjectModel;
using Keys = OpenQA.Selenium.Keys;
using OpenQA.Selenium.Appium;
using Utilities;
using Castle.Core.Internal;

namespace Selenium.BasePage
{
	/// <summary>
	/// Contains value that indicate the allowed Click Types for peforming click actions (click, double click, context click) an element
	/// </summary>
	public enum ClickType
	{
		DIRECT_API_CLICK = 1,
		MOUSE_CLICK = 2,
		JAVASCRIPT_CLICK = 3
	}

	/// <summary>
	/// Contains value that indicate the allowed Select Strategy for interacting with dropdowns
	/// </summary>
	public enum SelectStrategy
	{
		VISIBLE_TEXT = 1,
		VALUE = 2,
		INDEX = 3
	}

	/// <summary>
	/// Contains value that indicate the allowed wait states for waiting for an element
	/// </summary>
	public enum ElementWaitState
	{
		PRESENT = 1,
		VISIBLE = 2,
		CLICKABLE = 3,
		PRESENT_OF_ALL = 4,
		VISIBLE_OF_ALL = 5
	}

	/// <summary>
	/// Contains value that indicate the allowed alert actions on web popups
	/// </summary>
	public enum AlertAction
	{
		ACCEPT = 1,
		DISMISS = 2
	}

	/// <summary>
	/// Contains value that indicate the allowed Element type used for get element(s)
	/// </summary>
	public enum ElementType
	{
		SINGLE = 1,
		MULTIPLE = 2
	}


	/// <summary>
	/// Contains a wrapper implementation to interact with webelements using Selenium library and CEF (Chrome Embedded Framework)
	/// </summary>
	public class BasePage
	{
		private static IWebDriver Driver { get; set; }
		private static string CEFClient = "cefclient.exe";
		private static readonly int defaultTimeOut = 30;
		private static readonly string idRegex = @"^([#])(.*)";
		private static readonly string xpathRegex = @"^[\\/].*|^[(].*";
		private static readonly string classRegex = @"^([.])(.*)";
		private static readonly string linkTextRegex = @"^([@])(.*)";
		private static readonly string partialLinkTextRegex = @"^([@])(.*)([.][.][.])$";
		private static readonly string tagRegex = @"^([<])(.*)([>])$";
		private static readonly string cssRegex = @"^([c][s][s][=])(.*)";
		private static readonly string nameRegex = @"^([\[])(.*)[\]]$";

		private static ChromeOptions GetChromeOptions(int remoteDebuggingPort)
		{
			ChromeOptions option = new ChromeOptions();
			string ParentDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
			string currentDirectory = Path.GetFullPath(Path.Combine(ParentDirectory, @"..\"));
			option.BinaryLocation = currentDirectory + @"FrameworkCapabilities\Resources" + @"\CEF\" + CEFClient;
			option.AddArgument("remote-debugging-port="+remoteDebuggingPort);
			return option;
		}

		private static ChromeDriver GetChromeDriver(int remoteDebuggingPort)
		{
			return new ChromeDriver(Path.GetFullPath(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, @"..\")) + @"FrameworkCapabilities\Resources\ChromeDriver\", GetChromeOptions(remoteDebuggingPort));
		}

		public static void InitCefDriver(int remoteDebuggingPort= 7555)
		{
			try
			{				
				Driver = GetChromeDriver(remoteDebuggingPort);
				Thread.Sleep(3000);
				ProcessManager processManager = new ProcessManager();
				processManager.MinimizeProcess("cefclient");
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to instantiate driver. Error : {ex}");
			}
		}

		public static void InitWinAppDriver(string windowsApplicationDriverUrl, string applicationPath, string applicationDirectory, string applicationTopLevelWindow)
		{
			try
			{
				Driver = GetWinAppDriver(windowsApplicationDriverUrl, applicationPath, applicationDirectory, applicationTopLevelWindow);
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to instantiate driver. Error : {ex}");
			}
		}		

		public static WindowsDriver<WindowsElement> GetWinAppDriver(string windowsApplicationDriverUrl, string applicationPath, string applicationDirectory, string applicationTopLevelWindow)
		{
			WindowsDriver<WindowsElement> windowsDriver;
			AppiumOptions appCapabilities;
			bool exit;
			ProcessManager processManager = new ProcessManager();
			processManager.StartProcess(applicationDirectory,applicationPath);			
			do
			{
				appCapabilities = new AppiumOptions();
				appCapabilities.AddAdditionalCapability("app", "Root");
				windowsDriver = new WindowsDriver<WindowsElement>(new Uri(windowsApplicationDriverUrl), appCapabilities);
				try
				{						
					if (windowsDriver.FindElementByName(applicationTopLevelWindow).Displayed)
					{
						IWebElement ele = windowsDriver.FindElementByName(applicationTopLevelWindow);
						string appTopLevelWindowHex = int.Parse(ele.GetAttribute("NativeWindowHandle")).ToString("X");
						appCapabilities = new AppiumOptions();
						appCapabilities.AddAdditionalCapability("appTopLevelWindow", appTopLevelWindowHex);
						appCapabilities.AddAdditionalCapability("appWorkingDir", applicationDirectory);
						windowsDriver = new WindowsDriver<WindowsElement>(new Uri(windowsApplicationDriverUrl), appCapabilities);
						exit = false;
					}
				return windowsDriver;
				}
				catch(Exception)	
				{
					exit = true;
				}					
			} while (exit);
			return null;
		}		

		/// <summary>
		/// Opens the given url in browser
		/// </summary>
		/// <param name="url">The url of the application under test</param>
		/// <returns></returns>
		public static void Open(string url)
		{
			try
			{
				Driver.Navigate().GoToUrl(url);
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to open the given url. Error : {ex}");
			}
		}

		/// <summary>
		/// Returns the title of the current page.
		/// </summary>
		/// <returns></returns>
		public static string GetTitle()
		{
			try
			{
				return Driver.Title;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to get the title of the page. Error : {ex}");
			}
		}

		/// <summary>
		/// Closes the current window.
		/// </summary>
		/// <returns></returns>
		public static void Close()
		{
			try
			{
				Driver.Close();
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to close the current window. Error : {ex}");
			}
		}

		/// <summary>
		/// Quits the driver and closes every associated window.
		/// </summary>
		/// <returns></returns>
		public static void Quit()
		{
			try
			{
				Driver.Quit();
				Driver = null;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to quit driver and associated windows. Error : {ex}");
			}
		}

		/// <summary>
		/// Returns the current url of the page
		/// </summary>
		/// <returns></returns>
		public static string GetCurrentUrl()
		{
			try
			{
				return Driver.Url;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to get the URL of the current page. Error : {ex}");
			}
		}

		/// <summary>
		///	Gets the position of of the current window.
		/// </summary>
		/// <returns></returns>
		public static Point GetCurrentWindowRectangle()
		{
			try
			{
				return Driver.Manage().Window.Position;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to get current window rect. Error {ex}");
			}
		}

		/// <summary>
		/// Maximizes the current window
		/// </summary>
		/// <returns></returns>
		public static void MaximizeCurrentWindow()
		{
			try
			{
				Driver.Manage().Window.Maximize();
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to maximize the current window. Error : {ex}");
			}
		}

		/// <summary>
		/// Minimizes the current window
		/// </summary>
		/// <returns></returns>
		public static void MinimizeCurrentWindow()
		{
			try
			{
				Driver.Manage().Window.Minimize();
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to minimize the current window. Error : {ex}");
			}
		}

		/// <summary>
		/// Invokes the full screen operation on the current window.
		/// </summary>
		/// <returns></returns>
		public static void FullScreenCurrentWindow()
		{
			try
			{
				Driver.Manage().Window.FullScreen();
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to perform full screen operation on the current window. Error : {ex}");
			}
		}

		/// <summary>
		/// Refresh the current page.
		/// </summary>
		/// <returns></returns>
		public static void Refresh()
		{
			try
			{
				Driver.Navigate().Refresh();
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to refresh the current page. Error : {ex}");
			}
		}

		/// <summary>
		/// Navigate one step backward on the browser.
		/// </summary>
		/// <returns></returns>
		public static void NavigateBack()
		{
			try
			{
				Driver.Navigate().Back();
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to navigate one step backward on the browser. Error : {ex}");
			}
		}

		/// <summary>
		/// Navigate one step forward on the browser.
		/// </summary>
		/// <returns></returns>
		public static void NavigateForward()
		{
			try
			{
				Driver.Navigate().Forward();
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to navigate one step forward on the browser. Error : {ex}");
			}
		}

		/// <summary>
		/// Delete all cookies in the scope of the session.
		/// </summary>
		/// <returns></returns>
		public static void DeleteAllCookies()
		{
			try
			{
				Driver.Manage().Cookies.DeleteAllCookies();
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to delete all cookies in the current session. Error : {ex}");
			}
		}

		/// <summary>
		/// Returns the handle of the current window.
		/// </summary>
		/// <returns></returns>
		public static string GetCurrentWindowHandle()
		{
			try
			{
				return Driver.CurrentWindowHandle;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to get the current window handle. Error : {ex}");
			}
		}

		/// Returns the handle of the opened windows.
		/// </summary>
		/// <returns></returns>
		public static ReadOnlyCollection<string> GetWindowHandles()
		{
			try
			{
				return Driver.WindowHandles;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to get the windows handle. Error : {ex}");
			}
		}

		/// <summary>
		/// Scroll to the bottom of the page.
		/// </summary>
		/// <returns></returns>
		public static void ScrollToBottom()
		{
			try
			{
				int scrollPauseTime = 1;
				object lastHeight = ExecuteJavascript("return document.body.scrollHeight");

				while (true)
				{
					ExecuteJavascript("window.scrollTo(0, document.body.scrollHeight);");
					Thread.Sleep(scrollPauseTime);
					object newHeight = ExecuteJavascript("return document.body.scrollHeight");

					if (newHeight == lastHeight)
						break;
					lastHeight = newHeight;
				}
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to scroll to bottom. Error : {ex}");
			}
		}

		/// <summary>
		/// Scroll to the top of the page.
		/// </summary>
		/// <returns></returns>
		public static void ScrollToTop()
		{
			try
			{
				ExecuteJavascript("window.scrollTo(document.body.scrollHeight, 0);");
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to scroll to top. Error : {ex}");
			}
		}

		/// <summary>
		/// Scroll the element into the viewport.
		/// </summary>
		/// <param name="locator">The string pattern to find the element</param>
		/// <param name="timeOut">The timeout to find the element. If not set, timeout defaults to 30 seconds</param>
		/// <returns></returns>
		public static void ScrollElementInToView(string locator, int timeOut = 0)
		{
			IWebElement element = null;

			try
			{
				element = GetElement(locator, ElementWaitState.PRESENT, true, timeOut);
				ExecuteJavascript("var viewPortHeight = Math.max(document.documentElement.clientHeight, window.innerHeight || 0);" +
					"var elementTop = arguments[0].getBoundingClientRect().top;" +
					"window.scrollBy(0, elementTop-(viewPortHeight/2));", element);
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to scroll element in to viewport. Error : {ex}");
			}
		}

		/// <summary>
		/// Returns the web element based the specified pattern or 'null' if the element is not found.
		/// </summary>
		/// <param name="pattern">The string pattern used to find the element</param>
		/// <param name="waitState">
		/// The wait state for element retrial. Choose state from ElementWaitState class.
		/// Defaults to ElementWaitState.PRESENT.
		/// Allowed states are ElementWaitState.PRESENT, ElementWaitState.VISIBLE, ElementWaitState.INVISIBLE, 
		/// ElementWaitState.CLICKABLE, ElementWaitState.SELECTED, ElementWaitState.FRAME_AVAILABLE_AND_SWITCH_TO 
		/// </param>
		/// <param name="throwException">The boolean to throw exception or not. Defaulted to true.</param>
		/// <param name="timeOut">The timeout to find the element. If not set, timeout defaults to 30 seconds.</param>
		/// <returns>Returns the web element based the specified pattern or 'null' if the element is not found</returns>
		public static IWebElement GetElement(string pattern, ElementWaitState waitState = ElementWaitState.PRESENT, bool throwException = true, int timeOut = 0)
		{
			return (IWebElement)AbstractGetElement(ElementType.SINGLE, pattern, waitState, throwException, timeOut);
		}

		/// <summary>
		/// Returns the list of web elements based the specified pattern or 'null' if the element is not found.
		/// </summary>
		/// <param name="pattern">The string pattern used to find the element</param>
		/// <param name="waitState">
		/// The wait state for element retrial. Choose state from ElementWaitState class.
		/// Defaults to ElementWaitState.PRESENT_OF_ALL.
		/// Allowed states are ElementWaitState.PRESENT_OF_ALL, ElementWaitState.VISIBLE_OF_ALL, ElementWaitState.VISIBLE_OF_ANY
		/// </param>
		/// <param name="throwException">The boolean to throw exception or not. Defaulted to true.</param>
		/// <param name="timeOut">The timeout to find the element. If not set, timeout defaults to 30 seconds.</param>
		/// <returns>Returns the web element based the specified pattern or 'None' if the element is not found</returns>
		public static IList<IWebElement> GetElements(string pattern, ElementWaitState waitState = ElementWaitState.PRESENT_OF_ALL, bool throwException = true, int timeOut = 0)
		{
			return (IList<IWebElement>)AbstractGetElement(ElementType.MULTIPLE, pattern, waitState, throwException, timeOut);
		}

		public static int GetCountOfElements(string pattern, ElementWaitState waitState = ElementWaitState.PRESENT_OF_ALL, bool throwException = true, int timeOut = 0)
		{
			return GetElements(pattern, waitState, throwException, timeOut).Count();
		}

		/// <summary>
		/// Finds element within this element's children based on the pattern specified.
		/// </summary>
		/// <param name="parentElement">A known web element from which the child element can be found.</param>
		/// <param name="pattern">The string pattern used to find the element</param>
		/// <param name="throwException">The boolean to throw exception or not. Defaulted to true.</param>
		/// <returns>Returns the element within element's children based on the pattern specified</returns>
		public static IWebElement GetElementFromElement(IWebElement parentElement, string pattern, bool throwException = true)
		{
			IWebElement childElement = null;

			try
			{
				By byLocator = GetByLocator(pattern);
				childElement = parentElement.FindElement(byLocator);
			}
			catch (NoSuchElementException noEx)
			{
				if (throwException)
					throw new NoSuchElementException($"Unable to locate the element on the DOM. Error : {noEx}");
			}
			return childElement;
		}

		/// <summary>
		/// Finds a list of elements within this element's children by the specfied pattern.
		/// Will return a list of webelements if found, or an empty list if not.
		/// </summary>
		/// <param name="parentElement">A known web element from which the child element can be found.</param>
		/// <param name="pattern">The string pattern used to find the element</param>
		/// <param name="throwException">The boolean to throw exception or not. Defaulted to true.</param>
		/// <returns>Will return a list of webelements if found, or an empty list if not.</returns>
		public static IList<IWebElement> GetElementsFromElement(IWebElement parentElement, string pattern, bool throwException = true)
		{
			IList<IWebElement> childElements = null;

			try
			{
				By byLocator = GetByLocator(pattern);
				childElements = parentElement.FindElements(byLocator);
			}
			catch (NoSuchElementException noEx)
			{
				if (throwException)
					throw new NoSuchElementException($"Unable to locate the element on the DOM. Error : {noEx}");
			}
			return childElements;
		}

		/// <summary>
		/// Simulates typing into the given element.
		/// </summary>
		/// <param name="locator">The string pattern to find the element</param>
		/// <param name="text">A string for typing, or setting form fields. 
		/// For setting file inputs, provide a local file path.</param>
		/// <param name="waitstate">The wait state for element retrial. Choose state from ElementWaitState class.
		/// Defaults to ElementWaitState.PRESENT.</param>
		/// <param name="timeOut">The timeout to find the element.
		/// If not set, timeout defaults to 30 seconds.</param>
		/// <returns></returns>
		public static void SetText(string locator, string text, ElementWaitState waitstate = ElementWaitState.PRESENT, int timeOut = 0)
		{
			try
			{
				IWebElement element = GetElement(locator, waitstate, true, timeOut);
				Clear(locator, waitstate, timeOut);
				element.SendKeys(text);
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to set the text {text} on the element. Error : {ex}");
			}
		}

		/// <summary>
		/// Send keys by character to element in DOM.
		/// </summary>
		/// <param name="locator">The string pattern to find the element</param>
		/// <param name="text">A string for typing, or setting form fields. </param>
		/// <param name="waitState">The wait state for element retrial. Choose state from ElementWaitState class.
		/// Defaults to ElementWaitState.PRESENT.</param>
		/// <param name="timeOut">The timeout to find the element. 
		/// If not set, timeout defaults to 30 seconds.</param>
		/// <returns></returns>
		public static void SetTextAsChar(string locator, string text, ElementWaitState waitstate = ElementWaitState.PRESENT, int timeOut = 0)
		{
			try
			{
				char[] textChars = text.ToCharArray();
				IWebElement element = GetElement(locator, waitstate, true, timeOut);
				Clear(locator, waitstate, timeOut);
				for (int i = 0; i < textChars.Length; i++)
				{
					element.SendKeys(textChars[i].ToString());
					new WebDriverWait(Driver, TimeSpan.FromSeconds(2))
						.Until(isCharTyped => element.GetAttribute("value").Equals(text.Substring(0, i + 1)));
				}
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to set the text {text} on the element. Error : {ex}");
			}
		}

		/// <summary>
		/// Gets the text of the element.
		/// </summary>
		/// <param name="locator">The string pattern to find the element</param>
		/// <param name="waitstate">The wait state for element retrial. Choose state from ElementWaitState class.
		/// Defaults to ElementWaitState.PRESENT.</param>
		/// <param name="timeOut">The timeout to find the element.
		/// If not set, timeout defaults to 30 seconds.</param>
		/// <returns>Returns the text of the element</returns>
		public static string GetText(string locator, ElementWaitState waitstate = ElementWaitState.PRESENT, int timeOut = 0)
		{
			try
			{
				return GetElement(locator, waitstate, true, timeOut).Text;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to get the text on the element. Error : {ex}");
			}
		}

		/// <summary>
		/// Clicks an element.
		/// </summary>		
		/// <param name="locator">The string pattern to find the element or the web element itself. 
		/// If not set, clicks on current mouse position.</param>
		/// <param name="click_type">Available types ClickType.DIRECT_API_CLICK, ClickType.JAVASCRIPT_CLICK,  ClickType.MOUSE_CLICK</param>
		/// <param name="waitState">The wait state for element retrial. Choose state from ElementWaitState class.
		/// Defaults to ElementWaitState.PRESENT.</param>
		/// <param name="timeOut">The timeout to find the element. If not set, timeout defaults to 30 seconds.</param>
		/// <returns></returns>
		public static void Click<T>(T locator = default(T), ClickType clickType = ClickType.DIRECT_API_CLICK, ElementWaitState waitState = ElementWaitState.PRESENT, int timeOut = 0)
		{
			IWebElement element = null;

			try
			{
				if ((locator == null && clickType == ClickType.DIRECT_API_CLICK) || (locator == null && clickType == ClickType.JAVASCRIPT_CLICK))
					throw new InvalidOperationException("Please provide the string pattern for click types - ClickType.DIRECT_API_CLICK and ClickType.JAVASCRIPT_CLICK");

				if (locator != null)
				{
					if (typeof(T) == typeof(IWebElement))
						element = (IWebElement)locator;
					else if ((typeof(T) == typeof(string)) || (typeof(T) == typeof(String)))
						element = GetElement(locator.ToString(), waitState, true, timeOut);
					else
						throw new InvalidOperationException("Please provide the string pattern for locator or webelement.");
				}

				if (element != null)
				{
					switch (clickType)
					{
						case ClickType.DIRECT_API_CLICK:
							element.Click();
							break;
						case ClickType.JAVASCRIPT_CLICK:
							ExecuteJavascript("arguments[0].click();", element);
							break;
						case ClickType.MOUSE_CLICK:
							new Actions(Driver).Click(element).Perform();
							break;
						default:
							throw new InvalidOperationException($"{clickType} must be an instance of ClickType");
					}
				}
			}
			catch (ArgumentException)
			{
				throw new ArgumentException("String pattern is None. Please provide a valid string pattern for click types ClickType.DIRECT_API_CLICK and ClickType.JAVASCRIPT_CLICK");
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to click on the element. Error: {ex}");
			}
		}

		/// <summary>
		/// Double-clicks an element.
		/// </summary>
		/// <param name="locator">The string pattern to find the element or the web element itself. 
		/// If not set, clicks on current mouse position.</param>
		/// <param name="waitState">The wait state for element retrial. Choose state from ElementWaitState class. 
		/// Defaults to ElementWaitState.PRESENT.</param>
		/// <param name="timeOut">The timeout to find the element. If not set, timeout defaults to 30 seconds.</param>
		/// <returns></returns>
		public static void DoubleClick<T>(T locator = default(T), ElementWaitState waitState = ElementWaitState.PRESENT, int timeOut = 0)
		{
			IWebElement element = null;

			try
			{
				if (typeof(T) == typeof(IWebElement))
					element = (IWebElement)locator;
				else if ((typeof(T) == typeof(string)) || (typeof(T) == typeof(String)))
					element = GetElement(locator.ToString(), waitState, true, timeOut);
				else
					throw new InvalidOperationException("Please provide the string pattern for locator or webelement.");

				new Actions(Driver).DoubleClick(element).Perform();
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to double click on element. Error {ex}");
			}
		}

		/// <summary>
		/// Performs a right-click (context click) on an element.
		/// </summary>
		/// <param name="locator">The string pattern to find the element or the web element itself. 
		/// If not set, clicks on current mouse position.</param>
		/// <param name="waitState">The wait state for element retrial. Choose state from ElementWaitState class. 
		/// Defaults to ElementWaitState.PRESENT.</param>
		/// <param name="timeOut">The timeout to find the element. If not set, timeout defaults to 30 seconds.</param>
		/// <returns></returns>
		public static void RightClick<T>(T locator = default(T), ElementWaitState waitState = ElementWaitState.PRESENT, int timeOut = 0)
		{
			IWebElement element = null;

			try
			{
				if (typeof(T) == typeof(IWebElement))
					element = (IWebElement)locator;
				else if ((typeof(T) == typeof(string)) || (typeof(T) == typeof(String)))
					element = GetElement(locator.ToString(), waitState, true, timeOut);
				else
					throw new InvalidOperationException("Please provide the string pattern for locator or webelement.");

				new Actions(Driver).ContextClick(element).Perform();
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to right click on element. Error {ex}");
			}
		}

		/// <summary>
		/// Select/Unselect the check box.
		/// </summary>
		/// <param name="locator">The string pattern to find the element</param>
		/// <param name="isSelect">The boolean to perform select/unselect</param>
		/// <param name="waitstate">The wait state for element retrial. Choose state from ElementWaitState class.
		/// Defaults to ElementWaitState.PRESENT.</param>
		/// <param name="timeOut">The timeout to find the element. If not set, timeout defaults to 30 seconds.</param>
		/// <returns></returns>
		public static void SelectCheckBox(string locator, bool isSelect, ElementWaitState waitstate = ElementWaitState.PRESENT, int timeOut = 0)
		{
			try
			{
				IWebElement element = GetElement(locator, waitstate, true, timeOut);

				if ((!element.Selected) && (isSelect))
					element.Click();
				else if ((!element.Selected) && (!isSelect))
					element.Click();
				else if ((element.Selected) && (!isSelect))
					element.Click();
				else if ((element.Selected) && (isSelect))
					element.Click();
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to set the text Error : {ex}");
			}
		}

		/// <summary>
		/// Select an option or list of options on the drop down.
		/// Examples:
		/// SelectDropdown ('#dropdown', SelectStrategy.VALUE, True, 'foo')
		/// SelectDropdown ('#dropdown', SelectStrategy.VALUE, True, 'foo', 'foo1')
		/// SelectDropdown ('#dropdown', SelectStrategy.INDEX, True, 2)
		/// SelectDropdown ('#dropdown', SelectStrategy.INDEX, True, 1, 2, 3)
		/// SelectDropdown ('#dropdown', SelectStrategy.VISIBLE_TEXT, True, 'Chennai')
		/// SelectDropdown ('#dropdown', SelectStrategy.VISIBLE_TEXT, False, 'Chennai', 'Delhi')
		/// </summary>		
		/// <param name="locator">The string pattern to find the element</param>
		/// <param name="strategy">The allowed values are SelectStrategy.VISIBLE_TEXT, SelectStrategy.VALUE,
		/// SelectStrategy.INDEX </param>
		/// <param name="isSelect">The boolean to select/deselect items.
		/// Choose True for single select dropdown.
		/// For multi-select dropdown, choose 'True' to select items, 'False' to unselect items. </param>
		/// <param name="waitstate">The wait state for element retrial. Choose state from ElementWaitState class.
		/// Defaults to ElementWaitState.PRESENT.</param>
		/// <param name="timeOut">The timeout to find the element. If not set, timeout defaults to 30 seconds.</param>
		/// <param name="values">A single value (for single select dropdown) or multiple values (multi-select dropdown) to match against.
		/// </param>
		/// <returns></returns>
		public static void SelectDropdown(string locator, SelectStrategy strategy, bool isSelect, ElementWaitState waitstate = ElementWaitState.PRESENT, int timeOut = 0, params object[] values)
		{
			IWebElement element = null;

			try
			{
				element = GetElement(locator, waitstate, true, timeOut);
				SelectElement select = new SelectElement(element);

				if (strategy == SelectStrategy.VISIBLE_TEXT)
					SetVisibleText(select, isSelect, values.Cast<string>().ToArray());
				if (strategy == SelectStrategy.VALUE)
					SetValue(select, isSelect, values.Cast<string>().ToArray());
				if (strategy == SelectStrategy.INDEX)
					SetIndex(select, isSelect, values.Cast<int>().ToArray());
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to select/deselect the item {(values)} on the dropdown element. Error : {ex}");
			}
		}

		/// <summary>
		/// Returns a list of all options belonging to the dropdown.
		/// </summary>
		/// <param name="locator">The string pattern to find the element</param>
		/// <param name="waitstate">The wait state for element retrial. Choose state from ElementWaitState class.
		/// Defaults to ElementWaitState.PRESENT.</param>
		/// <param name="timeOut">The timeout to find the element. If not set, timeout defaults to 30 seconds.</param>
		/// <returns>Returns a list of all options belonging to the dropdown</returns>
		public static IList<IWebElement> GetAllOptionsDropdown(string locator, ElementWaitState waitstate = ElementWaitState.PRESENT, int timeOut = 0)
		{
			try
			{
				IWebElement element = GetElement(locator, waitstate, true, timeOut);
				IList<IWebElement> options = new SelectElement(element).Options;
				return options;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to get all the options from the dropdown element. Error : {ex}");
			}
		}

		/// <summary>
		/// Returns the text from the first selected option on the dropdown.
		/// </summary>
		/// <param name="locator">The string pattern to find the element</param>
		/// <param name="waitstate">The wait state for element retrial. Choose state from ElementWaitState class.
		/// Defaults to ElementWaitState.PRESENT.</param>
		/// <param name="timeOut">The timeout to find the element. If not set, timeout defaults to 30 seconds.</param>
		/// <returns>Returns the text from the first selected option on the dropdown.</returns>
		public static String GetOptionDropdown(string locator, ElementWaitState waitstate = ElementWaitState.PRESENT, int timeOut = 0)
		{
			try
			{
				IWebElement element = GetElement(locator, waitstate, true, timeOut);
				return new SelectElement(element).SelectedOption.Text;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to get the selected option from the dropdown element. Error : {ex}");
			}
		}

		/// <summary>
		/// Clear all selected entries. This is only valid when the SELECT supports multiple selections.
		/// </summary>
		/// <param name="locator">The string pattern to find the element</param>
		/// <param name="waitstate">The wait state for element retrial. Choose state from ElementWaitState class.
		/// Defaults to ElementWaitState.PRESENT.</param>
		/// <param name="timeOut">The timeout to find the element. If not set, timeout defaults to 30 seconds.</param>
		/// <returns></returns>
		public static void DeselectAllOptionsDropdown(string locator, ElementWaitState waitstate = ElementWaitState.PRESENT, int timeOut = 0)
		{
			try
			{
				IWebElement element = GetElement(locator, waitstate, true, timeOut);
				new SelectElement(element).DeselectAll();
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to deselect all options from the dropdown element. Error : {ex}");
			}
		}

		/// <summary>
		/// Submits a form.
		/// </summary>
		/// <param name="locator">The string pattern to find the element</param>
		/// <param name="waitstate">The wait state for element retrial. Choose state from ElementWaitState class.
		/// Defaults to ElementWaitState.PRESENT.</param>
		/// <param name="timeOut">The timeout to find the element. If not set, timeout defaults to 30 seconds.</param>
		/// <returns></returns>
		public static void Submit(string locator, ElementWaitState waitstate = ElementWaitState.PRESENT, int timeOut = 0)
		{
			try
			{
				IWebElement element = GetElement(locator, waitstate, true, timeOut);
				element.Submit();
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to submit the form from the element. Error : {ex}");
			}
		}

		/// <summary>
		/// Clears the text if it's a text entry element.
		/// </summary>
		/// <param name="locator">The string pattern to find the element</param>
		/// <param name="waitstate">The wait state for element retrial. Choose state from ElementWaitState class.
		/// Defaults to ElementWaitState.PRESENT.</param>
		/// <param name="timeOut">The timeout to find the element. If not set, timeout defaults to 30 seconds.</param>
		/// <returns></returns>
		public static void Clear(string locator, ElementWaitState waitstate = ElementWaitState.PRESENT, int timeOut = 0)
		{
			try
			{
				IWebElement element = GetElement(locator, waitstate, true, timeOut);
				element.Clear();
				if (!String.IsNullOrEmpty(GetAttribute(locator, "value")))
				{
					element.SendKeys(Keys.Control + "a");
					element.SendKeys(Keys.Delete);
				}
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to clear the text on the element. Error : {ex}");
			}
		}

		/// <summary>
		/// Gets the value of the given attribute of the element.
		/// </summary>
		/// <param name="locator">The string pattern to find the element</param>
		/// <param name="name">Name of the attribute/property to retrieve.</param>
		/// <param name="waitstate">The wait state for element retrial. Choose state from ElementWaitState class.
		/// Defaults to ElementWaitState.PRESENT.</param>
		/// <param name="timeOut">The timeout to find the element. If not set, timeout defaults to 30 seconds.</param>
		/// <returns>Gets the value of the given attribute of the element.</returns>
		public static string GetAttribute(string locator, string name, ElementWaitState waitstate = ElementWaitState.PRESENT, int timeOut = 0)
		{
			try
			{
				IWebElement element = GetElement(locator, waitstate, true, timeOut);
				return element.GetAttribute(name);
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to get attribute value for the attribute. Error : {ex}");
			}
		}

		/// <summary>
		/// Gets the value of a given javascript property of the element.
		/// </summary>
		/// <param name="locator">The string pattern to find the element</param>
		/// <param name="name">Name of the attribute/property to retrieve.</param>
		/// <param name="waitstate">The wait state for element retrial. Choose state from ElementWaitState class.
		/// Defaults to ElementWaitState.PRESENT.</param>
		/// <param name="timeOut">The timeout to find the element. If not set, timeout defaults to 30 seconds.</param>
		/// <returns>Gets the value of a given javascript property of the element.</returns>
		//public static string GetProperty(string locator, string name, ElementWaitState waitstate = ElementWaitState.PRESENT, int timeOut = 0)
		//{
		//	try
		//	{
		//		IWebElement element = GetElement(locator, waitstate, true, timeOut);
		//		return element.GetProperty(name);
		//	}
		//	catch (Exception ex)
		//	{
		//		throw new InvalidOperationException($"Unable to get property value for {name} on the element. Error : {ex}");
		//	}
		//}

		/// <summary>
		/// Gets the size of an element.
		/// </summary>
		/// <param name="locator">The string pattern to find the element</param>
		/// <param name="waitstate">The wait state for element retrial. Choose state from ElementWaitState class.
		/// Defaults to ElementWaitState.PRESENT.</param>
		/// <param name="timeOut">The timeout to find the element. If not set, timeout defaults to 30 seconds.</param>
		/// <returns>Gets the size of an element.</returns>
		public static Size GetSize(string locator, ElementWaitState waitstate = ElementWaitState.PRESENT, int timeOut = 0)
		{
			try
			{
				IWebElement element = GetElement(locator, waitstate, true, timeOut);
				return element.Size;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to get size on the element. Error : {ex}");
			}
		}

		/// <summary>
		/// Gets the location of the element in the renderable canvas.
		/// </summary>
		/// <param name="locator">The string pattern to find the element</param>
		/// <param name="waitstate">The wait state for element retrial. Choose state from ElementWaitState class.
		/// Defaults to ElementWaitState.PRESENT.</param>
		/// <param name="timeOut">The timeout to find the element. If not set, timeout defaults to 30 seconds.</param>
		/// <returns>Gets the location of an element.</returns>
		public static Point GetLocation(string locator, ElementWaitState waitstate = ElementWaitState.PRESENT, int timeOut = 0)
		{
			try
			{
				IWebElement element = GetElement(locator, waitstate, true, timeOut);
				return element.Location;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to get location on the element. Error : {ex}");
			}
		}

		/// <summary>
		/// Synchronously executes JavaScript in the current window or frame.
		/// </summary>
		/// <param name="script">The JavaScript to execute</param>
		/// <param name="args">Any applicable arguments for your JavaScript</param>
		/// <returns>Returns the object of the interaction with javaScript in the current window or frame.</returns>
		public static object ExecuteJavascript(string script, params object[] args)
		{
			try
			{
				IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
				return js.ExecuteScript(script, args);
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to execute javascript {script} on the argument(s) {args}. Error : {ex}");
			}
		}

		/// <summary>
		/// Asynchronously executes JavaScript in the current window or frame.
		/// </summary>
		/// <param name="script">The JavaScript to execute</param>
		/// <param name="args">Any applicable arguments for your JavaScript</param>
		/// <returns>Returns the object of the interaction with javaScript in the current window or frame.</returns>
		public static object ExecuteAsyncJavascript(string script, params object[] args)
		{
			try
			{
				IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
				return js.ExecuteAsyncScript(script, args);
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to execute asynchronous javascript {script} on the argument(s) {args}. Error : {ex}");
			}
		}

		/// <summary>
		/// Moving the mouse to the middle of an element.
		/// </summary>
		/// <param name="locator">The string pattern to find the element</param>
		/// <param name="waitstate">The wait state for element retrial. Choose state from ElementWaitState class.
		/// Defaults to ElementWaitState.PRESENT.</param>
		/// <param name="timeOut">The timeout to find the element. If not set, timeout defaults to 30 seconds.</param>
		/// <returns></returns>
		public static void MoveCursorToElement(string locator, ElementWaitState waitstate = ElementWaitState.PRESENT, int timeOut = 0)
		{
			IWebElement element = null;

			try
			{
				element = GetElement(locator, waitstate, true, timeOut);
				new Actions(Driver).MoveToElement(element).Perform();
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to move to the element. Error : {ex}");
			}
		}

		/// <summary>
		/// Move the mouse by an offset of the specified element. Offsets are relative to the top-left corner of the element.
		/// </summary>
		/// <param name="locator">The string pattern to find the element</param>
		/// <param name="xOffSet">X offset to move to.</param>
		/// <param name="yOffSet">Y offset to move to.</param>
		/// <param name="waitstate">The wait state for element retrial. Choose state from ElementWaitState class.
		/// Defaults to ElementWaitState.PRESENT.</param>
		/// <param name="timeOut">The timeout to find the element. If not set, timeout defaults to 30 seconds.</param>
		/// <returns></returns>
		public static void MoveCursorToElementByOffset(string locator, int xOffSet, int yOffSet, ElementWaitState waitstate = ElementWaitState.PRESENT, int timeOut = 0)
		{
			IWebElement element = null;

			try
			{
				element = GetElement(locator, waitstate, true, timeOut);
				new Actions(Driver).MoveToElement(element, xOffSet, yOffSet).Perform();
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to move to the element. Error : {ex}");
			}
		}

		/// <summary>
		/// Moving the mouse to an offset from current mouse position.
		/// </summary>
		/// <param name="xOffSet">X offset to move to, as a positive or negative integer.</param>
		/// <param name="yOffSet">Y offset to move to, as a positive or negative integer.</param>
		/// <returns></returns>
		public static void MoveCursorByOffset(string locator, int xOffSet, int yOffSet, ElementWaitState waitstate = ElementWaitState.PRESENT, int timeOut = 0)
		{
			try
			{
				GetElement(locator, waitstate, true, timeOut);
				new Actions(Driver).MoveByOffset(xOffSet, yOffSet).Perform();
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to move by offset {xOffSet}, {yOffSet}. Error : {ex}");
			}
		}

		/// <summary>
		/// Holds down the left mouse button on the source element, 
		/// then moves to the target element and releases the mouse button.
		/// </summary>
		/// <param name="source">The source string pattern used to find the element</param>
		/// <param name="target">The target string pattern used to find the element</param>
		/// <param name="waitstate">The wait state for element retrial. Choose state from ElementWaitState class.
		/// Defaults to ElementWaitState.PRESENT.</param>
		/// <param name="timeOut">The timeout to find the element. If not set, timeout defaults to 30 seconds.</param>
		/// <returns></returns>
		public static void DragAndDrop(string source, string target, ElementWaitState waitstate = ElementWaitState.PRESENT, int timeOut = 0)
		{
			try
			{
				IWebElement srcElement = GetElement(source, waitstate, true, timeOut);
				IWebElement trgElement = GetElement(target, waitstate, true, timeOut);
				new Actions(Driver).DragAndDrop(srcElement, trgElement).Perform();
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to drag and drop on elements. Error : {ex}");
			}
		}

		/// <summary>
		///  Holds down the left mouse button on the source element, 
		///  then moves to the target offset and releases the mouse button.
		/// </summary>
		/// <param name="srcLocator">The source string pattern used to find the element</param>
		/// <param name="xOffset">X offset to move to.</param>
		/// <param name="yOffset">Y offset to move to.</param>
		/// <param name="waitstate">The wait state for element retrial. Choose state from ElementWaitState class.
		/// Defaults to ElementWaitState.PRESENT.</param>
		/// <param name="timeOut">The timeout to find the element. If not set, timeout defaults to 30 seconds.</param>
		/// <returns></returns>
		public static void DragAndDropByOffset(string srcLocator, int xOffset, int yOffset, ElementWaitState waitstate = ElementWaitState.PRESENT, int timeOut = 0)
		{
			try
			{
				IWebElement srcElement = GetElement(srcLocator, waitstate, true, timeOut);
				new Actions(Driver).DragAndDropToOffset(srcElement, xOffset, yOffset).Perform();
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to move by a offset {xOffset}, {yOffset} from the source Element. Error : {ex}");
			}
		}

		/// <summary>
		/// Returns whether the element is present on the DOM or not.
		/// </summary>		
		/// <param name="locator">The string pattern to find the element.</param>
		/// <param name="timeOut">The timeout to find the element. If not set, timeout defaults to 30 seconds.</param>
		/// <returns>Returns true if the element is present in DOM otherwise false.</returns>
		public static bool IsElementPresent(string locator, int timeOut = 0)
		{
			bool isElementPresent = false;
			IWebElement element = GetElement(locator, ElementWaitState.PRESENT, false, timeOut);
			if (element != null)
				isElementPresent = true;
			return isElementPresent;
		}

		/// <summary>
		/// Returns whether the element is present on the DOM or not.
		/// </summary>		
		/// <param name="locator">The string pattern to find the element.</param>
		/// <param name="timeOut">The timeout to find the element. If not set, timeout defaults to 30 seconds.</param>
		/// <returns>Returns true if the element is present in DOM otherwise false.</returns>
		public static bool IsElementNotPresent(string locator, int timeOut = 10)
		{
			bool isElementNotPresent = false;
			IWebElement element = GetElement(locator, ElementWaitState.PRESENT, false, timeOut);
			if (element == null)
				isElementNotPresent = true;
			return isElementNotPresent;
		}

		/// <summary>
		/// Returns whether the element is visible or not.
		/// </summary>		
		/// <param name="locator">The string pattern to find the element.</param>
		/// <param name="timeOut">The timeout to find the element. If not set, timeout defaults to 30 seconds.</param>
		/// <returns>Returns true if the element is present in DOM otherwise false.</returns>
		public static bool IsElementVisible(string locator, int timeOut = 0)
		{
			bool isElementVisible = false;
			IWebElement element = GetElement(locator, ElementWaitState.VISIBLE, false, timeOut);
			if (element != null)
				isElementVisible = element.Displayed;
			return isElementVisible;
		}

		/// <summary>
		/// Returns whether the element is enabled or not.
		/// </summary>		
		/// <param name="locator">The string pattern to find the element.</param>
		/// <param name="timeOut">The timeout to find the element. If not set, timeout defaults to 30 seconds.</param>
		/// <returns>Returns true if the element is present in DOM otherwise false.</returns>
		public static bool IsElementEnabled(string locator, int timeOut = 0)
		{
			bool isElementEnabled = false;
			IWebElement element = GetElement(locator, ElementWaitState.PRESENT, false, timeOut);
			if (element != null)
				isElementEnabled = element.Enabled;
			return isElementEnabled;
		}

		/// <summary>
		/// Returns whether the element is selected or not.
		/// </summary>		
		/// <param name="locator">The string pattern to find the element.</param>
		/// <param name="timeOut">The timeout to find the element. If not set, timeout defaults to 30 seconds.</param>
		/// <returns>Returns true if the element is present in DOM otherwise false.</returns>
		public static bool IsElementSelected(string locator, int timeOut = 0)
		{
			bool isElementSelected = false;
			IWebElement element = GetElement(locator, ElementWaitState.PRESENT, false, timeOut);
			if (element != null)
				isElementSelected = element.Selected;
			return isElementSelected;
		}

		/// <summary>
		/// Switch focus to default frame/window
		/// </summary>	
		/// <returns></returns>
		public static void SwitchToDefaultContent()
		{
			try
			{
				Driver.SwitchTo().DefaultContent();
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to switch to Default content, Error : {ex}");
			}
		}

		/// <summary>
		/// Switch focus to specified frame
		/// <param name="frameReference">The "Locator" or "Index" of the frame to switch to.</param>
		/// </summary>	
		/// <returns></returns>
		public static void SwitchToFrame<T>(T frameReference)
		{
			try
			{
				IWebElement element = null;

				if (typeof(T) == typeof(string))
				{
					element = GetElement(frameReference.ToString(), ElementWaitState.PRESENT, true, 60);
					Driver.SwitchTo().Frame(element);
				}
				else if (typeof(T) == typeof(int))
				{
					int indexValue = Convert.ToInt32(frameReference);
					Driver.SwitchTo().Frame(indexValue);
				}
				else
					throw new InvalidOperationException("Please provide the string pattern for locator or webelement.");
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to switch to frame, Error : {ex}");
			}
		}

		/// <summary>
		/// Switch focus to parent frame
		/// </summary>	
		/// <returns></returns>
		public static void SwitchToParentFrame()
		{
			try
			{
				Driver.SwitchTo().ParentFrame();
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to switch to parent frame, Error : {ex}");
			}
		}

		/// <summary>
		/// Returns page title of child window and close the child window
		/// </summary>	
		public static string SwitchToChildWindow()
		{
			try
			{
				string parentWindow = GetCurrentWindowHandle();
				string pageTitle = string.Empty;
				IReadOnlyCollection<string> winHandleBefore = GetWindowHandles();
				Driver.SwitchTo().Window(winHandleBefore.ElementAt(1));
				pageTitle = GetTitle();
				//foreach (string win in GetWindowHandles())
				//{
				//	Driver.SwitchTo().Window(winHandleBefore.ElementAt(1));
				//	Driver.Manage().Window.Maximize();
				//	pageTitle = GetTitle();
				//}
				//Driver.Close();
				//Driver.SwitchTo().Window(parentWindow);
				return pageTitle;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Unable to switch to child window, Error : {ex}");
			}
		}

		private static void SetVisibleText(SelectElement select, bool isSelect, params string[] values)
		{
			foreach (string value in values)
			{
				if (isSelect)
					select.SelectByText(value);
				else
					select.DeselectByText(value);
			}
		}

		private static void SetValue(SelectElement select, bool isSelect, params string[] values)
		{
			foreach (string value in values)
			{
				if (isSelect)
					select.SelectByValue(value);
				else
					select.DeselectByValue(value);
			}
		}

		private static void SetIndex(SelectElement select, bool isSelect, params int[] values)
		{
			foreach (int value in values)
			{
				if (isSelect)
					select.SelectByIndex(value);
				else
					select.DeselectByIndex(value);
			}
		}

		private static By GetByLocator(string pattern)
		{
			By byLocator = null;

			if (Regex.Match(pattern, idRegex).Success)
				byLocator = By.Id(Regex.Match(pattern, idRegex).Groups[2].ToString());
			else if (Regex.Match(pattern, xpathRegex).Success)
				byLocator = By.XPath(Regex.Match(pattern, xpathRegex).Value);
			else if (Regex.Match(pattern, classRegex).Success)
				byLocator = By.ClassName(Regex.Match(pattern, classRegex).Groups[2].ToString());
			else if (Regex.Match(pattern, partialLinkTextRegex).Success)
				byLocator = By.PartialLinkText(Regex.Match(pattern, partialLinkTextRegex).Groups[2].ToString());
			else if (Regex.Match(pattern, linkTextRegex).Success)
				byLocator = By.LinkText(Regex.Match(pattern, linkTextRegex).Groups[2].ToString());
			else if (Regex.Match(pattern, tagRegex).Success)
				byLocator = By.TagName(Regex.Match(pattern, tagRegex).Groups[2].ToString());
			else if (Regex.Match(pattern, cssRegex).Success)
				byLocator = By.CssSelector(Regex.Match(pattern, cssRegex).Groups[2].ToString());
			else if (Regex.Match(pattern, nameRegex).Success)
				byLocator = By.Name(Regex.Match(pattern, nameRegex).Groups[2].ToString());
			return byLocator;
		}

		private static object AbstractGetElement(ElementType elementType, string pattern, ElementWaitState waitState, bool throwException, int timeOut)
		{
			object webElement = null;
			int newTimeOut = 0;
			newTimeOut = timeOut == 0 ? defaultTimeOut : timeOut;

			try
			{
				By byLocator = GetByLocator(pattern);
				if (elementType == ElementType.SINGLE)
					webElement = GetSingleElement(waitState, byLocator, newTimeOut);
				else if (elementType == ElementType.MULTIPLE)
					webElement = GetMultipleElements(waitState, byLocator, newTimeOut);
				else
					throw new InvalidOperationException($"{elementType} must be an instance of Elementtype enum");
			}
			catch (StaleElementReferenceException staleEx)
			{
				if (throwException)
					throw new StaleElementReferenceException($"Unable to locate the element as it is either destroyed or no longer attached to the DOM. Error: {staleEx}");
			}
			catch (TimeoutException timeoutEx)
			{
				if (throwException)
					throw new TimeoutException($"Timed out after {timeoutEx} seconds waiting for the element {pattern} with state {waitState}. Error : {timeoutEx}");
			}
			catch (WebDriverException webEx)
			{
				if (throwException)
					throw new TimeoutException($"An error occurred identifying the element {pattern} on the web page. Error : {webEx}");
			}
			catch (InvalidOperationException invalidOpEx)
			{
				if (throwException)
					throw new InvalidOperationException($"{waitState} must be an instance of ElementType enum. Error : {invalidOpEx}");
			}
			catch (Exception ex)
			{
				if (throwException)
					throw new InvalidOperationException($"Unhandled exception. Error : {ex}");
			}
			return webElement;
		}

		private static IWebElement GetSingleElement(ElementWaitState waitState, By byLocator, int timeOut)
		{
			IWebElement webElement = null;
			TimeSpan timeOutSpan = TimeSpan.FromSeconds(timeOut);

			switch (waitState)
			{
				case ElementWaitState.PRESENT:
					webElement = new WebDriverWait(Driver, timeOutSpan).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(byLocator));
					break;
				case ElementWaitState.VISIBLE:
					webElement = new WebDriverWait(Driver, timeOutSpan).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(byLocator));
					break;
				case ElementWaitState.CLICKABLE:
					webElement = new WebDriverWait(Driver, timeOutSpan).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(byLocator));
					break;
				default:
					throw new InvalidOperationException($"Invalid wait state {waitState} to get single web element. Please choose an appropriate option!");
			}
			return webElement;
		}

		private static IList<IWebElement> GetMultipleElements(ElementWaitState waitState, By byLocator, int timeOut)
		{
			IList<IWebElement> webElements = null;
			TimeSpan timeOutSpan = TimeSpan.FromSeconds(timeOut);

			switch (waitState)
			{
				case ElementWaitState.PRESENT_OF_ALL:
					webElements = new WebDriverWait(Driver, timeOutSpan).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.PresenceOfAllElementsLocatedBy(byLocator));
					break;
				case ElementWaitState.VISIBLE_OF_ALL:
					webElements = new WebDriverWait(Driver, timeOutSpan).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.VisibilityOfAllElementsLocatedBy(byLocator));
					break;
				default:
					throw new InvalidOperationException($"Invalid wait state {waitState} to get multiple web elements. Please choose an appropriate option!");
			}
			return webElements;
		}		

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]

		private static extern bool SetForegroundWindow(IntPtr hWnd);
		[DllImport("user32.dll", EntryPoint = "FindWindow")]

		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		//public static void UploadFile(string fileName)
		//{
		//	Thread.Sleep(1000);
		//	var dialogHWnd = FindWindow(null, "Open Files");
		//	var setFocus = SetForegroundWindow(dialogHWnd);
		//	if (setFocus)
		//	{
		//		Thread.Sleep(500);
		//		SendKeys.SendWait(fileName);
		//		SendKeys.SendWait("{ENTER}");
		//	}
		//}
		/// <summary>
		/// Returns string of children elements from a given parent locator.
		///</summary>
		/// <param name="locator">The string pattern to find the element.</param>
		/// <param name="timeOut">The timeout to find the element. If not set, timeout defaults to 30 seconds.</param>
		/// <returns>Returns List of string if parent locator has children.</returns>
		public static List<string> GetTextOfElements(string locator, ElementWaitState waitState = ElementWaitState.PRESENT_OF_ALL, bool throwException = true, int timeOut = 0)
		{
			try
			{
				List<string> storeElements = new List<string>();
				IList<IWebElement> getElements = GetElements(locator, ElementWaitState.PRESENT_OF_ALL, true, timeOut);
				for (int i = 1; i <= getElements.Count; i++)
				{
					string elementBasedOnIndex = locator + "[" + i + "]";
					string readText = BasePage.GetElement(elementBasedOnIndex, ElementWaitState.PRESENT, true, timeOut).Text;
					storeElements.Add(readText);
				}
				return storeElements;
			}
			catch (NoSuchElementException noExc)
			{
				throw new NoSuchElementException($"Unable to locate the element on the DOM. Error : {noExc}");
			}
		}

		/// <summary>
		/// Waits until the element is not present in DOM.
		/// </summary>		
		/// <param name="locator">The string pattern to find the element.</param>
		/// <param name="timeOut">The timeout to find the element. If not set, timeout defaults to 60 seconds.</param>
		/// <returns></returns>
		public static void WaitUntilElementIsInvisible(string locator, int timeOut = 60)
		{
			new WebDriverWait(Driver, TimeSpan.FromSeconds(timeOut))
				.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(GetByLocator(locator)));
		}

		/// <summary>
		/// Returns whether the element is in focus or not.
		/// </summary>		
		/// <param name="locator">The string pattern to find the element.</param>
		/// <param name="timeOut">The timeout to find the element. If not set, timeout defaults to 30 seconds.</param>
		/// <returns>Returns true if the element is focused in DOM otherwise false.</returns>
		public static bool GetFocusedElement(string locator, int timeout = 60)
		{
			IWebElement element;
			element = GetElement(locator.ToString(), ElementWaitState.PRESENT, true, 60);
			return element.Equals(Driver.SwitchTo().ActiveElement());
		}
	}
}