using System;
using System.Threading.Tasks;
using System.Threading;

namespace DeviceTask
{
	public class AppTask
	{
		private Action<Exception> _Error = (x) => {	};
		private Action<object> _Complete = (x) => {	};

		public AppTask ()
		{
			JobID = Guid.NewGuid ().ToString ();
			Token = new CancellationTokenSource ();
		}

		/// <summary>
		/// Is the Task Running
		/// </summary>
		/// <value><c>true</c> if this instance is running; otherwise, <c>false</c>.</value>
		public bool IsRunning { get; set; }

		/// <summary>
		/// What you want to execute.  must return a value.  use null if nothing
		/// </summary>
		/// <value>The task.</value>
		public Task<object> task { get; set;}

		/// <summary>
		/// Set internal
		/// </summary>
		/// <value>The token.</value>
		public CancellationTokenSource Token {get; }

		/// <summary>
		/// If no errors, Complete fires
		/// </summary>
		/// <value>The complete.</value>
		public Action<object> Complete {
			get {
				return _Complete;
			}
			set {
				_Complete = value;
			}
		}

		/// <summary>
		/// If errors, handle this
		/// </summary>
		/// <value>The error.</value>
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

		/// <summary>
		/// Job ID
		/// </summary>
		/// <value>The job I.</value>
		public string JobID { get;set; }
	}
}

