using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FrameworkCapabilities.Utilities.FileHandlers
{
	//
	// Summary:
	//     Contains methods to parse and read the XML content using XmlDocument class
	public class XmlDocumentReader
	{
		private readonly XmlDocument _xmldoc = new XmlDocument();

		private readonly XmlNamespaceManager _xmlnsManager = null;

		private readonly Hashtable _ns = new Hashtable();

		//
		// Summary:
		//     Reads and loads the xml file as XmlDocument
		//
		// Parameters:
		//   data:
		public XmlDocumentReader(string data)
		{
			Encoding encoding = Encoding.GetEncoding("UTF-8");
			StreamReader streamReader = null;
			try
			{
				if (data.EndsWith(".xml"))
				{
					streamReader = new StreamReader(data, encoding);
					_xmldoc.LoadXml(streamReader.ReadToEnd());
				}
				else
				{
					_xmldoc.LoadXml(data);
				}

				_xmlnsManager = new XmlNamespaceManager(_xmldoc.NameTable);
				XmlNodeList xmlNodeList = _xmldoc.SelectNodes("//namespace::*[not(. = ../../namespace::*)]");
				foreach (XmlNode item in xmlNodeList)
				{
					if (!_ns.ContainsKey(item.LocalName))
					{
						_ns.Add(item.LocalName, item.Value);
						_xmlnsManager.AddNamespace(item.LocalName, item.Value);
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error occured while reading xml file as document " + ex.ToString());
			}
			finally
			{
				streamReader?.Dispose();
			}
		}

		//
		// Summary:
		//     Gets the node using xpath
		//
		// Parameters:
		//   xpath:
		public XmlNode GetSingleNode(string xpath)
		{
			return GetSingleNode(_xmldoc, xpath);
		}

		//
		// Summary:
		//     Gets the node using parent node and xpath
		//
		// Parameters:
		//   parent:
		//
		//   xpath:
		public XmlNode GetSingleNode(XmlNode parent, string xpath)
		{
			return parent.SelectSingleNode(xpath);
		}

		//
		// Summary:
		//     Gets the list of nodes using xpath
		//
		// Parameters:
		//   xpath:
		public XmlNodeList GetNodesList(string xpath)
		{
			return GetNodesList(_xmldoc, xpath);
		}

		//
		// Summary:
		//     Gets the list of nodes using parent node and xpath
		//
		// Parameters:
		//   parent:
		//
		//   xpath:
		public XmlNodeList GetNodesList(XmlNode parent, string xpath)
		{
			return parent.SelectNodes(xpath, _xmlnsManager);
		}

		//
		// Summary:
		//     Gets the inner text of a node using xpath
		//
		// Parameters:
		//   xpath:
		public string GetTextSingleNode(string xpath)
		{
			return GetTextSingleNode(_xmldoc, xpath);
		}

		//
		// Summary:
		//     Gets the inner text of a node using parent node and xpath
		//
		// Parameters:
		//   parent:
		//
		//   xpath:
		public string GetTextSingleNode(XmlNode parent, string xpath)
		{
			XmlNodeList nodesList = GetNodesList(parent, xpath);
			return nodesList.Item(0).InnerText.ToString();
		}

		//
		// Summary:
		//     Gets the inner text of all the nodes using xpath
		//
		// Parameters:
		//   xpath:
		public Hashtable GetTextMultipleNode(string xpath)
		{
			return GetTextMultipleNode(_xmldoc, xpath);
		}

		//
		// Summary:
		//     Gets the inner text of all the sub-nodes for a given parent node using xpath
		//
		// Parameters:
		//   xpath:
		//
		//   parent:
		public Hashtable GetTextMultipleNode(XmlNode parent, string xpath)
		{
			Hashtable hashtable = new Hashtable();
			XmlNodeList nodesList = GetNodesList(parent, xpath);
			for (int i = 0; i < nodesList.Count; i++)
			{
				hashtable.Add(i, nodesList.Item(i).InnerText.ToString());
			}

			return hashtable;
		}

		//
		// Summary:
		//     Gets the attribute inner text for the given xpath
		//
		// Parameters:
		//   xpath:
		//
		//   attribute:
		public string GetAttributeSingleNode(string xpath, string attribute)
		{
			return GetAttributeSingleNode(_xmldoc, xpath, attribute);
		}

		//
		// Summary:
		//     Gets the attribute inner text for the given parent node and xpath
		//
		// Parameters:
		//   parent:
		//
		//   xpath:
		//
		//   attribute:
		public string GetAttributeSingleNode(XmlNode parent, string xpath, string attribute)
		{
			XmlNodeList nodesList = GetNodesList(parent, xpath);
			return nodesList.Item(0).Attributes.GetNamedItem(attribute).InnerText.ToString();
		}
	}
}
