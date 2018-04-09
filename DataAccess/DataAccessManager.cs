using CSM.DataModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CSM.DataAccess
{
	public class DataAccessManager : System.IDisposable
	{
		protected readonly SqlConnection Conn = null;

		public void Dispose()
		{
			if (Conn?.State == System.Data.ConnectionState.Open) Conn.Close();
			Conn?.Dispose();
		}

		public DataAccessManager(string connectionString)
		{
			try
			{
				Conn = new SqlConnection(connectionString);
				Conn.Open();
			}
			catch (Exception ex)
			{
				Conn?.Dispose();
				throw;
			}

		}

		public async Task<IEnumerable<Table>> GetAllApplications()
		{
			IEnumerable<Table> result = Enumerable.Empty<Table>();
			try
			{
				
			}
			catch (Exception ex)
			{
				//write log
			}

			return await Task.FromResult(result);
		}
	}
}
