using CSM.DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSM.Repositories
{
	public interface IProductRepository : IRepository
	{
		Task<IEnumerable<Product>> GetProducts();
		Task<Product> GetProductByID(int id);
		Task<bool> AddNewProduct(Product newProduct);
		Task<bool> UpdateProduct(int id, Product newProduct);
		Task<bool> RemoveProduct(int id);
	}
}
