namespace CSM.DataModel
{
	public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public long Price { get; set; }
		public bool IsDeleted { get; set; }

		public Product(int id, string name, long price)
		{
			this.ID = id;
			this.Name = name;
			this.Price = price;
		}
    }

	public class ProductCollection
	{
		public Product Product { get; set; }
		public int Quantity { get; set; }


		public ProductCollection(Product product, int quantity)
		{
			this.Product = product;
			this.Quantity = quantity;
		}
	}
}
