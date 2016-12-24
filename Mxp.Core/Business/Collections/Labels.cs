using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Mxp.Core.Services;
using Mxp.Core.Services.Responses;
using Mxp.Core.Utils;
using Mxp.Core.Helpers;

namespace Mxp.Core.Business
{
	public class Labels : SGCollection<Label>
	{
		public enum LabelEnum {
			Accept = 285,
			Accepted = 153,
			Add = 243,
			AddImage = 3631,
			Additional = 283,
			AddReceipt = 1344,
			Address = 131,
			Allowance = 593,
			Allowances = 593,
			Amount = 19,
			ApprovalReports = 3348,
			Approvals = 3349,
			ApprovalStatus = 343,
			Approve = 567,
			ArrivalAirport = 3338,
			ArrivalCountry = 3337,
			ArrivalDate = 192,
			ArrivalTime = 3336,
			Attendees = 3393,
			BackHome = 1664,
			Booked = 529,
			Breakfast = 597,
			BusinessDistance = 15,
			BusinessRelation = 3441,
			CalculatedDistance = 1783,
			Cancel = 4607,
			Cancelled = 874,
			CarRental = 858,
			Category = 42,
			Categories = 1522,
			ChooseFromLibrary = 2845,
			City = 77,
			Class = 268,
			Closed = 432,
			Comment = 82,
			CommentWrite = 3269,
			Commute = 550,
			Company = 7,
			CompanyFounded = 177,
			Confirmed = 873,
			ControlStatus = 344,
			CostCenter = 93,
			Country = 39,
			Countries = 3268,
			CreateAllowance = 3413,
			CreateExpense = 3265,
			CreateMileage = 3266,
			CreditCard = 3574,
			Currencies = 3276,
			Currency = 8,
			CurrencyDetails = 2793,
			CustomerCurrency = 2794,
			Date = 38,
			DateFrom = 812,
			DateTo = 813,
			Delete = 299,
			DoYouConfirm = 2323,
			DepartmentID = 93,
			DepartureAirport = 3341,
			DepartureCountry = 3340,
			DepartureDate = 191,
			DepartureTime = 3339,
			Details = 30,
			Dinner = 599,
			Distance = 560,
			Done = 2519,
			Draft = 527,
			DropCountry = 3347,
			DropDate = 1240,
			DropLocation = 1241,
			DropTime = 1251,
			Edit = 68,
			Employee = 680,
			Empty = 2349,
			EndDate = 661,
			EndPoint = 3270,
			ExchangeRate = 1078,
			Expense = 3262,
			Expenses = 1090,
			ExpenseInfo = 382,
			FillAllFields = 3015,
			FirstConnectionMessage = 1337,
			FirstName = 117,
			Flight = 3335,
			FromLibrary = 3399,
			FuelMileage = 417,
			Green = 3275,
			HcopSearchMandatory = 3501,
			Hco = 3439,
			Hcp = 3437,
			Hcu = 3438,
			History = 3575,
			HourFrom = 3414,
			HourTo = 3415,
			IconsLegend = 3576,
			Info = 1315,
			Invoiced = 528,
			InvoiceID = 78,
			ItemInfoChar1 = 1001,
			ItemInfoChar2 = 1002,
			ItemInfoChar3 = 1003,
			ItemInfoChar4 = 3217,
			ItemInfoChar5 = 3218,
			ItemInfoChar6 = 3219,
			ItemInfoChar7 = 3220,
			ItemInfoChar8 = 3221,
			ItemInfoNum1 = 1004,
			ItemInfoNum2 = 1005,
			Infochar1 = 1001,
			Infochar2 = 1002,
			Infochar3 = 1003,
			Infochar4 = 3217,
			Infochar5 = 3218,
			Infochar6 = 3219,
			Infochar7 = 3220,
			Infochar8 = 3221,
			Infonum1 = 1004,
			Infonum2 = 1005,
			ReportInfochar1 = 1271,
			ReportInfochar2 = 1272,
			ReportInfochar3 = 1273,
			ReportInfonum1 = 1274,
			ReportInfonum2 = 1275,
			LastName = 116,
			LeaveComment = 3350,
			LicencePlate = 3197,
			Loading = 3400,
			Location = 338,
			Lodging = 610,
			Login = 2314,
			Logout = 648,
			Lunch = 598,
			Mandatory = 2337,
			MandatoryCommentApproval = 2371,
			Merchant = 3344,
			MerchantName = 41,
			MerchantCity = 77,
			Mileage = 417,
			MileageImpossibleToCreate = 3402,
			MinLocations = 2268,
			Name = 1778,
			NameLabel = 134,
			NetAmount = 88,
			NewAllowance = 3416,
			NewAttendees = 1443,
			NewExpense = 3264,
			NewMileage = 3267,
			No = 371,
			NoInternetConnection = 3401,
			NoVehicle = 474,
			Npi = 3389,
			NumberNights = 275,
			OdometerFrom = 1284,
			OdometerTo = 1285,
			Open = 433,
			Orange = 3274,
			Parchar1 = 836,
			Parchar2 = 837,
			Parchar3 = 838,
			Parchar4 = 839,
			Parchar5 = 840,
			Parchar6 = 2056,
			Parchar7 = 2057,
			Parchar8 = 2058,
			Parchar9 = 2059,
			Parchar10 = 2060,
			Parint1 = 841,
			Parint2 = 842,
			Parint3 = 843,
			Parint4 = 844,
			Parint5 = 845,
			Parint6 = 2061,
			Parint7 = 2062,
			Parint8 = 2063,
			Parint9 = 2064,
			Parint10 = 2065,
			Paramount1 = 853,
			Paramount2 = 854,
			Paramount3 = 855,
			Paramount4 = 856,
			Paramount5 = 857,
			Paramount6 = 2071,
			Paramount7 = 2072,
			Paramount8 = 2073,
			Paramount9 = 2074,
			Paramount10 = 2075,
			Parind1 = 846,
			Parind2 = 847,
			Parind3 = 848,
			Parind4 = 851,
			Parind5 = 852,
			Parind6 = 2066,
			Parind7 = 2067,
			Parind8 = 2068,
			Parind9 = 2069,
			Parind10 = 2070,
			Password = 2801,
			PickupCountry = 3346,
			PickupDate = 1238,
			PickupLocation = 1239,
			PickupTime = 1250,
			PreferredHotel = 276,
			Prepayments = 1083,
			Private = 16,
			PrivatelyFunded = 66,
			ProjectID = 657,
			PullDownText = 3403,
			Quantity = 89,
			Recent = 3097,
			Recents = 3097,
			Receipt = 175,
			Receipts = 3404,
			Red = 3273,
			Refresh = 4611,
			Reject = 286,
			Rejected = 154,
			RememberMe = 3263,
			RequestTimeOut = 3628,
			Report = 1396,
			ReportExpensesMandatory = 3577,
			Reports = 1906,
			RequiredFields = 1338,
			Save = 406,
			Scanned = 1428,
			Search = 186,
			SearchMin3Letters = 1339,
			Segment = 2358,
			SelectAll = 3629,
			SelectNone = 3630,
			Settings = 1723,
			Settled = 348,
			Speciality = 3436,
			Split = 385,
			Splitted = 1725,
			SplitAmountExceeded = 1100,
			Spouse = 3440,
			StartDate = 659,
			StartPoint = 2821,
			State = 141,
			Status = 10,
			Stay = 3342,
			Submit = 62,
			Submitted = 22,
			TakeAPicture = 2846,
			Task = 80,
			Today = 552,
			Total = 1379,
			TotalAmount = 65,
			Transactions = 23,
			Transmission = 2435,
			Travel = 421,
			Traveller = 2832,
			TravelRequest = 3092,
			TravelRequestID = 449,
			Travels = 34,
			Type = 972,
			Unsplit = 1409,
			Username = 2800,
			VatAmount = 86,
			VatCode = 788,
			VatDetails = 2795,
			Vehicle = 107,
			VehicleCategory = 1242,
			Waiting = 523,
			WaitingForValidation = 3351,
			WorkNight = 3704,
			Website = 3833,
			WriteReceiptCode = 3632,
			Yes = 370,
			ZipCode = 140,

			// Icons legend
			ExpensePolicyGreen = 3561,
			ExpensePolicyOrange = 3562,
			ExpensePolicyRed = 3563,
			ExpenseReceiptBlack = 3564,
			ReportReceiptGreen = 3565,
			ReportReceiptRed = 3566,
			ReportReceiptBlack = 3567,
			ReportThumbGreen = 3568,
			ReportThumbRed = 3569,
			ReportPending = 3570,
			ReportPolicyGreen = 3571,
			ReportPolicyOrange = 3572,
			ReportPolicyRed = 3573,

			// Days
			Monday = 291,
			Tuesday = 292,
			Wednesday = 293,
			Thrusday = 294,
			Friday = 295,
			Saturday = 296,
			Sunday = 297,

			// Months
			January = 1364,
			February = 1365,
			March = 1366,
			April = 1367,
			May = 1368,
			June = 1369,
			July = 1370,
			August = 1371,
			September = 1372,
			October = 1373,
			November = 1374,
			December = 1375,

			UserAlreadyAuthenticated = 4301,
			ErrorXamarin = 4302,
			MileageMaxSegment = 4303,
			MileageMinSegment = 4304,
			AddSplitItem = 4305,
			RemoveUnauthorized = 4306,
			SelectJourney = 4307,
			FetchPlaceError = 4308,
			Invalid = 4309,
			Unauthorized = 4310,
			Preparing = 4311,
			Saving = 4312,
			Deleting = 4313,
			SegmentDetails = 4314,
			Segments = 4315,
			Approving = 4316,
			Actions = 4317,
			Submitting = 4318,
			Unsplitting = 4319,
			Splitting = 4320,
			Compressing = 4321,
			Uploading = 4322,
			Cancelling = 4323,
			Connexion = 4324,
			AddCurrentLocation = 4325,
			LocationServiceDisabled = 4326,
			FirstConnexion = 3398,
			ErrorValidation = 1234,
			MileageSegmentValidation = 2268,
			ChooseAttendee = 3442,
			ChooseExpense = 3721,
			General = 1701,
			New = 2969,
			MissingExpense = 2837,
			Close = 438,
			Authenticate = 807,
			Next = 96,
			Previous = 95,
			CreateReport = 1467,
			CancelReport = 1252,
			MileageInfo = 586,
			Advanced = 1235,
			Select = 829,
			AddExpense = 2343,
			ShowMap = 3799,
			NoSelection = 377,
			SpendCatcher = 4612,
			SpendCatcherHeaderMessage = 4613,
			SpendCatcherDisabledMessage = 4614,
			SpendCatcherConfirmationMessage = 4615,
			SpendCatcherOpenApp = 4616,
			GTPHeaderMessage = 4608,
			HCPSearhResult = 1892,
			ChangeExpenseToPrivate = 652,
			ChangeExpenseToBusiness = 651,
			TravelFilter = 6010,
			NonTravelFilter = 6011,
			SaveAndCopy = 3741,
			SpendCatcherCheckbox = 3678,
        }

		public Dictionary<int, string> UserLabels { get; set; }

		public Labels () {
			this.UserLabels = new Dictionary<int, string> ();
		}

		public async override Task FetchAsync () {
			await SystemService.Instance.FetchLabelsAsync (this);
		}

		public string GetLabel (int key) {
			try {
				return this.UserLabels [key];
			} catch (Exception) {
				return ((LabelEnum)key).ToString ();
			}
		}

		public string GetLabel (LabelEnum label) {
			return this.GetLabel ((int)label);
		}

		public string GetDayOfWeekLabel (int dayOfWeek) {
			if (dayOfWeek == 0)
				return this.GetLabel (LabelEnum.Sunday);

			return this.GetLabel (290 + dayOfWeek);
		}

		public string GetMonthLabel (int month) {
			return this.GetLabel (1363 + month);
		}

		public string GetLabel (DynamicFieldHolder field) {
			bool isReport = field.LocationName == DynamicFieldHolder.LocationEnum.Report;
			switch (field.LinkName) {
				case "Infochar1":
					return this.GetLabel (isReport ? Labels.LabelEnum.ReportInfochar1 : Labels.LabelEnum.Infochar1);
				case "Infochar2":
					return this.GetLabel (isReport ? Labels.LabelEnum.ReportInfochar2 : Labels.LabelEnum.Infochar2);
				case "Infochar3":
					return this.GetLabel (isReport ? Labels.LabelEnum.ReportInfochar3 : Labels.LabelEnum.Infochar3);
				case "Infochar4":
					return this.GetLabel (Labels.LabelEnum.Infochar4);
				case "Infochar5":
					return this.GetLabel (Labels.LabelEnum.Infochar5);
				case "Infochar6":
					return this.GetLabel (Labels.LabelEnum.Infochar6);
				case "Infochar7":
					return this.GetLabel (Labels.LabelEnum.Infochar7);
				case "Infochar8":
					return this.GetLabel (Labels.LabelEnum.Infochar8);
				case "Infonum1":
					return this.GetLabel (isReport ? Labels.LabelEnum.ReportInfonum1 : Labels.LabelEnum.Infonum1);
				case "Infonum2":
					return this.GetLabel (isReport ? Labels.LabelEnum.ReportInfonum2 : Labels.LabelEnum.Infonum2);
			}

			return field.LinkDescription;
		}

		public static Labels Instance {
			get {
				return LoggedUser.Instance.Labels;
			}
		}

		public static string GetLoggedUserLabel (LabelEnum label) {
			return Labels.Instance.GetLabel (label);
		}

		public IEnumerable<Response> rawResponses;
		public override void Populate (IEnumerable<Response> collection) {
			this.rawResponses = collection;
			base.Populate (collection);
			this.UserLabels.Clear ();
			IconsLegend.Reset ();
			this.ForEach (label => this.UserLabels.Add (label.DictionaryId, label.DictionaryLabel));
			this.Clear ();
		}
	}
}