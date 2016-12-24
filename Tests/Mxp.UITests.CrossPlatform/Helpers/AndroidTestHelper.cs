using System;
using System.Linq;
using Mxp.UITests.CrossPlatform.Helpers.Requests;
using Xamarin.UITest;

namespace Mxp.UITests.CrossPlatform.Helpers
{
	public class AndroidTestHelper : TestHelperBase
	{

		public AndroidTestHelper(IApp app) : base(app, Platform.Android)
		{
			object r = this._app.Invoke("CopyImageBackdoor");
		}

		public override void TapFirstCell()
		{
			this._app.Tap(x => x.Class("AppCompatTextView").Id("text1"));
		}

		public override void TapLoginButton()
		{
			this.TapButton(new TapButtonRequest { Marked = "Login" });
		}

		public override void EnterLoginData(EnterLoginDataRequest request)
		{
			this._app.EnterText(c => c.Marked("Username"), request.UserName);
			this._app.EnterText(c => c.Marked("Password"), request.Password);
		}

		public override void EnterValue<T>(EnterValueRequest<T> request)
		{
			this._app.ScrollDownTo(x => x.Marked(request.Label));

			this._app.Tap(x => x.Text(request.Label));

			if (!(request.Value is string))
			{
				this.EnterNumericValue(new EnterNumericValueRequest<T> { Value = request.Value });
			}
			else {
				var waitrequest = new WaitForElementRequest
				{
					Class = "DialogTitle",
					Id = "alertTitle"
				};

				this.WaitForElement(waitrequest);
				this._app.ClearText(x => x.Id("EditText"));
				this._app.EnterText(x => x.Id("EditText"), request.Value.ToString());
				this._app.Tap(x => x.Id("button1"));
			}

			this.WaitForElement(new WaitForElementRequest { Marked = request.Label });
		}

		private void EnterNumericValue<T>(EnterNumericValueRequest<T> request)
		{
			foreach (var digit in request.Value.ToString())
			{
				if ((digit == ',') || (digit == ','))
				{
					this._app.Tap(x => x.Id("fourth").Child(1));
					return;
				}

				this._app.Tap(x => x.Class("AppCompatButton").Text(digit.ToString()));
			}

			this._app.Tap(x => x.Id("set_button"));
		}

		private void EnterText2(EnterTextRequest request)
		{// todo: merge with generic Entertext. Check if it is textfield.
			if (string.IsNullOrEmpty(request.Text)) return;

			var query = this.GetElementQuery(new WaitForElementRequest { Id = request.Id, Text = request.ControlText });
			try
			{
				this._app.EnterText(query, request.Text);
				this.Wait(TimeSpan.FromSeconds(1));

				if (this._app.Query(x => x.Class("FrameLayout").Id("content")).Any())
				{
					this._app.Tap(x => x.Text("Ok"));
				}
			}
			catch (Exception e)
			{
				this._app.ScrollDownTo(query);
				this.EnterText2(request);
			}
		}

		public override void SelectValue(SelectValueRequest request)
		{
			SelectBase(new SelectBaseRequest
			{
				Label = request.Label,
				Value = request.Value,
				SearchControlId = "Search",
				SearchControlClass = "AppCompatEditText"
			});
		}

		public override void SelectAutocompleteValue(SelectAutocompleteValueRequest request)
		{
			this.SelectBase(new SelectBaseRequest
			{
				Label = request.Label,
				Value = request.Value,
				SearchControlId = "EditText",
				SearchControlClass = "AppCompatAutoCompleteTextView"
			});
		}

		private void SelectBase(SelectBaseRequest request)
		{
			string charactertosearch = string.Empty;

			this._app.ScrollDownTo(request.Label);

			this._app.Tap(x => x.Marked(request.Label));

			WaitForElementRequest waitrequest = new WaitForElementRequest
			{
				Class = request.SearchControlClass,
				Id = request.SearchControlId
			};

			this.WaitForElement(waitrequest);

			foreach (char currentchar in request.Value)
			{
				if (!char.IsLetter(currentchar))
				{
					if (string.IsNullOrEmpty(charactertosearch))
						continue;
					break;
				}

				charactertosearch += currentchar.ToString();
			}
			this.Wait(TimeSpan.FromSeconds(1));
			// sometimes the text is not fully entered, so we try several times
			Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery> searchcontrolquery = x => x.Class(request.SearchControlClass).Id(request.SearchControlId);
			while (!this._app.Query(x => x.Class("AppCompatTextView").Id("text1")).Any(x => x.Text.Contains(charactertosearch)))
			{
				this._app.ClearText(searchcontrolquery);
				this._app.EnterText(searchcontrolquery, charactertosearch);
				this.Wait(TimeSpan.FromSeconds(2));
			}

			this._app.ScrollDownTo(x => x.Marked(request.Value));
			this._app.Tap(x => x.Marked(request.Value));

			if (string.IsNullOrEmpty(request.WaitBackControlClass) && string.IsNullOrEmpty(request.SearchControlId))
				this.Wait(TimeSpan.FromSeconds(2));
			else
				this.WaitForElement(new WaitForElementRequest { Id = request.WaitBackSearchControlId, Class = request.WaitBackControlClass });
		}

		public override void EnterAutocompleteValue(EnterAutocompleteValueRequest request)
		{
			this._app.EnterText(x => x.Id("AutoCompleteTextView").Index(request.Index), request.Value);
			this.WaitForElement(new WaitForElementRequest { Class = "DropDownListView" });

			var i = 0;

			foreach (var result in this._app.Query(x => x.Class("DropDownListView").Child("AppCompatTextView").Id("text1")))
			{
				if (result.Text.StartsWith(request.Value))
					break;
				i++;
			}
			this._app.Tap(x => x.Class("DropDownListView").Child("AppCompatTextView").Id("text1").Index(i));
		}

		public override bool IsLogged()
		{
			this.WaitForElement(new WaitForElementRequest { ControlType = ControlType.Label, Text = "MobileXpense", ErrorMessage = "Error waiting for element" });
			return this.IsLabelElementExists("MobileXpense");
		}

		public override string CreateAnExpense(CreateAnExpenseRequest request)
		{
			// Go to expense tab
			this._app.Tap(x => x.Class("AppCompatTextView").Marked("Expenses"));
			this.Wait(TimeSpan.FromSeconds(2));

			this._app.Tap(x => x.Class("ActionMenuItemView").Id("Action_new"));

			this.WaitForElement(new WaitForElementRequest { Marked = "Mileage" });
			this._app.Tap(x => x.Text("Expense"));
			// Select first expense type

			this._app.ScrollDownTo(x => x.Marked(request.Category));
			this._app.EnterText(x => x.Class("AppCompatEditText").Id("Search"), request.Category);
			this._app.Tap(x => x.Id("text1").Marked(request.Category));
			this.WaitForElement(new WaitForElementRequest
			{
				Class = "DialogTitle",
				Marked = "Pickup country"
			});
			this._app.ScrollDownTo(x => x.Marked(request.Country));
			this._app.Tap(x => x.Text(request.Country));

			var amountrequest = new EnterValueRequest<decimal>
			{
				Label = "* Amount",
				Value = request.Amount
			};
			this.EnterValue(amountrequest);

			var currencyrequest = new SelectValueRequest
			{
				Label = "* Currency",
				Value = request.Currency
			};
			this.SelectValue(currencyrequest);

			var quantityrequest = new EnterValueRequest<decimal>
			{
				Label = "* Quantity",
				Value = request.Quantity
			};
			this.EnterValue(quantityrequest);

			this._app.Tap(x => x.Marked("Done"));

			this.WaitForElement(new WaitForElementRequest
			{
				Class = "AppCompatTextView",
				Marked = "Details"
			});

			//Add comment specific to be sure it is right expense we check in last step
			var timestamp = this.GetTimeStamp();

			if (request.IsCommentMandatory)
				this._app.Tap(x => x.Text("* Comment"));
			else
				this._app.Tap(x => x.Text("Comment"));

			this.Wait(TimeSpan.FromSeconds(1));
			this._app.EnterText(x => x.Id("EditText"), timestamp);
			this._app.Tap(x => x.Id("button1"));

			if (request.AddReceipt)
				this.AddReceiptToExpense();

			this.SaveData(Keys.ExpenseComment, timestamp);

			return timestamp;
		}

		public override void AddReceiptToExpense()
		{
			this._app.Tap(x => x.Text("Receipts"));
			this.SelectImage();
		}

		public override void SaveExpense()
		{
			this._app.Tap(x => x.Class("AppCompatTextView").Index(1));
			this.WaitForElement(new WaitForElementRequest { Marked = "Settings" });
		}

		public override bool ExpenseIsSaved(ExpenseIsSavedRequest request)
		{
			var timestamp = string.IsNullOrEmpty(request.ExpenseComment) ? this.GetData<string>(Keys.ExpenseComment) : request.ExpenseComment;

			this.WaitForElement(new WaitForElementRequest
			{
				Class = "AppCompatTextView",
				Id = "AmountLC"
			});
			this._app.Tap(x => x.Class("AppCompatTextView").Id("AmountLC"));
			this.WaitForElement(new WaitForElementRequest { Marked = "Expense", ErrorMessage = "Error finding expense" });
			return this._app.Query(c => c.Marked(timestamp)).Any();
		}

		public override string CreateAReport()
		{
			this._app.Tap(x => x.Class("AppCompatTextView").Marked("Reports"));
			this.Wait(TimeSpan.FromSeconds(1));
			this._app.Tap(x => x.Id("Action_new"));

			//Add comment specific to be sure it is right expense we check in last step
			var timestamp = this.GetTimeStamp();

			this.WaitForElement(new WaitForElementRequest
			{
				Class = "AppCompatTextView",
				Text = "* Name"
			});
			this._app.Tap(x => x.Class("AppCompatTextView").Text("* Name"));

			this.WaitForElement(new WaitForElementRequest
			{
				Class = "AppCompatEditText",
				Id = "EditText"
			});

			this._app.EnterText(x => x.Id("EditText"), timestamp);
			this._app.Tap(x => x.Id("button1"));
			this.WaitForElement(new WaitForElementRequest
			{
				Class = "AppCompatTextView",
				Text = "* Name"
			});
			this._app.Tap(x => x.Class("AppCompatTextView").Text("Expenses").Marked("Expenses"));

			// Select first expense
			this._app.Tap(x => x.Class("AppCompatCheckBox"));

			this.SaveData(Keys.ReportComment, timestamp);

			return timestamp;
		}

		public override void AddReceiptToReport()
		{
			this._app.Tap(x => x.Text("Receipts"));
			this.SelectImage();
			this.Wait(TimeSpan.FromSeconds(1));
		}

		public override void SaveReport()
		{
			this._app.Tap(x => x.Id("Actionbar_done"));
	
			this._app.WaitForElement(x => x.Id("alertTitle"));
			this._app.WaitForNoElement(x => x.Id("alertTitle"));
			this._app.WaitForElement(x => x.Marked("Navigate up"));
			this._app.Tap(x => x.Marked("Navigate up"));
			this._app.WaitForElement(x => x.Text("MobileXpense"));
		}

		public override bool ReportIsSaved(ReportIsSavedRequest request)
		{
			// Check if there is a report with the timestamp
			var timestamp = string.IsNullOrEmpty(request.ReportComment) ? this.GetData<string>(Keys.ReportComment) : request.ReportComment;

			// Wait 2 seconds before trying to find the report
			this.Wait(TimeSpan.FromSeconds(2));

			this._app.ScrollDownTo(timestamp);

			return this._app.Query(x => x.Marked(timestamp)).Any();
		}

		public override void AddExpenseToReport(AddExpenseToReportRequest request)
		{
			var timestamp = string.IsNullOrEmpty(request.ReportComment) ? this.GetData<string>(Keys.ReportComment) : request.ReportComment;
			// Get to reports tab
			if (!this._app.Query(x => x.Marked("MobileXpense")).Any())
			{
				this._app.Tap(x => x.Id("Actionbar_done"));
				this.WaitForElement(new WaitForElementRequest { Marked = "MobileXpense" });
			}
			this._app.Tap(x => x.Class("AppCompatTextView").Marked("Reports"));
			this.Wait(TimeSpan.FromSeconds(1));//todo: check if can reduce it

			this._app.ScrollDownTo(timestamp);
			this._app.Tap(x => x.Marked(timestamp));
			// Select expenses screen
			WaitForElement(new WaitForElementRequest { Class = "ImageButton", Marked = "Navigate up" });
			this._app.Tap(x => x.Marked("Expenses"));

			this.WaitForElement(new WaitForElementRequest { Marked = "Add expense" });
			// Add expense
			this._app.Tap(x => x.Marked("Add expense"));

			this.WaitForElement(new WaitForElementRequest
			{
				Class = "DialogTitle",
				Text = "Expenses"
			});

			this._app.Tap(x => x.Class("AppCompatCheckBox"));
			this._app.Tap(x => x.Id("button1").Text("Done"));
			this.WaitForElement(new WaitForElementRequest { Marked = "Navigate up" });

			this._app.Tap(x => x.Marked("Navigate up"));
			this._app.WaitForElement(x => x.Text("MobileXpense"));
		}

		public override bool ReportExpenseReceiptSaved(ReportExpenseReceiptSavedRequest request)
		{
			var timestamp = string.IsNullOrEmpty(request.Reportcomment) ? this.GetData<string>(Keys.ReportComment) : request.Reportcomment;
			// Select report with timestamp and go to expenses
			this._app.ScrollDownTo(x => x.Marked(timestamp));
			this._app.Tap(x => x.Marked(timestamp));
			this.WaitForElement(new WaitForElementRequest { Marked = "Details" });
			this._app.Tap(x => x.Class("AppCompatTextView").Text("Expenses"));
			this.WaitForElement(new WaitForElementRequest { Marked = "Add expense" });

			// Select first expense
			this._app.Tap(x => x.Class("RelativeLayout").Descendant().Id("AmountLC"));
			// check if there is one receipt linked
			this.WaitForElement(new WaitForElementRequest { Id = "Action_split" });
			this._app.Tap(x => x.Class("AppCompatTextView").Text("Receipts"));
			this.WaitForElement(new WaitForElementRequest { Id = "Image" });
			return this._app.Query(x => x.Id("Image")).Any();
		}

		public override void SelectExpense(SelectExpenseRequest request)
		{
			// navigate up (can have 2 level, so try 2 times)
			if (this._app.Query(x => x.Marked("Navigate up")).Any())
			{
				this._app.Tap(x => x.Marked("Navigate up"));
			}
			if (this._app.Query(x => x.Marked("Navigate up")).Any())
			{
				this._app.Tap(x => x.Marked("Navigate up"));
			}

			this._app.Tap(x => x.Class("AppCompatTextView").Marked("Expenses"));
			this.WaitForElement(new WaitForElementRequest
			{
				Class = "AppCompatTextView",
				Id = "AmountLC"
			});
			this._app.Tap(x => x.Class("AppCompatTextView").Id("AmountLC"));
			this.WaitForElement(new WaitForElementRequest { Marked = request.ExpenseComment, ErrorMessage = "Error finding expense" });
		}

		public override void ToggleSwitch(ToggleSwitchRequest request)
		{
			try
			{
				this._app.ScrollDownTo(x => x.Class("AppCompatTextView").Text(request.Label));
			}
			catch (Exception e)
			{
				this._app.ScrollUpTo(x => x.Class("AppCompatTextView").Text(request.Label));
			}

			this._app.Tap(x => x.Class("AppCompatTextView").Text(request.Label).Sibling(0));
		}

		public override bool GetSwitchState(GetSwitchStateRequest request)
		{
			this.WaitForElement(new WaitForElementRequest
			{
				Class = "AppCompatTextView",
				Text = request.Label
			});

			return this._app.Query(x => x.Class("AppCompatTextView").Text(request.Label).Sibling(0).Property("checked").Value<bool>()).FirstOrDefault();
		}

		public override bool GetIconState(GetIconStateRequest request)
		{
			var suffixlabel = request.Label + "Icon";
			this.WaitForElement(new WaitForElementRequest
			{
				Class = "AppCompatCheckBox",
				Id = suffixlabel
			});

			return this._app.Query(x => x.Class("AppCompatCheckBox").Id(suffixlabel).Property("checked").Value<bool>()).FirstOrDefault();
		}

		public override void DeleteExpense()
		{
			this._app.Tap(x => x.Id("Action_delete"));
			this._app.Tap(x => x.Id("button1"));
			this.WaitForElement(new WaitForElementRequest { ControlType = ControlType.Label, Text = "MobileXpense", ErrorMessage = "Error waiting for element" });
		}

		public override void CancelExpense()
		{
			this._app.Tap(x => x.Id("Actionbar_cancel"));
			this.WaitForElement(new WaitForElementRequest { ControlType = ControlType.Label, Text = "MobileXpense", ErrorMessage = "Error waiting for element" });
		}

		public override void DeleteAllowance()
		{
			this.DeleteExpense();
		}

		public override void DeleteMilleagee()
		{
			this.DeleteExpense();
		}

		public override bool ExpenseIsDeleted(ExpenseIsDeletedRequest request)
		{
			var timestamp = string.IsNullOrEmpty(request.ExpenseComment) ? this.GetData<string>(Keys.ExpenseComment) : request.ExpenseComment;
			this.WaitForElement(new WaitForElementRequest
			{
				Class = "AppCompatTextView",
				Id = "AmountLC"
			});
			this._app.Tap(x => x.Class("AppCompatTextView").Id("AmountLC"));
			this.WaitForElement(new WaitForElementRequest
			{
				Class = "AppCompatTextView",
				Marked = "Details"
			});
			return !this._app.Query(c => c.Marked(timestamp)).Any();
		}

		public override void SelectReport(SelectReportRequest request)
		{
			// navigate up (can have 2 level, so try 2 times)
			if (this._app.Query(x => x.Marked("Navigate up")).Any())
			{
				this._app.Tap(x => x.Marked("Navigate up"));
				this.Wait(TimeSpan.FromSeconds(1));
			}
			if (this._app.Query(x => x.Marked("Navigate up")).Any())
			{
				this._app.Tap(x => x.Marked("Navigate up"));
				this.Wait(TimeSpan.FromSeconds(1));
			}
			var timestamp = string.IsNullOrEmpty(request.ReportComment) ? this.GetData<string>(Keys.ReportComment) : request.ReportComment;
			this._app.Tap(x => x.Class("AppCompatTextView").Marked("Reports"));
			this.Wait(TimeSpan.FromSeconds(1));

			this._app.ScrollDownTo(timestamp);
			this._app.Tap(x => x.Marked(timestamp));
		}

		public override void DeleteReport()
		{
			this._app.Tap(x => x.Id("Action_delete"));
			this._app.Tap(x => x.Id("button1"));
			this.WaitForNoElement(new WaitForElementRequest { ControlType = ControlType.Label, Text = "MobileXpense", ErrorMessage = "Error waiting for element" });
		}

		public override bool ReportIsDeleted(ReportIsDeletedRequest request)
		{
			var timestamp = string.IsNullOrEmpty(request.ReportComment) ? this.GetData<string>(Keys.ReportComment) : request.ReportComment;
			try
			{
				this._app.ScrollDownTo(x => x.Marked(timestamp));
			}
			catch (Exception e)
			{
				return true;
			}

			return false;
		}

		public override string CreateAMilleage(CreateAMilleageRequest request)
		{
			this._app.Tap(x => x.Text("Expenses"));
			this._app.Tap(x => x.Class("ActionMenuItemView").Id("Action_new"));
			this.WaitForElement(new WaitForElementRequest { Marked = "Mileage" });
			this._app.Tap(x => x.Text("Mileage"));
			this.WaitForElement(new WaitForElementRequest { Id = "AutoCompleteTextView" });

			var startcityrequest = new EnterAutocompleteValueRequest
			{
				Value = request.StartCity
			};
			this.EnterAutocompleteValue(startcityrequest);

			var endcityrequest = new EnterAutocompleteValueRequest
			{
				Value = request.EndCity,
				Index = 1
			};
			this.EnterAutocompleteValue(endcityrequest);

			this._app.ScrollDownTo("Comment");
			this._app.Tap(x => x.Text("Comment"));
			this.WaitForElement(new WaitForElementRequest { Id = "EditText" });

			var timestamp = this.GetTimeStamp();
			this._app.EnterText(x => x.Id("EditText"), timestamp);
			this._app.Tap(x => x.Id("button1"));

			this._app.ScrollUpTo(x => x.Marked("Segment details"));

			this.SaveData(Keys.MileageComment, timestamp);
			return timestamp;
		}

		public override void SaveMileage()
		{
			this._app.Tap(x => x.Class("AppCompatTextView").Index(1));

			this.WaitForElement(new WaitForElementRequest { Marked = "Expenses" });
		}

		public override bool MileageIsSaved(MileageIsSavedRequest request)
		{
			var timestamp = string.IsNullOrEmpty(request.MileageComment) ? this.GetData<string>(Keys.MileageComment) : request.MileageComment;

			this._app.Tap(x => x.Id("AmountLC"));
			this.WaitForElement(new WaitForElementRequest { Id = "AutoCompleteTextView" });

			try
			{
				this._app.ScrollDownTo(timestamp);
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}

		public override int GetDistance(GetDistanceRequest request)
		{
			var getvaluerequest = new GetValueRequest
			{
				Label = request.Label
			};
			var stringvalue = this.GetValue(getvaluerequest);

			int cellvalue;

			if (int.TryParse(stringvalue, out cellvalue))
			{
				return cellvalue;
			}

			return -1;
		}

		public override void SetDistance(SetDistanceRequest request)
		{
			try
			{
				this._app.ScrollDownTo(x => x.Class("AppCompatTextView").Text(request.Label));
			}
			catch (Exception e)
			{
				this._app.ScrollUpTo(x => x.Class("AppCompatTextView").Text(request.Label));
			}

			var stringkm = request.KM.ToString();
			this._app.Tap(x => x.Text(request.Label));
			WaitForElement(new WaitForElementRequest { Id = "number_picker" });

			foreach (var digit in stringkm)
			{
				this._app.Tap(x => x.Text(digit.ToString()));
				this.Wait(TimeSpan.FromSeconds(0.5));
			}

			this.Wait(TimeSpan.FromSeconds(1));

			this._app.Tap(x => x.Id("set_button"));

			this._app.WaitForNoElement(x => x.Id("set_button"));

			this.Wait(TimeSpan.FromSeconds(1));
		}

		public override string CreateAllowance(CreateAllowanceRequest request)
		{
			this._app.Tap(x => x.Text("Expenses"));
			this._app.Tap(x => x.Class("ActionMenuItemView").Id("Action_new"));
			this.WaitForElement(new WaitForElementRequest { Marked = "Allowances" });
			this._app.Tap(x => x.Text("Allowances"));

			this.WaitForElement(new WaitForElementRequest { Marked = "* Country" });
			this._app.Tap(x => x.Text("* Country"));
			this._app.ScrollDownTo(request.Country);
			this._app.Tap(x => x.Text(request.Country));

			this.WaitForElement(new WaitForElementRequest { Marked = "* Country" });
			this._app.Tap(x => x.Text("Comment"));

			var timestamp = this.GetTimeStamp();
			this.WaitForElement(new WaitForElementRequest { Id = "EditText" });
			this._app.EnterText(x => x.Id("EditText"), timestamp);
			this._app.Tap(x => x.Id("button1"));

			this.SaveData(Keys.AllowanceComment, timestamp);
			return timestamp;
		}

		public override void SaveAllowance()
		{
			var donebuttonpresent = this._app.Query(x => x.Id("button1").Text("Done")).Any();

			if (donebuttonpresent)
			{
				// done button
				this._app.Tap(x => x.Id("button1"));
			}

			this.WaitForElement(new WaitForElementRequest { Id = "Actionbar_done" });
			this._app.Tap(x => x.Class("AppCompatTextView").Index(1));
		}

		public override bool AllowanceIsSaved(AllowanceIsSavedRequest request)
		{
			var timestamp = string.IsNullOrEmpty(request.AllowanceComment) ? this.GetData<string>(Keys.AllowanceComment) : request.AllowanceComment;

			this._app.Tap(x => x.Id("AmountLC"));

			try
			{
				this.WaitForElement(new WaitForElementRequest { Marked = timestamp });
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}

		public override void SelectFirstMileage()
		{
			this._app.Tap(x => x.Id("AmountLC"));
			this.WaitForElement(new WaitForElementRequest { Id = "AutoCompleteTextView" });
		}

		public override string GetValue(GetValueRequest request)
		{
			try
			{
				this._app.ScrollDownTo(x => x.Marked(request.Label));
			}
			catch (Exception e)
			{
				this._app.ScrollUpTo(x => x.Marked(request.Label));
			}
			this.Wait(TimeSpan.FromSeconds(1)); // Seems sometimes text returns "", but it goes better after a pause
			return this._app.Query(x => x.Class("AppCompatTextView").Marked(request.Label).Sibling(0)).First().Text;
		}

		public override void AddBusinessRelation(AddBusinessRelationRequest request)
		{
			this._app.Tap(x => x.Text("Attendees"));
			this.WaitForElement(new WaitForElementRequest
			{
				Class = "ActionMenuItemView",
				Id = "Action_new"
			});
			this._app.Tap(x => x.Class("ActionMenuItemView").Id("Action_new"));

			this.WaitForElement(new WaitForElementRequest
			{
				Class = "AppCompatTextView",
				Text = "Business Relation"
			});
			this._app.Tap(x => x.Marked("Business Relation"));

			this.WaitForElement(new WaitForElementRequest { Class = "DialogTitle" });

			this.EnterText2(new EnterTextRequest { ControlText = "First name", Text = request.FirstName });
			this.EnterText2(new EnterTextRequest { ControlText = "* Last name", Text = request.LastName });
			this.EnterText2(new EnterTextRequest { ControlText = "* Company", Text = request.Company });

			this._app.Tap(x => x.Id("button1"));

			this.WaitForNoElement(new WaitForElementRequest { Class = "DialogTitle" });
		}

		public override void AddSpouse(AddSpouseRequest request)
		{
			this._app.Tap(x => x.Text("Attendees"));
			this.WaitForElement(new WaitForElementRequest
			{
				Class = "ActionMenuItemView",
				Id = "Action_new"
			});
			this._app.Tap(x => x.Class("ActionMenuItemView").Id("Action_new"));

			this.WaitForElement(new WaitForElementRequest
			{
				Class = "AppCompatTextView",
				Text = "Business Relation"
			});
			this._app.Tap(x => x.Marked("Spouse"));

			this.WaitForElement(new WaitForElementRequest { Class = "DialogTitle" });
			this.EnterText2(new EnterTextRequest { ControlText = "First name", Text = request.FirstName });
			this.EnterText2(new EnterTextRequest { ControlText = "* Last name", Text = request.LastName });

			this._app.Tap(x => x.Id("button1"));

			this.WaitForNoElement(new WaitForElementRequest { Class = "DialogTitle" });
		}

		public override void AddHealthCareProvider(AddHealthCareProviderRequest request)
		{
			this._app.Tap(x => x.Text("Attendees"));
			this.WaitForElement(new WaitForElementRequest
			{
				Class = "ActionMenuItemView",
				Id = "Action_new"
			});
			this._app.Tap(x => x.Class("ActionMenuItemView").Id("Action_new"));

			this.WaitForElement(new WaitForElementRequest
			{
				Class = "AppCompatTextView",
				Text = "Business Relation"
			});
			this._app.Tap(x => x.Marked("Health Care Provider"));

			this.WaitForElement(new WaitForElementRequest { Class = "DialogTitle" });
			this.EnterText2(new EnterTextRequest { ControlText = "Firstn ame", Text = request.FirstName });
			this.EnterText2(new EnterTextRequest { ControlText = "Last name", Text = request.LastName });
			this.EnterText2(new EnterTextRequest { ControlText = "Zip", Text = request.Zip });
			this.EnterText2(new EnterTextRequest { ControlText = "City/Town", Text = request.City });
			this.EnterText2(new EnterTextRequest { ControlText = "* State", Text = request.State });
			this.EnterText2(new EnterTextRequest { ControlText = "Speciality", Text = request.Speciality });
			this.EnterText2(new EnterTextRequest { ControlText = "Npi", Text = request.NPINumber });

			this._app.Tap(x => x.Id("button1"));

			this.WaitForElement(new WaitForElementRequest { Id = "Location" });

			this._app.Tap(x => x.Class("ListView").Descendant("LinearLayout"));

			this.WaitForNoElement(new WaitForElementRequest { Class = "DialogTitle" });
		}

		public override void AddHealthCareOrganization(AddHealthCareOrganizationRequest request)
		{
			this._app.Tap(x => x.Text("Attendees"));
			this.WaitForElement(new WaitForElementRequest
			{
				Class = "ActionMenuItemView",
				Id = "Action_new"
			});
			this._app.Tap(x => x.Class("ActionMenuItemView").Id("Action_new"));

			this.WaitForElement(new WaitForElementRequest
			{
				Class = "AppCompatTextView",
				Text = "Business Relation"
			});
			this._app.Tap(x => x.Marked("Health Care Organization"));

			this.WaitForElement(new WaitForElementRequest { Class = "DialogTitle" });

			this.EnterText2(new EnterTextRequest { ControlText = "Company", Text = request.Company });
			this.EnterText2(new EnterTextRequest { ControlText = "Zip", Text = request.Zip });
			this.EnterText2(new EnterTextRequest { ControlText = "City/Town", Text = request.City });
			this.EnterText2(new EnterTextRequest { ControlText = "* State", Text = request.State });
			this.EnterText2(new EnterTextRequest { ControlText = "Speciality", Text = request.Speciality });
			this.EnterText2(new EnterTextRequest { ControlText = "Npi", Text = request.NPINumber });

			this._app.Tap(x => x.Id("button1"));

			this.WaitForElement(new WaitForElementRequest { Id = "Location" });

			this._app.Tap(x => x.Class("ListView").Descendant("LinearLayout"));

			this.WaitForNoElement(new WaitForElementRequest { Class = "DialogTitle" });
		}

		public override void AddUnrecognizedHealthCareProvider(AddUnrecognizedHealthCareProviderRequest request)
		{
			this._app.Tap(x => x.Text("Attendees"));
			this.WaitForElement(new WaitForElementRequest
			{
				Class = "ActionMenuItemView",
				Id = "Action_new"
			});
			this._app.Tap(x => x.Class("ActionMenuItemView").Id("Action_new"));

			this.WaitForElement(new WaitForElementRequest
			{
				Class = "AppCompatTextView",
				Text = "Business Relation"
			});
			this._app.Tap(x => x.Marked("Unrecognized Health Care Provider"));

			this.WaitForElement(new WaitForElementRequest { Class = "DialogTitle" });

			this.EnterText2(new EnterTextRequest { ControlText = "* First name", Text = request.FirstName });
			this.EnterText2(new EnterTextRequest { ControlText = "* Last name", Text = request.LastName });
			this.EnterText2(new EnterTextRequest { ControlText = "Company", Text = request.Company });
			this.EnterText2(new EnterTextRequest { ControlText = "Address", Text = request.Address });
			this.EnterValue<int>(new EnterValueRequest<int> { Label = "Zip", Value = int.Parse(request.Zip) });
			this.EnterText2(new EnterTextRequest { ControlText = "* City/Town", Text = request.City });
			this.EnterText2(new EnterTextRequest { ControlText = "State", Text = request.State });
			this.EnterText2(new EnterTextRequest { ControlText = "Speciality", Text = request.Speciality });
			this.EnterText2(new EnterTextRequest { ControlText = "NPI Number", Text = request.NPINumber });

			this._app.Tap(x => x.Id("button1"));

			this.WaitForNoElement(new WaitForElementRequest { Class = "DialogTitle" });
		}

		public override int GetListCount(GetListCountRequest request)
		{
			int count;

			if (!string.IsNullOrEmpty(request.Id))
			{
				count = this._app.Query(x => x.Class("ListView").Id(request.Id).Invoke("getAdapter").Invoke("getCount").Value<int>()).FirstOrDefault();
			}
			else {
				count = this._app.Query(x => x.Class("ListView").Invoke("getAdapter").Invoke("getCount").Value<int>()).FirstOrDefault();
			}

			return count;
		}

		public override int GetAttendeeCount(GetAttendeeCountRequest request)
		{
			this._app.Tap(x => x.Text("Attendees"));
			this.WaitForElement(new WaitForElementRequest
			{
				Class = "ActionMenuItemView",
				Id = "Action_new"
			});
			return this.GetListCount(new GetListCountRequest());
		}

		public override void PullToRefresh()
		{
			this.WaitForElement(new WaitForElementRequest { Class = "WrapperView" });
			var rect = this._app.Query(x => x.Class("WrapperView")).First().Rect;
			this._app.DragCoordinates(rect.CenterX, rect.Y + 20, rect.CenterX, rect.Height * 3);
			this.WaitForElement(new WaitForElementRequest { Class = "CircleImageView" });
			this.WaitForNoElement(new WaitForElementRequest { Class = "CircleImageView" });
		}

		public override bool CheckErrorDialogShowing()
		{
			return this._app.Query(x => x.Class("alertTitle").Text("Error")).Any();
		}

		public override bool CheckIconVisibilityAndCounter(CheckIconVisibilityAndCounterRequest request)
		{
			return this._app.Query(x => x.Class("ImageView").Id(request.Id)).FirstOrDefault() != null;
		}

		public override bool CheckAttendeeIcon(CheckAttendeeIconRequest request)
		{
			this._app.Tap(x => x.Class("ImageButton").Marked("Navigate up"));
			this.WaitForElement(new WaitForElementRequest { Marked = "MobileXpense" });
			this._app.Tap(x => x.Class("AppCompatTextView").Marked("Expenses"));
			this.Wait(TimeSpan.FromSeconds(2));//todo: check if can reduce it
			return this.CheckIconVisibilityAndCounter(new CheckIconVisibilityAndCounterRequest { Id = "AttendeeIcon" });
		}

		public override bool CheckReceiptIcon(CheckReceiptIconRequest request)
		{
			this._app.Tap(x => x.Class("ImageButton").Marked("Navigate up"));
			this.WaitForElement(new WaitForElementRequest { Marked = "MobileXpense" });
			this._app.Tap(x => x.Class("AppCompatTextView").Marked("Expenses"));
			this.Wait(TimeSpan.FromSeconds(2));//todo: check if can reduce it
			return this.CheckIconVisibilityAndCounter(new CheckIconVisibilityAndCounterRequest { Id = "ReceiptIcon" });
		}

		private void SelectImage()
		{
			this._app.Invoke("AddReicept");
		}
	}
}
