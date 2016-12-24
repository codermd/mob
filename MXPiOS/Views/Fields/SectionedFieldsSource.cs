using System;
using System.Collections.ObjectModel;
using Mxp.Core.Business;
using UIKit;
using Foundation;
using Mxp.Core.Utils;

namespace Mxp.iOS
{
	public class SectionedFieldsSource : TableSectionsSource
	{
		public SectionedFieldsSource(Collection<TableSectionModel> sectionsModel, UIViewController viewController){

			this.Sections = new Collection<SectionSource>();
			sectionsModel.ForEach(tableSectionModel=>{
				this.Sections.Add(new SectionFieldsSource(tableSectionModel, viewController));
			});
		}
	}
}

