using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkCapabilities.Utilities.DatabaseOperations
{
	public class SqlHelpers
	{
		public SqlConnection GetSqlConnection(string dataSource,string initialCatalog, string integratedSecurity,string connectionTimeout, string username="", string password="")
		{
			SqlConnection connection = null;
			string connectionString;
			try
			{
				connectionString = String.Format("Data Source = {0}; Initial Catalog = {1}; Integrated Security = {2}; Connection Timeout = {3}",
					dataSource, initialCatalog, integratedSecurity,connectionTimeout);
				connection = new SqlConnection(connectionString);
				connection.Open();
				connection.Close();
			}
			catch (SqlException)
			{
				
			}
			return connection;
		}

		public void ExecuteQuery(SqlConnection connection, string query)
		{
			using (SqlCommand command = new SqlCommand(query, connection))
			{
				try
				{
					connection.Open();
					command.ExecuteNonQuery();
				}
				finally
				{
					connection.Close();
				}
			}
		}

		public T ExecuteAndGetScalarValue<T>(SqlConnection connection, string query)
		{
			using (SqlCommand command = new SqlCommand(query, connection))
			{
				try
				{
					connection.Open();
					object value = command.ExecuteScalar();
					if (value == null || value == DBNull.Value)
					{
						return default(T);
					}
					return (T)value;
				}
				finally
				{
					connection.Close();
				}
			}
		}

		public List<T> ExecuteAndGetList<T>(SqlConnection connection, string query)
		{
			using (SqlCommand command = new SqlCommand(query, connection))
			{
				try
				{
					connection.Open();
					List<T> result = new List<T>();
					SqlDataReader reader = command.ExecuteReader();
					while (reader.Read())
					{
						result.Add((T)reader.GetValue(0));
					}
					return result;
				}
				finally
				{
					connection.Close();
				}
			}
		}
	}
}
