using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Selenium.Utilities
{
	//
	// Summary:
	//     Contains extension methods for specflow table
	public static class SpecFlowTableExtensions
	{		
		//
		// Summary:
		//     Converts the specflow table with a single row to dictionary that maps the table
		//     header to that of its row
		//
		// Parameters:
		//   table:
		//     The specflow table
		//
		// Returns:
		//     A dictionary with header as key and row as value
		public static Dictionary<string, string> ToDictionary(this Table table)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			string[] array = table.Header.ToArray();
			if (table.RowCount > 0)
			{
				for (int i = 0; i < array.Length; i++)
				{
					dictionary.Add(array[i], table.Rows[0][i]);
				}
			}

			return dictionary;
		}

		//
		// Summary:
		//     Converts the specflow table with a multiple rows to dictionary that maps the
		//     table header to that of its rows
		//
		// Parameters:
		//   table:
		//     The specflow table
		//
		//   multipleRows:
		//     Flag
		//
		// Returns:
		//     A dictionary with header as key and an array of rows as value
		public static Dictionary<string, string[]> ToDictionary(this Table table, bool multipleRows)
		{
			if (multipleRows)
			{
				Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
				string[] array = table.Header.ToArray();
				if (table.RowCount > 0)
				{
					for (int i = 0; i < array.Length; i++)
					{
						string[] array2 = new string[table.RowCount];
						for (int j = 0; j < table.RowCount; j++)
						{
							array2[j] = table.Rows[j][i];
						}

						dictionary.Add(array[i], array2);
					}
				}

				return dictionary;
			}

			throw new SpecFlowException("this method is used for reading multiple rows from specflow table");
		}
	}
}
