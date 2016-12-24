using System;
using System.Collections.ObjectModel;

namespace Mxp.Core.Business
{
	public class TableSectionModel
	{
		public string Title;
		public Collection<Field> Fields = new Collection<Field>();

		public TableSectionModel ()
		{

		}

		public TableSectionModel (string title) : this()
		{
			this.Title = title;
		}

		public TableSectionModel (string title, Collection<Field> fields) : this(title)
		{
			this.Fields = fields;
		}

		public TableSectionModel (Collection<Field> fields) : this()
		{
			this.Fields = fields;
		}
	}
}

