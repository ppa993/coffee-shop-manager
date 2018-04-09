using System.Collections.Generic;

namespace CSM.DataModel
{
	public class Table
    {
		public int ID { get; set; }
		public IEnumerable<ProductCollection> Products { get; set; }
		public long Total { get; set; }
		
		public Table()
		{

		}
		public Table(int id, IEnumerable<ProductCollection> products)
		{
			this.ID = id;
			this.Products = products;
		}

		public void GetTotal()
		{
			long temp = 0;
			foreach(var product in this.Products)
			{
				temp += product.Product.Price * product.Quantity;
			}
			this.Total = temp;
		}
    }
}
