using CSM.DataModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSM.Repositories
{
	public class TableRepository : ITableRepository
	{
        protected readonly string ConnectionString;
		List<Product> products;
		List<Table> tables;

        public TableRepository()
        {
			products = new List<Product>
			{
				new Product(1, "Coca", 10000),
				new Product(2, "Pepsi", 11000),
				new Product(3, "Lavie", 7000),
				new Product(4, "Milk coffee", 8000),
				new Product(5, "Aquafina", 7000),
				new Product(6, "Black coffee", 9000),
				new Product(7, "Capuchino", 12000),
				new Product(8, "Sting", 9000),
				new Product(9, "Ginger tea", 15000)
			};

			tables = new List<Table>();

			var productCollection = new List<ProductCollection>
			{
				new ProductCollection(products[0], 1),
				new ProductCollection(products[2], 3),
				new ProductCollection(products[5], 4),
				new ProductCollection(products[7], 2),
				new ProductCollection(products[1], 1),
				new ProductCollection(products[3], 3),
				new ProductCollection(products[4], 2),
				new ProductCollection(products[5], 3)
			};
			tables.Add(new Table(1, productCollection));
			tables[0].GetTotal();

			productCollection = new List<ProductCollection>
			{
				new ProductCollection(products[0], 4),
				new ProductCollection(products[1], 3),
				new ProductCollection(products[2], 2),
				new ProductCollection(products[3], 1)
			};
			tables.Add(new Table(7, productCollection));
			tables[1].GetTotal();
		}

        public void Dispose()
        {

		}

		public async Task<IEnumerable<Table>> GetTables()
		{
			return await Task.FromResult(tables);
		}

		public async Task<Table> GetTableByID(int id)
		{
			var table = tables.Where(x => x.ID == id).FirstOrDefault();
			return await Task.FromResult(table);
		}

		public async Task<IEnumerable<Product>> GetProducts()
		{
			return await Task.FromResult(products);
		}
	}
}
