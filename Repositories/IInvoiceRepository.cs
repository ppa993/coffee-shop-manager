using CSM.DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSM.Repositories
{
	public interface IInvoiceRepository : IRepository
	{
		Task<IEnumerable<Invoice>> GetInvoices();
		Task<Invoice> GetInvoiceByID(int id);
		Task<bool> CreateNewInvoice(int tableID);
	}
}
