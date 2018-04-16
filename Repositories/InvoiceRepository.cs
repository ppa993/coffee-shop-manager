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
		DataAccessManager dataAccess;

		public InvoiceRepository()
		{
			dataAccess = new DataAccessManager();
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

		public Task<IEnumerable<Income>> GetIncomeByType(StatisticType type, DateTime time)
		{
			return dataAccess.GetIncomeByType(type, time);
		}
	}
}
