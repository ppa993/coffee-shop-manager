using CSM.DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSM.Repositories
{
	public interface ITableRepository : IRepository
	{
		Task<IEnumerable<Table>> GetTables();
		Task<Table> GetTableByID(int id);
		Task<IEnumerable<Product>> GetProducts();
		Task<bool> UpdateTableProducts(int tableID, int productID, UpdateAction action, int targetID);
		Task<bool> MoveTable(int tableID, int targetID);
	}
}
