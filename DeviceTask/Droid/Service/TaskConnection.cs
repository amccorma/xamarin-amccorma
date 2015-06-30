using System;
using Android.Content;
using Android.OS;
using Android.Util;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace DeviceTask.Droid
{
	public class TaskConnection : Java.Lang.Object, IServiceConnection
	{
		protected TaskBinder _Binder;

		private AppTask _Task;

        protected internal AppTask Task
        {
			get { return _Task; }
			set 
			{ 
				_Task = value; 
			}
		}				

		public TaskBinder Binder
		{
			get { return this._Binder; }
			set { this._Binder = value; }
		}


		public TaskConnection(TaskBinder binder, AppTask task)
		{
			if (binder != null) {
				this._Binder = binder;
			}

			this.Task = task;
		}



		// This gets called when a client tries to bind to the Service with an Intent and an 
		// instance of the ServiceConnection. The system will locate a binder associated with the 
		// running Service 
		public void OnServiceConnected (ComponentName name, IBinder service)
		{
			// cast the binder located by the OS as our local binder subclass
			TaskBinder serviceBinder = service as TaskBinder;
			if (serviceBinder != null) {
				this._Binder = serviceBinder;
				this._Binder.IsBound = true;
				//this._Binder.JobEnded += JobEnded;
				Log.Debug ( "ServiceConnection", "OnServiceConnected Called" );

				serviceBinder.Service.StartTask (this.Task);
			}
		}

		// This will be called when the Service unbinds, or when the app crashes
		public void OnServiceDisconnected (ComponentName name)
		{
			this._Binder.IsBound = false;
			Log.Debug ( "ServiceConnection", "Service unbound" );
		}
	}
}

