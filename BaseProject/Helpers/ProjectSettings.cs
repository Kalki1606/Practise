using FrameworkCapabilities.Utilities.FileHandlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProject.Helpers
{
	public class ProjectSettings
	{
		private const string _projectSettingsXmlFileName		  = "ProjectSettings.xml";
		private static XmlDocumentReader _projectSettingsDocument = null;

		private static XmlDocumentReader ProjectSettingsDocument
		{
			get
			{
				if (_projectSettingsDocument == null)
				{
					string currentDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
					_projectSettingsDocument = new XmlDocumentReader(currentDirectory + "/" + _projectSettingsXmlFileName);
				}
				return _projectSettingsDocument;
			}
		}

		public static string WindowsApplicationDriverUrl => ProjectSettingsDocument.GetTextSingleNode("//windowsApplicationDriverUrl");

		public static string ApplicationPath => ProjectSettingsDocument.GetTextSingleNode("//applicationPath");

		public static string ApplicationDirectory => ProjectSettingsDocument.GetTextSingleNode("//applicationDirectory");

		public static string ApplicationTopLevelWindow => ProjectSettingsDocument.GetTextSingleNode("//applicationTopLevelWindow");

		public static int RemoteDebuggingPort => int.Parse(ProjectSettingsDocument.GetTextSingleNode("//remoteDebuggingPort"));

		public static DataBaseConfig Load(string dataBaseName)
		{
			DataBaseConfig dataBaseConfig = new DataBaseConfig
			{
				DataSource = ProjectSettingsDocument.GetTextSingleNode("//dataBase[@name='" + dataBaseName + "']/dataSource"),
				InitialCatalog = ProjectSettingsDocument.GetTextSingleNode("//dataBase[@name='" + dataBaseName + "']/initialCatalog"),
				IntegratedSecurity = ProjectSettingsDocument.GetTextSingleNode("//dataBase[@name='" + dataBaseName + "']/integratedSecurity"),
				ConnectionTimeout = ProjectSettingsDocument.GetTextSingleNode("//dataBase[@name='" + dataBaseName + "']/connectionTimeout"),
			};
			return dataBaseConfig;
		}

		public static string DbUsername => ProjectSettingsDocument.GetTextSingleNode("databaseSettings//username");

		public static string DbPassword => ProjectSettingsDocument.GetTextSingleNode("databaseSettings//password");
	}

	public class DataBaseConfig
	{
		public string DataSource { get; set; }
		public string InitialCatalog { get; set; }
		public string IntegratedSecurity { get; set; }
		public string ConnectionTimeout { get; set; }
	}
}
