using CSM.DataAccess;
using CSM.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSM.Repositories
{
	public class InvoiceRepository : IInvoiceRepository
	{
		protected readonly string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\apham24\Documents\GitHub\coffee-shop-manager\DataAccess\Database.mdf;Integrated Security=True;Connect Timeout=30";
		DataAccessManager dataAccess;

		public InvoiceRepository()
		{
			dataAccess = new DataAccessManager(ConnectionString);
		}

		public void Dispose()
		{

		}

		public Task<IEnumerable<Invoice>> GetInvoices()
		{
			return dataAccess.GetInvoices();
		}

		public Task<Invoice> GetInvoiceByID(int id)
		{
			return dataAccess.GetInvoiceByID(id);
		}

		public Task<bool> CreateNewInvoice(int tableID)
		{
			return dataAccess.CreateNewInvoice(tableID);
		}
	}
}
