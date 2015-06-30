using System;
using System.Threading.Tasks;
using System.Threading;

namespace DeviceTask
{
	public class AppTask<T>
	{
		private Action<Exception> _Error = (x) => {	};
		private Action<T> _Completed = () => {	};

		public AppTask ()
		{
			JobID = Guid.NewGuid ().ToString ();
		}

		public Task<T> task {get; set;}

		/// <summary>
		/// Cancellation Token
		/// </summary>
		/// <value>The token.</value>
		public CancellationToken Token {get;set;}

		public Action Completed {
			get {
				return _Completed;
			}
			set {
				_Completed = value;
			}
		}

		public Action<Exception> Error 
		{
			get
			{
				return _Error;
			}
			set
			{
				_Error = value;
			}
		}

		public string JobID { get;set; }

		public bool WasCancelled { get; set; }
	}
}

