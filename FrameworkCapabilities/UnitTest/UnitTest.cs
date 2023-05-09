using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Selenium.BasePage;
using Assert = NUnit.Framework.Assert;
using OpenQA.Selenium;
using System.Drawing;

namespace FrameworkCapabilities.UnitTest
{
    [TestFixture]
    public class UnitTest
    {
        [OneTimeSetUp]
        public void OneTimeSetUp ()
        {
            string  WindowsApplicationDriverUrl="http://127.0.0.1:4723/";
            string ApplicationPath="C:\\net6.0-windows\\WpfSampleApplication.exe";
            string ApplicationDirectory="C:\\net6.0-windows";
            BasePage.InitWinAppDriver(WindowsApplicationDriverUrl, ApplicationPath, ApplicationDirectory, "MainWindow");
        }

        [Test]
        [Order(1)]
        public void GetTitleTest ()
        {
            string actual = BasePage.GetTitle();
            string expected = "MainWindow";
            Assert.AreEqual(actual, expected);
        }

        [Test]
        [Order(2)]
        public void MaximizeWindowTest ()
        {
            bool result = false;
            try
            {
                BasePage.MaximizeCurrentWindow();
                result = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to maximize the window" + ex.StackTrace);
            }
            Assert.IsTrue(result);
        }

        [Test]
        [Order(3)]
        public void ClickTest ()
        {
            string _radioBtnClick = "//*[@AutomationId='SampleRadioButton1']";
            bool result = false;
            try
            {
                BasePage.Click(_radioBtnClick);
                result = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to click on the element" + ex.StackTrace);
            }
            Assert.IsTrue(result);
        }

        [Test]
        [Order(4)]
        public void GetTextTest ()
        {
            string _sampleTextBox = "//*[@AutomationId='SampleTextBox']";
            string actual =   BasePage.GetText(_sampleTextBox);
            Assert.AreEqual("Enter Text here", actual);
        }

        [Test]
        [Order(5)]
        public void GetAttributeTest ()
        {
            bool result =  false;
            string _radioBtnClick = "//*[@AutomationId='SampleRadioButton1']";
            try
            {
                string attribute=  BasePage.GetAttribute(_radioBtnClick, "AutomationId");
                result = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to fetch the attribute" + ex.StackTrace);
            }
            Assert.IsTrue(result);
        }

        [Test]
        [Order(6)]
        public void DoubleClickTest ()
        {
            bool result = false;
            string _checkBox = "[CheckBox2]";
            try
            {
                BasePage.DoubleClick(_checkBox);
                result = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to double-click on the element" + ex.StackTrace);
            }
            Assert.IsTrue(result);
        }

        [Test]
        [Order(7)]
        public void ClearTest ()
        {
            bool result=false;
            string password = "AAA";
            string _passwordField = "//*[@AutomationId='PasswordField']";
            try
            {
                BasePage.SetText(_passwordField, password);//passed
                BasePage.Clear(_passwordField);
                result = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to clear the field" + ex.StackTrace);
            }
            Assert.IsTrue(result);
        }
        [Test]
        [Order(8)]
        public void GetSizeTest ()
        {
            string _passwordField = "//*[@AutomationId='PasswordField']";

            Size expected =new Size(120,18);
            Size actual = BasePage.GetSize(_passwordField);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Order(9)]
        public void GetElementTest ()
        {
            IWebElement element  =  BasePage.GetElement("//*[@AutomationId='SampleRadioButton1']");
            string actual = element.Text;
            string expected= "RadioButton1";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Order(10)]
        public void GetElementsTest ()
        {
            IList<IWebElement> elements =BasePage.GetElements("//*[@Name='WpfSampleApplication.MainWindow+DataAuthor']");
            Assert.AreEqual(6, elements.Count());
        }


        [Test]
        [Order(11)]
        public void GetElementFromElementTest ()
        {
            IWebElement  parentElement =BasePage.GetElement( "//*[@Name='WpfSampleApplication.MainWindow+DataAuthor']");
            IWebElement  childElement= BasePage.GetElementFromElement(parentElement,"[101]",true);
            string actual = childElement.Text;
            string expected= "101";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Order(12)]
        public void GetElementsofElementTest ()
        {
            IWebElement parentElement =BasePage.GetElement("//*[@AutomationId='SampleDataGrid']");
            IList<IWebElement> elements= BasePage.GetElementsFromElement(parentElement,"//*[@Name='WpfSampleApplication.MainWindow+DataAuthor']", true);
            int count = elements.Count();
            Assert.AreEqual(6, count);
        }

        [Test]
        [Order(13)]
        public void GetCountofElementsTest ()
        {
            int elementsCount= BasePage.GetCountOfElements("//*[@ClassName='TabControl']//*[@ClassName='TabItem']");
            Assert.AreEqual(2, elementsCount);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown ()
        {
            BasePage.Close();
        }
    }
}
