using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Mxp.Core.Utils;
using Mxp.Core.Services.Responses;

namespace Mxp.Core.Business
{
	public partial class AllowanceSegment
	{
		private Collection<Field> _allowanceFields;
		public Collection<Field> GetAllowanceFields () {
			if (this._allowanceFields == null) {
				this._allowanceFields = new Collection<Field> ();

				if (AFDateFrom.canCreate(this))
					this._allowanceFields.Add(new AFDateFrom(this));

				if (AFDateTo.canCreate(this))
					this._allowanceFields.Add(new AFDateTo(this));

				if (AFCountry.canCreate(this))
					this._allowanceFields.Add(new AFCountry(this));

				if (AFComment.canCreate(this))
					this._allowanceFields.Add(new AFComment(this));

				if (AFLocation.canCreate(this))
					this._allowanceFields.Add(new AFLocation(this));

				if (AFBreakfast.canCreate(this))
					this._allowanceFields.Add(new AFBreakfast(this));

				if (AFLunch.canCreate(this))
					this._allowanceFields.Add(new AFLunch(this));

				if (AFDinner.canCreate(this))
					this._allowanceFields.Add(new AFDinner(this));

				if (AFLodging.canCreate(this))
					this._allowanceFields.Add(new AFLodging(this));

				if (AFInfo.canCreate(this))
					this._allowanceFields.Add(new AFInfo(this));

				if (AFWorkNight.canCreate(this))
					this._allowanceFields.Add(new AFWorkNight(this));
			}

			return this._allowanceFields;
		}
	}
}