using System;

namespace sc
{
	public class Country : TableItem
	{

		public string Id { get; set; }
		public string Name { get; set; }
		public bool Recent { get; set; }

		public Country ()
		{
		}

		public override string ToString()
		{
			return string.Format("[Country: Id={0}, Name={1}, Recent={2}, title={3}]", Id, Name, Recent, title);
		}

		public string title {
			get {
				return this.Name;
			}
		}
	}
}

