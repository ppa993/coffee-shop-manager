﻿using CSM.DataModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CSM.DataAccess
{
	public class DataAccessManager
	{
		protected readonly string connectionString;

		#region Constructor

		public DataAccessManager()
		{
			connectionString = @"Server=tcp:csmrestapi.database.windows.net,1433;Initial Catalog=CsmRestAPIDb;Persist Security Info=False;User ID=anst;Password=ilBA85!@;";
		}
		#endregion

		#region TABLE
		#region GetTables
		public async Task<IEnumerable<Table>> GetTables()
		{
			IEnumerable<Table> result = Enumerable.Empty<Table>();
			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					conn.Open();

					var query = @"SELECT * FROM TABLES";
					using (SqlCommand command = new SqlCommand(query, conn))
					{
						using (SqlDataReader reader = await command.ExecuteReaderAsync())
						{
							var tables = new List<Table>();
							while (await reader.ReadAsync())
							{
								var id = await reader.GetFieldValueAsync<int>(0);
								var table = await GetTableByID(id);
								tables.Add(table);
							}
							result = tables;
						}
					}
				}
			}
			catch (Exception ex)
			{
				//write log
			}

			return await Task.FromResult(result);
		}
		#endregion

		#region GetTableByID
		public async Task<Table> GetTableByID(int id)
		{
			Table result = new Table();
			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					conn.Open();
					var table = new Table(id);
					var products = await GetTableProducts(id, conn);
					table.Products = products;
					table.GetTotal();
					result = table;
				}
			}
			catch (Exception ex)
			{
				//write log
			}

			return await Task.FromResult(result);
		}

		public async Task<IEnumerable<ProductCollection>> GetTableProducts(int id, SqlConnection conn)
		{
			IEnumerable<ProductCollection> result = Enumerable.Empty<ProductCollection>();
			try
			{
				var productsQuery = @"SELECT b.ID, b.NAME, b.PRICE, a.QUANTITY FROM TABLEDETAIL a JOIN PRODUCT b
										ON a.PRODUCTID = b.ID
										WHERE a.TABLEID = @id";
				using (SqlCommand productsCommand = new SqlCommand(productsQuery, conn))
				{
					productsCommand.Parameters.Add("@id", System.Data.SqlDbType.Int);
					productsCommand.Parameters["@id"].Value = id;

					using (SqlDataReader productsReader = await productsCommand.ExecuteReaderAsync())
					{
						var products = new List<ProductCollection>();
						while (await productsReader.ReadAsync())
						{
							var productID = await productsReader.GetFieldValueAsync<int>(0);
							var productName = await productsReader.GetFieldValueAsync<string>(1);
							var productPrice = await productsReader.GetFieldValueAsync<long>(2);
							var quantity = await productsReader.GetFieldValueAsync<int>(3);

							products.Add(new ProductCollection(new Product(productID, productName, productPrice), quantity));
						}
						result = products;
					}
				}
			}
			catch (Exception ex)
			{
				//write log
			}

			return await Task.FromResult(result);
		}
		#endregion

		#region Update table methods
		public async Task<bool> AddTableProduct(int tableID, int productID, SqlConnection conn)
		{
			var result = false;
			try
			{
				var query = @"IF EXISTS (SELECT * FROM [dbo].[TableDetail] WHERE TableID=@tableID AND ProductID=@productID)
							BEGIN
								UPDATE [dbo].[TableDetail] SET Quantity = Quantity + 1
								WHERE TableID=@tableID AND ProductID=@productID
							END
							ELSE
							BEGIN
								INSERT INTO [dbo].[TableDetail](TableID, ProductID, Quantity)
								VALUES(@tableID, @productID, 1)
							END";

				using (SqlCommand command = new SqlCommand(query, conn))
				{
					command.Parameters.Add("@tableID", System.Data.SqlDbType.Int);
					command.Parameters.Add("@productID", System.Data.SqlDbType.Int);
					command.Parameters["@tableID"].Value = tableID;
					command.Parameters["@productID"].Value = productID;

					var rows = await command.ExecuteNonQueryAsync();
					result = rows > 0;
				}
			}
			catch (Exception ex)
			{
				//write log
			}

			return await Task.FromResult(result);
		}

		public async Task<bool> RemoveTableProduct(int tableID, int productID, SqlConnection conn)
		{
			var result = false;
			try
			{
				var query = @"IF EXISTS (SELECT * FROM [dbo].[TableDetail] WHERE TableID=@tableID AND ProductID=@productID AND Quantity > 1)
							BEGIN 
								UPDATE [dbo].[TableDetail] SET Quantity = Quantity - 1
								WHERE TableID=@tableID AND ProductID=@productID
							END
							ELSE
							BEGIN
								DELETE FROM dbo.TableDetail WHERE TableID=@tableID AND ProductID=@productID
							END";

				using (SqlCommand command = new SqlCommand(query, conn))
				{
					command.Parameters.Add("@tableID", System.Data.SqlDbType.Int);
					command.Parameters.Add("@productID", System.Data.SqlDbType.Int);
					command.Parameters["@tableID"].Value = tableID;
					command.Parameters["@productID"].Value = productID;

					var rows = await command.ExecuteNonQueryAsync();
					result = rows > 0;
				}
			}
			catch (Exception ex)
			{
				//write log
			}

			return await Task.FromResult(result);
		}

		public async Task<bool> MoveTableProduct(int tableID, int productID, int targetID, SqlConnection conn)
		{
			var result = false;
			try
			{
				var query = @"IF EXISTS (SELECT * FROM [dbo].[TableDetail] WHERE TableID=@targetID AND ProductID=@productID)
							BEGIN
								UPDATE [dbo].[TableDetail] SET Quantity = Quantity + (SELECT Quantity FROM [dbo].[TableDetail] WHERE TableID=@tableID AND ProductID=@productID)
								WHERE TableID=@targetID AND ProductID=@productID

								DELETE FROM dbo.TableDetail WHERE TableID=@tableID AND ProductID=@productID
							END
							ELSE
							BEGIN
								UPDATE [dbo].[TableDetail] SET TableID = @targetID
								WHERE TableID=@tableID AND ProductID=@productID					
							END";

				using (SqlCommand command = new SqlCommand(query, conn))
				{
					command.Parameters.Add("@tableID", System.Data.SqlDbType.Int);
					command.Parameters.Add("@targetID", System.Data.SqlDbType.Int);
					command.Parameters.Add("@productID", System.Data.SqlDbType.Int);
					command.Parameters["@tableID"].Value = tableID;
					command.Parameters["@targetID"].Value = targetID;
					command.Parameters["@productID"].Value = productID;

					var rows = await command.ExecuteNonQueryAsync();
					result = rows > 0;
				}
			}
			catch (Exception ex)
			{
				//write log
			}

			return await Task.FromResult(result);
		}

		public async Task<bool> MoveTable(int tableID, int targetID)
		{
			var result = false;
			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					conn.Open();
					var table = await GetTableByID(tableID);

					//Update target table
					var sourceProducts = table.Products;
					foreach (var product in sourceProducts)
					{
						result = await MoveTableProduct(tableID, product.Product.ID, targetID, conn);
					}
				}
			}
			catch (Exception ex)
			{
				//write log
			}

			return await Task.FromResult(result);
		}

		public async Task<bool> UpdateTableProduct(int tableID, int productID, UpdateAction action, int targetID)
		{
			var result = false;
			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					conn.Open();
					switch (action)
					{
						case UpdateAction.Add:
							result = await AddTableProduct(tableID, productID, conn);
							break;
						case UpdateAction.Remove:
							result = await RemoveTableProduct(tableID, productID, conn);
							break;
						default:
							result = await MoveTableProduct(tableID, productID, targetID, conn);
							break;
					}
				}
			}
			catch (Exception ex)
			{
				//write log
			}

			return await Task.FromResult(result);
		}
		#endregion
		#endregion

		#region PRODUCT
		#region GetProducts
		public async Task<IEnumerable<Product>> GetProducts()
		{
			IEnumerable<Product> result = Enumerable.Empty<Product>();
			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					conn.Open();
					var query = @"SELECT * FROM PRODUCT WHERE ISDELETED = 0";
					using (SqlCommand command = new SqlCommand(query, conn))
					{
						using (SqlDataReader reader = await command.ExecuteReaderAsync())
						{
							var products = new List<Product>();
							while (await reader.ReadAsync())
							{
								var id = await reader.GetFieldValueAsync<int>(0);
								var name = await reader.GetFieldValueAsync<string>(1);
								var price = await reader.GetFieldValueAsync<long>(2);

								var product = new Product(id, name, price);

								products.Add(product);
							}
							result = products;
						}
					}
				}
			}
			catch (Exception ex)
			{
				//write log
			}

			return await Task.FromResult(result);
		}
		#endregion

		#region GetProductByID
		public async Task<Product> GetProductByID(int id)
		{
			Product result = new Product();
			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					conn.Open();
					var query = @"SELECT * FROM PRODUCT WHERE Id = @id";
					using (SqlCommand command = new SqlCommand(query, conn))
					{
						using (SqlDataReader reader = await command.ExecuteReaderAsync())
						{
							command.Parameters.Add("@id", System.Data.SqlDbType.Int);
							command.Parameters["@id"].Value = id;

							while (await reader.ReadAsync())
							{
								var productId = await reader.GetFieldValueAsync<int>(0);
								var name = await reader.GetFieldValueAsync<string>(1);
								var price = await reader.GetFieldValueAsync<long>(2);

								result = new Product(productId, name, price);

							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				//write log
			}

			return await Task.FromResult(result);
		}
		#endregion

		#region RemoveProduct
		public async Task<bool> RemoveProduct(int id)
		{
			var result = false;
			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					conn.Open();

					var query = @"UPDATE [dbo].[Product] SET IsDeleted = 1
									WHERE ID=@id";

					using (SqlCommand command = new SqlCommand(query, conn))
					{
						command.Parameters.Add("@id", System.Data.SqlDbType.Int);
						command.Parameters["@id"].Value = id;

						var rows = await command.ExecuteNonQueryAsync();
						result = rows == 1;
					}
				}
			}
			catch (Exception ex)
			{
				//write log
			}

			return await Task.FromResult(result);
		}
		#endregion

		#region UpdateProduct
		public async Task<bool> UpdateProduct(int id, Product newProduct)
		{
			var result = false;
			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					conn.Open();
					var query = @"UPDATE [dbo].[Product] SET Name = @newName, Price = @newPrice
									WHERE ID=@id";
					using (SqlCommand command = new SqlCommand(query, conn))
					{
						command.Parameters.Add("@newName", System.Data.SqlDbType.NVarChar);
						command.Parameters.Add("@newPrice", System.Data.SqlDbType.BigInt);
						command.Parameters.Add("@id", System.Data.SqlDbType.Int);
						command.Parameters["@newName"].Value = newProduct.Name;
						command.Parameters["@newPrice"].Value = newProduct.Price;
						command.Parameters["@id"].Value = id;

						var rows = await command.ExecuteNonQueryAsync();
						result = rows == 1;
					}
				}
			}
			catch (Exception ex)
			{
				//write log
			}

			return await Task.FromResult(result);
		}
		#endregion

		#region AddNewProduct
		public async Task<bool> AddNewProduct(Product newProduct)
		{
			var result = false;
			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					conn.Open();
					var query = @"INSERT INTO [dbo].[Product](Name, Price)
									VALUES(@name, @price)";
					using (SqlCommand command = new SqlCommand(query, conn))
					{
						command.Parameters.Add("@name", System.Data.SqlDbType.NVarChar);
						command.Parameters.Add("@price", System.Data.SqlDbType.BigInt);
						command.Parameters["@name"].Value = newProduct.Name;
						command.Parameters["@price"].Value = newProduct.Price;

						var rows = await command.ExecuteNonQueryAsync();
						result = rows == 1;
					}
				}
			}
			catch (Exception ex)
			{
				//write log
			}

			return await Task.FromResult(result);
		}
		#endregion
		#endregion

		#region INVOICE
		#region GetInvoices
		public async Task<IEnumerable<Invoice>> GetInvoices()
		{
			IEnumerable<Invoice> result = Enumerable.Empty<Invoice>();
			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					conn.Open();

					var query = @"SELECT * FROM INVOICE";
					using (SqlCommand command = new SqlCommand(query, conn))
					{
						using (SqlDataReader reader = await command.ExecuteReaderAsync())
						{
							var invoices = new List<Invoice>();
							while (await reader.ReadAsync())
							{
								var id = await reader.GetFieldValueAsync<int>(0);
								var invoice = await GetInvoiceByID(id);
								invoices.Add(invoice);
							}
							result = invoices;
						}
					}
				}
			}
			catch (Exception ex)
			{
				//write log
			}

			return await Task.FromResult(result);
		}
		#endregion

		#region GetInvoiceByID
		public async Task<Invoice> GetInvoiceByID(int id)
		{
			Invoice result = new Invoice();
			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					conn.Open();
					var invoice = await GetInvoice(id, conn);
					var products = await GetInvoiceProducts(id, conn);
					invoice.Products = products;
					result = invoice;
				}
			}
			catch (Exception ex)
			{
				//write log
			}

			return await Task.FromResult(result);
		}
		public async Task<Invoice> GetInvoice(int id, SqlConnection conn)
		{
			Invoice result = new Invoice();
			try
			{
				var query = @"SELECT * FROM INVOICE WHERE Id=@id";
				using (SqlCommand productsCommand = new SqlCommand(query, conn))
				{
					productsCommand.Parameters.Add("@id", System.Data.SqlDbType.Int);
					productsCommand.Parameters["@id"].Value = id;

					using (SqlDataReader productsReader = await productsCommand.ExecuteReaderAsync())
					{
						Invoice invoice = new Invoice();
						while (await productsReader.ReadAsync())
						{
							var invoiceID = await productsReader.GetFieldValueAsync<int>(0);
							var tableID = await productsReader.GetFieldValueAsync<int>(1);
							var total = await productsReader.GetFieldValueAsync<long>(2);
							var createdDate = await productsReader.GetFieldValueAsync<DateTime>(3);

							invoice = new Invoice(invoiceID, tableID, total, createdDate);
						}
						result = invoice;
					}
				}
			}
			catch (Exception ex)
			{
				//write log
			}

			return await Task.FromResult(result);
		}

		public async Task<IEnumerable<ProductCollection>> GetInvoiceProducts(int id, SqlConnection conn)
		{
			IEnumerable<ProductCollection> result = Enumerable.Empty<ProductCollection>();
			try
			{
				var productsQuery = @"SELECT b.ID, b.NAME, b.PRICE, a.QUANTITY FROM INVOICEDETAIL a JOIN PRODUCT b
										ON a.PRODUCTID = b.ID
										WHERE a.INVOICEID = @id";
				using (SqlCommand productsCommand = new SqlCommand(productsQuery, conn))
				{
					productsCommand.Parameters.Add("@id", System.Data.SqlDbType.Int);
					productsCommand.Parameters["@id"].Value = id;

					using (SqlDataReader productsReader = await productsCommand.ExecuteReaderAsync())
					{
						var products = new List<ProductCollection>();
						while (await productsReader.ReadAsync())
						{
							var productID = await productsReader.GetFieldValueAsync<int>(0);
							var productName = await productsReader.GetFieldValueAsync<string>(1);
							var productPrice = await productsReader.GetFieldValueAsync<long>(2);
							var quantity = await productsReader.GetFieldValueAsync<int>(3);

							products.Add(new ProductCollection(new Product(productID, productName, productPrice), quantity));
						}
						result = products;
					}
				}
			}
			catch (Exception ex)
			{
				//write log
			}

			return await Task.FromResult(result);
		}
		#endregion

		#region CreateNewInvoice
		public async Task<bool> CreateNewInvoice(int tableID)
		{
			var result = false;
			try
			{
				var table = await GetTableByID(tableID);
				int invoiceID = 0;

				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					conn.Open();

					var query = @"INSERT INTO [dbo].[Invoice](TableID, Total, CreatedDate)
									OUTPUT Inserted.Id
									VALUES(@tableID, @total, @createdDate)";

					using (SqlCommand command = new SqlCommand(query, conn))
					{
						command.Parameters.Add("@tableID", System.Data.SqlDbType.Int);
						command.Parameters.Add("@total", System.Data.SqlDbType.BigInt);
						command.Parameters.Add("@createdDate", System.Data.SqlDbType.DateTime);
						command.Parameters["@tableID"].Value = tableID;
						command.Parameters["@total"].Value = table.Total;
						command.Parameters["@createdDate"].Value = DateTime.Now;

						invoiceID = (int)await command.ExecuteScalarAsync();
					}

					if (invoiceID != 0)
					{
						var detailQuery = @"BEGIN
												INSERT INTO [dbo].[InvoiceDetail](InvoiceID, ProductID, Quantity)
												SELECT @invoiceID, ProductID, Quantity FROM [dbo].[TableDetail]
												WHERE TableID = @tableID
											
												DELETE FROM [dbo].[TableDetail] WHERE TableID = @tableID
											END";

						using (SqlCommand command = new SqlCommand(detailQuery, conn))
						{
							command.Parameters.Add("@invoiceID", System.Data.SqlDbType.Int);
							command.Parameters.Add("@tableID", System.Data.SqlDbType.Int);
							command.Parameters["@invoiceID"].Value = invoiceID;
							command.Parameters["@tableID"].Value = tableID;

							var rows = await command.ExecuteNonQueryAsync();

							result = rows > 0; 
						}
					}
				}
			}
			catch (Exception ex)
			{
				//write log
			}

			return await Task.FromResult(result);
		}
		#endregion
		#endregion

		#region INCOME
		public async Task<IEnumerable<Income>> GetIncomeByType(StatisticType type, DateTime time)
		{
			IEnumerable<Income> result = Enumerable.Empty<Income>();
			try
			{
				switch (type)
				{
					case StatisticType.Day:
						result = await GetIncomeByDay(time);
						break;
					case StatisticType.Month:
						result = await GetIncomeByMonth(time);
						break;
					default:
						result = await GetIncomeByYear(time);
						break;
				}
			}
			catch (Exception ex)
			{
				//write log
			}

			return await Task.FromResult(result);
		}
		public async Task<IEnumerable<Income>> GetIncomeByDay(DateTime time)
		{
			IEnumerable<Income> result = Enumerable.Empty<Income>();
			try
			{
				var invoices = await GetInvoices();
				invoices = invoices.Where(x => x.CreatedDate.Date == time.Date).ToArray();

				List<Income> incomes = new List<Income>();
				foreach(var invoice in invoices)
				{
					var income = new Income()
					{
						Total = invoice.Total,
						Label = "Table #" +invoice.TableID + " " + invoice.CreatedDate.ToShortTimeString()
					};
					incomes.Add(income);
				}
				result = incomes;
			}
			catch (Exception ex)
			{
				//write log
			}

			return await Task.FromResult(result);
		}
		public async Task<IEnumerable<Income>> GetIncomeByMonth(DateTime time)
		{
			IEnumerable<Income> result = Enumerable.Empty<Income>();
			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					conn.Open();

					var query = @"SELECT SUM(Total), DAY(CreatedDate) FROM dbo.Invoice 
									WHERE MONTH(CreatedDate) = MONTH(@time) AND YEAR(CreatedDate) = YEAR(@time) 
									GROUP BY DAY(CreatedDate)";

					using (SqlCommand command = new SqlCommand(query, conn))
					{
						command.Parameters.Add("@time", System.Data.SqlDbType.DateTime);
						command.Parameters["@time"].Value = time;
						List<Income> monthIncome = new List<Income>();
						using (SqlDataReader reader = await command.ExecuteReaderAsync())
						{
							while (await reader.ReadAsync())
							{
								var total = await reader.GetFieldValueAsync<long>(0);
								var day = await reader.GetFieldValueAsync<int>(1);
								var income = new Income()
								{
									Total = total,
									Label = string.Format("{0}/{1}", day, time.Month)
								};
								monthIncome.Add(income);
							}
						}
						result = monthIncome;
					}
				}
			}
			catch (Exception ex)
			{
				//write log
			}

			return await Task.FromResult(result);
		}
		public async Task<IEnumerable<Income>> GetIncomeByYear(DateTime time)
		{
			IEnumerable<Income> result = Enumerable.Empty<Income>();
			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{
					conn.Open();

					var query = @"SELECT SUM(Total), MONTH(CreatedDate) FROM dbo.Invoice 
									WHERE YEAR(CreatedDate) = YEAR(@time) 
									GROUP BY MONTH(CreatedDate)";

					using (SqlCommand command = new SqlCommand(query, conn))
					{
						command.Parameters.Add("@time", System.Data.SqlDbType.DateTime);
						command.Parameters["@time"].Value = time;
						List<Income> yearIncome = new List<Income>();
						using (SqlDataReader reader = await command.ExecuteReaderAsync())
						{
							while (await reader.ReadAsync())
							{
								var total = await reader.GetFieldValueAsync<long>(0);
								var day = await reader.GetFieldValueAsync<int>(1);
								var income = new Income()
								{
									Total = total,
									Label = string.Format("{0}/{1}", day, time.Year)
								};
								yearIncome.Add(income);
							}
						}
						result = yearIncome;
					}
				}
			}
			catch (Exception ex)
			{
				//write log
			}

			return await Task.FromResult(result);
		}


		#endregion
	}
}
