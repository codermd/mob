using System;
using System.Collections.Generic;
using System.Globalization;
using Mxp.Utils;

namespace Mxp.Core.Business
{
	public class TravelDynamicField : DynamicField
	{
		public TravelDynamicField(Travel model, DynamicFieldHolder dynamicFieldHolder) : base(model, dynamicFieldHolder) {
			this.linkName = this.kvoSelectorMapper [this.DynamicFieldHolder.LinkName.ToLower ()];
			this.Title = LoggedUser.Instance.Labels.GetLabel (this.kvoTitleMapper [dynamicFieldHolder.LinkName.ToLower()]);
		}

		public override bool IsEditable {
			get {
				return false;
			}
		}
			
		private Dictionary<String,String> kvoSelectorMapper = new Dictionary<String,String>(){
			{ "parchar1", "TravelRequestparchar1ov" },
			{ "parchar2", "TravelRequestparchar2ov" },
			{ "parchar3", "TravelRequestparchar3ov" },
			{ "parchar4", "TravelRequestparchar4ov" },
			{ "parchar5", "TravelRequestparchar5ov"},
			{ "parchar6", "TravelRequestparchar6ov" },
			{ "parchar7", "TravelRequestparchar7ov" },
			{ "parchar8", "TravelRequestparchar8ov" },
			{ "parchar9", "TravelRequestparchar9ov" },
			{ "parchar10", "TravelRequestparchar10ov" },
			{ "parint1", "TravelRequestparint1ov" },
			{ "parint2", "TravelRequestparint2ov" },
			{ "parint3", "TravelRequestparint3ov" },
			{ "parint4", "TravelRequestparint4ov" },
			{ "parint5", "TravelRequestparint5ov" },
			{ "parint6", "TravelRequestparint6ov" },
			{ "parint7", "TravelRequestparint7ov" },
			{ "parint8", "TravelRequestparint8ov" },
			{ "parint9", "TravelRequestparint9ov" },
			{ "parint10", "TravelRequestparint10ov" },
			{ "paramount1", "TravelRequestparamount1ov" },
			{ "paramount2", "TravelRequestparamount2ov" },
			{ "paramount3", "TravelRequestparamount3ov" },
			{ "paramount4", "TravelRequestparamount4ov" },
			{ "paramount5", "TravelRequestparamount5ov" },
			{ "paramount6", "TravelRequestparamount6ov" },
			{ "paramount7", "TravelRequestparamount7ov" },
			{ "paramount8", "TravelRequestparamount8ov" },
			{ "paramount9", "TravelRequestparamount9ov" },
			{ "paramount10", "TravelRequestparamount10ov" },
			{ "parind1", "TravelRequestparind1ov" },
			{ "parind2", "TravelRequestparind2ov" },
			{ "parind3", "TravelRequestparind3ov" },
			{ "parind4", "TravelRequestparind4ov" },
			{ "parind5", "TravelRequestparind5ov" },
			{ "parind6", "TravelRequestparind6ov" },
			{ "parind7", "TravelRequestparind7ov" },
			{ "parind8", "TravelRequestparind8ov" },
			{ "parind9", "TravelRequestparind9ov" },
			{ "parind10", "TravelRequestparind10ov" }
		};

		private Dictionary<String,int> kvoTitleMapper = new Dictionary<String,int>(){
			{ "parchar1", 836 }, 	
			{ "parchar2", 837 }, 	
			{ "parchar3", 838 }, 	
			{ "parchar4", 839 }, 	
			{ "parchar5", 840 }, 	
			{ "parchar6", 2056 }, 	
			{ "parchar7", 2057 }, 	
			{ "parchar8", 2058 }, 	
			{ "parchar9", 2059 }, 	
			{ "parchar10", 2060 }, 	
			{ "parint1", 841 }, 	
			{ "parint2", 842 },	
			{ "parint3", 843 }, 	
			{ "parint4", 844 }, 	
			{ "parint5", 845 }, 	
			{ "parint6", 2061 }, 	
			{ "parint7", 2062 }, 	
			{ "parint8", 2063 }, 	
			{ "parint9", 2064 }, 	
			{ "parint10", 2065 }, 	
			{ "paramount1", 853 }, 	
			{ "paramount2", 854 }, 	
			{ "paramount3", 855 }, 	
			{ "paramount4", 856 }, 	
			{ "paramount5", 857 }, 	
			{ "paramount6", 2071 }, 	
			{ "paramount7", 2072 }, 	
			{ "paramount8", 2073 }, 	
			{ "paramount9", 2074 }, 	
			{ "paramount10", 2075 }, 	
			{ "parind1", 846 }, 	
			{ "parind2", 847 }, 	
			{ "parind3", 848 }, 	
			{ "parind4", 851 }, 	
			{ "parind5", 852 }, 	
			{ "parind6", 2066 }, 	
			{ "parind7", 2067 }, 	
			{ "parind8", 2068 }, 	
			{ "parind9", 2069 }, 	
			{ "parind10", 207 }
		};
	}
}