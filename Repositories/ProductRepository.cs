﻿using CSM.DataAccess;
using CSM.DataModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSM.Repositories
{
	public class ProductRepository : IProductRepository
	{
        DataAccessManager dataAccess;

        public ProductRepository()
        {
			dataAccess = new DataAccessManager();
		}

		public void Dispose()
		{
		}

		public Task<IEnumerable<Product>> GetProducts()
		{
			return dataAccess.GetProducts();
		}

		public Task<Product> GetProductByID(int id)
		{
			return dataAccess.GetProductByID(id);
		}

		public Task<bool> RemoveProduct(int id)
		{
			return dataAccess.RemoveProduct(id);
		}

		public Task<bool> UpdateProduct(int id, Product newProduct)
		{
			return dataAccess.UpdateProduct(id, newProduct);
		}

		public Task<bool> AddNewProduct(Product newProduct)
		{
			return dataAccess.AddNewProduct(newProduct);
		}
	}
}
