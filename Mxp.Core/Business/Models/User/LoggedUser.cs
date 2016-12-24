using System;
using System.Threading.Tasks;

using Mxp.Core.Services;
using System.Linq;
using Xamarin.Forms;

namespace Mxp.Core.Business
{
	public partial class LoggedUser : User
	{
		public event EventHandler<AlertMessage.AlertMessageEventArgs> AlertMessageChanged;

		private static readonly string FILENAME = "LoggedUser";

		private const string NEED_CREDENTIALS_MESSAGE = "Please enter your credentials";

		private static LoggedUser _instance;
		public static LoggedUser Instance {
			get {
				if (_instance == null)
					Instance = new LoggedUser ();

				return _instance;
			}
			private set {
				_instance = value;
				if (_instance != null)
					LoggedUser.Instance.UnserializeStringFormat (ReadAndWriter.readFile (FILENAME));
			}
		}

		static ILoggedUserFileIO ReadAndWriter = DependencyService.Get<ILoggedUserFileIO> ();

		public bool IsSessionActive { get; set; }

		public override string Username {
			get {
				return this.username;
			}
			set {
				if (value != null && value.StartsWith ("staging")) {
					this.UserAPI = Service.DomainApiEnum.Staging;
					this.username = value.Substring (value.IndexOf (".") + 1);
				} else {
					if (value != null)
						this.UserAPI = Service.DomainApiEnum.Mob;
					this.username = value;
				}
			}
		}

		public string VUsername {
			get {
				if (String.IsNullOrWhiteSpace (this.Username))
					return String.Empty;
				
				return this.UserAPI.ApiForCredential () + this.Username;
			}
		}

		private string email;
		public string Email {
			get {
				return this.email;
			}
			set {
				if (value != null && value.StartsWith ("staging")) {
					this.UserAPI = Service.DomainApiEnum.Staging;
					this.email = value.Substring (value.IndexOf (".") + 1);
				} else {
					if (value != null)
						this.UserAPI = Service.DomainApiEnum.Mob;
					this.email = value;
				}
			}
		}

		public string VEmail {
			get {
				if (String.IsNullOrWhiteSpace (this.Email))
					return String.Empty;
				
				return this.UserAPI.ApiForCredential () + this.Email;
			}
		}

		private Service.DomainApiEnum _userAPI;
		public Service.DomainApiEnum UserAPI {
			get {
				return this._userAPI;
			}
			set {
				this._userAPI = value;
				Service.RefreshBaseUrl ();
			}
		}

		public string Password { get; set; }
		public string Token { get; set; }

		// Lazy fetch
		public Reports DraftReports { get; set; }
		public Reports OpenReports { get; set; }
		public Reports ClosedReports { get; set; }

		public Expenses BusinessExpenses { get; private set; }
		public Expenses PrivateExpenses { get; private set; }
		public SpendCatcherExpenses SpendCatcherExpenses { get; private set; }

		public TravelApprovals TravelApprovals { get; set; }
		public ReportApprovals ReportApprovals { get; set; }

		// Cached
		public Preferences Preferences { get; private set; }
		public Countries Countries { get; private set; }
		public Currencies Currencies { get; private set; }
		public Products Products { get; private set; }
		public Labels Labels { get; private set; }
		public VehicleCategories VehicleCategories { get; private set; }
		public Journeys FavouriteJourneys { get; private set; }

		private Currency _currency;
		public Currency Currency {
			get {
				if (this._currency == null && this.Preferences.FldCurrencyId != 0) {
					this._currency = this.Currencies.Single (currency => currency.Id == this.Preferences.FldCurrencyId);
				}

				return this._currency;
			}
		}

		private Country _country;
		public Country Country {
			get {
				if (this._country == null && this.Preferences.FldCountryId != 0)
					this._country = this.Countries.SingleOrDefault (country => country.Id == this.Preferences.FldCountryId);

				return this._country;
			}
		}

		public bool IsAuthenticated {
			get {
				return !String.IsNullOrEmpty (this.Token);
			}
		}

		public string ForgotPasswordUrl {
			get { 
				return Service.DomainApiEnum.Mob.GetRootUrl () + "HOME/MXP_loginforgotPW.asp?Forget=True&DefLanguage=&APP=MXP&Style=R5_Orange.css";
			}
		}

		private LoggedUser () {
			this.BusinessExpenses = new Expenses (Expenses.ExpensesTypeEnum.Business);
			this.PrivateExpenses = new Expenses (Expenses.ExpensesTypeEnum.Private);
			this.SpendCatcherExpenses = new SpendCatcherExpenses ();

			this.DraftReports = new Reports (Reports.ReportTypeEnum.Draft);
			this.OpenReports = new Reports (Reports.ReportTypeEnum.Open);
			this.ClosedReports = new Reports (Reports.ReportTypeEnum.Closed);
		
			this.ReportApprovals = new ReportApprovals ();
			this.TravelApprovals = new TravelApprovals ();

			this.Countries = new Countries ();
			this.Products = new Products ();
			this.Currencies = new Currencies ();
			this.Labels = new Labels ();
			this.VehicleCategories = new VehicleCategories ();
			this.FavouriteJourneys = new Journeys ();

			this.Preferences = new Preferences ();
		}

		public bool AutoLogin { get; set; }

		public bool CanAutoLogin {
			get {
				return this.AutoLogin
					       && (!String.IsNullOrWhiteSpace (this.Username) || !String.IsNullOrWhiteSpace (this.Email))
					       && this.IsAuthenticated;
			}
		}

		public bool CanLogin {
			get {
				return !String.IsNullOrWhiteSpace (this.Username) && !String.IsNullOrWhiteSpace (this.Password);
			}
		}

		public bool CanLoginByMail {
			get {
				return !String.IsNullOrWhiteSpace (this.Email);
			}
		}

		public async Task<string> LoginByMailAsync () {
			if (!this.CanLoginByMail)
				throw new ValidationError("Error", NEED_CREDENTIALS_MESSAGE);

			this.TrackContext.Load ();

			return await SSOService.Instance.GetRedirectionURLLoginByMailAsync ();
		}

		public async Task LoginAsync () {
			if (!this.CanLogin)
				throw new ValidationError("Error", NEED_CREDENTIALS_MESSAGE);

			await SSOService.Instance.LoginAsync ();

			this.TrackContext.Load ();
		}

		public async Task CheckTokenAsync () {
			try {
				await SSOService.Instance.CheckTokenAsync ();
			} catch (Exception) {
				this.ResetData ();
				throw;
			}
		}

		public async Task RefreshCacheAsync () {
			try {
				await Task.WhenAll (
					this.Preferences.FetchAsync (),
					this.Products.FetchAsync (),
					Task.Run (async () => {
						await this.Countries.FetchAsync ();

						if (this.Country == null)
							this._country = await Country.FetchAsync (this.Preferences.FldCountryId);
					}),
					this.Currencies.FetchAsync (),
					this.Labels.FetchAsync (),
					this.VehicleCategories.FetchAsync (),
					this.FavouriteJourneys.FetchAsync()
				);
				this.NotifyPropertyChanged("RefreshCacheAsync");
			} catch (Exception e) {
				LoggedUser.Instance = new LoggedUser ();
				throw e;
			}

			if (this.AutoLogin)
				ReadAndWriter.writeFile (FILENAME, this.SerializeStringFormat ());

			this.IsSessionActive = true;
		}

		public void ResetData () {
			ReadAndWriter.writeFile (FILENAME, this.SerializeStringFormat (false));
		}

		public void Logout () {
			ReadAndWriter.RemoveFile (FILENAME);
			LoggedUser.Instance = new LoggedUser ();
			this.NotifyPropertyChanged ("logout");
		}

		public Report GetReport (Reports.ReportTypeEnum type, int id) {
			switch (type) {
				case Reports.ReportTypeEnum.Draft:
					return this.DraftReports.Single (report => report.Id == id);
				case Reports.ReportTypeEnum.Open:
					Report found = this.OpenReports.SingleOrDefault (report => report.Id == id);
					return found == null ? this.DraftReports.Single (report => report.Id == id) : found;
				case Reports.ReportTypeEnum.Closed:
					return this.ClosedReports.Single (report => report.Id == id);
				case Reports.ReportTypeEnum.Approval:
					return this.ReportApprovals.Single (approval => approval.Report.Id == id).Report;
				default:
					return null;
			}
		}

		public string BrowserLink {
			get {
				return Service.BaseUrl + "home/i_LoginMobile.asp?SessionSharedKey=" + this.Token;
			}
		}

		public void NotifyMessageChanged (AlertMessage message) {
			this.AlertMessageChanged?.Invoke (this, new AlertMessage.AlertMessageEventArgs (message));
		}
	}
}