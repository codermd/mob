using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Mxp.Core.Helpers
{
	// Source : http://stackoverflow.com/questions/298976/is-there-a-better-alternative-than-this-to-switch-on-type
	public static class TypeSwitch {
		public class CaseInfo {
			public bool IsDefault { get; set; }
			public TypeInfo Target { get; set; }
			public Action<object> Action { get; set; }
		}

		public class ReturnedCaseInfo {
			public TypeInfo Target { get; set; }
			public object @object { get; set; }
			public Func<Task<object>> Action { get; set; }
		}

		public static void Do(object source, params CaseInfo[] cases) {
			System.Type type = source is System.Type ? (System.Type)source : source.GetType ();
			TypeInfo typeInfo = type.GetTypeInfo ();

			foreach (var entry in cases) {
				if (entry.IsDefault || entry.Target.IsAssignableFrom(typeInfo)) {
					entry.Action(source);
					break;
				}
			}
		}

		public static T Do<T>(System.Type type, params ReturnedCaseInfo[] cases) {
			TypeInfo typeInfo = type.GetTypeInfo ();

			foreach (ReturnedCaseInfo returnedCaseInfo in cases) {
				if (returnedCaseInfo.Target.IsAssignableFrom(typeInfo)) {
					return (T) returnedCaseInfo.@object;
				}
			}

			return default (T);
		}

		public static async Task<T> DoTask<T>(System.Type type, params ReturnedCaseInfo[] cases) {
			TypeInfo typeInfo = type.GetTypeInfo ();

			foreach (ReturnedCaseInfo returnedCaseInfo in cases)
				if (returnedCaseInfo.Target.IsAssignableFrom (typeInfo))
					return (T) await returnedCaseInfo.Action ();

			return default (T);
		}

		public static CaseInfo Case<T>(Action action) {
			return new CaseInfo() {
				Action = x => action(),
				Target = typeof(T).GetTypeInfo ()
			};
		}

		public static CaseInfo Case<T>(Action<T> action) {
			return new CaseInfo() {
				Action = (x) => action((T)x),
				Target = typeof(T).GetTypeInfo ()
			};
		}

		public static CaseInfo Default(Action action) {
			return new CaseInfo() {
				Action = x => action(),
				IsDefault = true
			};
		}

		public static ReturnedCaseInfo Case<T> (Func<Task<object>> action) {
			return new ReturnedCaseInfo () {
				Target = typeof(T).GetTypeInfo (),
				Action = action
			};
		}

		public static ReturnedCaseInfo Case<T> (object @object) {
			return new ReturnedCaseInfo () {
				Target = typeof(T).GetTypeInfo (),
				@object = @object
			};
		}
	}
}