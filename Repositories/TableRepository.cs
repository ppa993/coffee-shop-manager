﻿using CSM.DataAccess;
using CSM.DataModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSM.Repositories
{
	public class TableRepository : ITableRepository
	{
        protected readonly string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\apham24\Documents\GitHub\coffee-shop-manager\DataAccess\Database.mdf;Integrated Security=True;Connect Timeout=30";
		DataAccessManager dataAccess;

        public TableRepository()
        {
			dataAccess = new DataAccessManager(ConnectionString);
		}

        public void Dispose()
        {

		}

		public async Task<IEnumerable<Table>> GetTables()
		{
			return await dataAccess.GetTables();
		}

		public async Task<Table> GetTableByID(int id)
		{
			return await dataAccess.GetTableByID(id);
		}

		public async Task<IEnumerable<Product>> GetProducts()
		{
			return await dataAccess.GetProducts();
		}

		public async Task<bool> UpdateTableProducts(int tableID, int productID, UpdateAction action, int targetID)
		{
			return await dataAccess.UpdateTableProduct(tableID, productID, action, targetID);
		}

		public async Task<bool> MoveTable(int tableID, int targetID)
		{
			return await dataAccess.MoveTable(tableID, targetID);
		}
	}
}
