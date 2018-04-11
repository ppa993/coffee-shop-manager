using System;
using System.Collections.Generic;

namespace CSM.DataModel
{
	public class Invoice
    {
		public int ID { get; set; }
		public int TableID { get; set; }
		public IEnumerable<ProductCollection> Products { get; set; }
		public long Total { get; set; }
		public DateTime CreatedDate { get; set; }
		
		public Invoice()
		{
		}
		public Invoice(int id, int tableID, long total, DateTime createdDate)
		{
			this.ID = id;
			this.TableID = tableID;
			this.Total = total;
			this.CreatedDate = createdDate;
		}

    }
}
