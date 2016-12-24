using System;

namespace sc
{
	public class Product : TableItem
	{

		public string Name { get; set; }
		public string Id { get; set; }

		public Product ()
		{
		}

		public string title {
			get {
				return this.Name;
			}
		}
	}
}

