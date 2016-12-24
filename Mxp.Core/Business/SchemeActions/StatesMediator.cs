using System;
using System.Threading.Tasks;
using Mxp.Core.Services;

namespace Mxp.Core.Business
{
	public static class StateExtentions {
		public static void Execute (this StatesMediator.StateEnum state, StatesMediator instance) {
			switch (state) {
				case StatesMediator.StateEnum.NeedLogin:
					instance.RedirectToLogin ();
					break;
				case StatesMediator.StateEnum.AutoLogin:
					instance.AutoLogin ();
					break;
				case StatesMediator.StateEnum.Ready:
					instance.Invoke ();
					break;
			}
		}
	}

	public class StatesMediator
	{
		public interface IStateListener {
			void RedirectToLoginView (ValidationError error = null);
			void RedirectToMainView ();
			void RedirectToLoginByEmail (string redirection);
		}

		public class StateArgs {
			public StateEnum State { get; set; }

			public StateArgs (StateEnum state) {
				this.State = state;
			}
		}

		public enum StateEnum {
			Unknown,
			NeedLogin,
			AutoLogin,
			Ready
		}

		public event EventHandler<StateArgs> StateChanged;

		private void NotifyStateChanged () {
			EventHandler<StateArgs> stateChanged = this.StateChanged;

			if (stateChanged != null)
				stateChanged (this, new StateArgs (this.State));
		}

		private StateEnum _state = StateEnum.Unknown;
		public StateEnum State {
			get {
				return this._state;
			}
			set {
				this._state = value;
				this.NotifyStateChanged ();
				this.State.Execute (this);
			}
		}

		public ICommand Command { get; set; }

		private ICommandsFactory mCommandsFactory;
		private IStateListener mListener;

		public StatesMediator (ICommandsFactory commandsFactory, IStateListener listener) {
			this.mListener = listener;
			this.mCommandsFactory = commandsFactory;
		}

		public void ChangeState (string uri) {
			this.InitializeCommand (uri);

			if (this.Command is SAMLAbstractCommand || (LoggedUser.Instance.IsSessionActive && LoggedUser.Instance.IsAuthenticated))
				this.State = StateEnum.Ready;
			else if (!LoggedUser.Instance.CanAutoLogin)
				this.State = StateEnum.NeedLogin;
			else if (LoggedUser.Instance.CanAutoLogin)
				this.State = StateEnum.AutoLogin;
		}

		private void InitializeCommand (string uri) {
			if (uri == null)
				return;

			ICommand command = SchemeActionStrategy.GetCommandFromFactory (uri, this.mCommandsFactory);

			if (command is SAMLAbstractCommand && this.Command != null && !(this.Command is SAMLAbstractCommand))
				((SAMLAbstractCommand)command).NextCommand = this.Command;

			this.Command = command;
		}

		public void RedirectToLogin (ValidationError error = null) {
			if (this.Command != null)
				this.Command.RedirectToLoginView (error);
			else
				this.mListener.RedirectToLoginView (error);
		}

		public async void AutoLogin () {
			try {
				await LoggedUser.Instance.CheckTokenAsync ();
			} catch (ValidationError e) {
				this.RedirectToLogin (e);
				return;
			} catch (Exception) {
				this.RedirectToLogin (new ValidationError ("Error", Service.NoConnectionError));
				return;
			}

			this.State = StateEnum.Ready;
		}

		public async void Invoke () {
			if (this.Command != null) {
				try {
					await this.Command.InvokeAsync ();
				} catch (ValidationError e) {
					this.RedirectToLogin (e);
					return;
				} catch (Exception) {
					this.RedirectToLogin (new ValidationError ("Error", Service.NoConnectionError));
					return;
				}
			} else
				this.mListener.RedirectToMainView ();

			this.Command = null;
		}

		public void ResetState () {
			this.State = StateEnum.Unknown;
		}
	}
}