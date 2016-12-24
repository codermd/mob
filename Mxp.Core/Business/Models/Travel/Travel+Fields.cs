using System;
using System.Collections.ObjectModel;
using Mxp.Core.Utils;

namespace Mxp.Core.Business
{
	public partial class Travel
	{
		public Collection<TableSectionModel> GetFlightsFields () {
			Collection<TableSectionModel> fields = new Collection<TableSectionModel> ();

			this.Flights.ForEach ((flight, index) => {
				string title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Flight) + " " + (index + 1);
				fields.Add (new TableSectionModel (title, flight.GetMainFields ()));
			});

			return fields;
		}

		public Collection<TableSectionModel> GetStayFields () {
			Collection<TableSectionModel> fields = new Collection<TableSectionModel> ();

			this.Stays.ForEach ((stay, index) => {
				string title = Labels.GetLoggedUserLabel (Labels.LabelEnum.Stay) + " " + (index + 1);
				fields.Add (new TableSectionModel (title, stay.GetMainFields ()));
			});

			return fields;
		}
			
		public Collection<TableSectionModel> GetCarRentalsFields () {
			Collection<TableSectionModel> fields = new Collection<TableSectionModel> ();

			this.CarRentals.ForEach ((carRental, index) => {
				string title = Labels.GetLoggedUserLabel (Labels.LabelEnum.CarRental) + " " + (index + 1);
				fields.Add (new TableSectionModel (title, carRental.GetMainFields ()));
			});

			return fields;
		}

		public Collection<TableSectionModel> GetMainFields () {
			TableSectionModel firstSection = new TableSectionModel (Labels.GetLoggedUserLabel (Labels.LabelEnum.Traveller), new Collection<Field> () {
				new TravelFieldFirstName (this),
				new TravelFieldLastName (this)
			});

			TableSectionModel thirdSection = new TableSectionModel (Labels.GetLoggedUserLabel (Labels.LabelEnum.Comment), new Collection<Field> () {
				new TravelFieldComment (this)
			});
				
			return new Collection<TableSectionModel> () {
				firstSection,
				this.GetDynamicTableSectionModel (),
				thirdSection
			};
		}

		public TableSectionModel GetDynamicTableSectionModel () {
			Collection<Field> fields = new Collection<Field> ();

			fields.Add (new TravelFieldName (this));
			fields.Add (new TravelFieldMaxAmount (this));
			fields.Add (new TravelFieldFromDate (this));
			fields.Add (new TravelFieldToDate (this));
			fields.Add (new TravelFieldStatus (this));

			if (Preferences.Instance.CanShowPermission (Preferences.Instance.TVRProject))
				fields.Add(new TravelFieldProject (this));

			if (Preferences.Instance.CanShowPermission (Preferences.Instance.TVRCostCenter))
				fields.Add(new TravelFieldProject (this));
				
			this.GetDynamicFields ().ForEach (field => fields.Add (field));

			fields.Add (new TravelFieldOtherRequests (this));

			return new TableSectionModel (Labels.GetLoggedUserLabel (Labels.LabelEnum.Travel), fields);
		}

		public Collection<Field> GetDynamicFields(){
			Collection<Field> fields = new Collection<Field> ();

			Preferences.Instance.GetTravelDynamicFields ().ForEach (dynamicFieldHolder => {
				fields.Add (new TravelDynamicField (this, dynamicFieldHolder));
			});

			return fields;
		}
	}
}