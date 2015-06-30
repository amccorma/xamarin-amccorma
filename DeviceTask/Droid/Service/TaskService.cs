using System;
using Android.App;
using Android.Content;
using Android.Util;
using Android.OS;
using Android.Preferences;

namespace DeviceTask.Droid
{
	[Service]
	public class TaskService : Service
	{
		private IBinder _binder;
		readonly string _LogTag = "TaskService";

		public TaskService ()
		{
		}

		public override void OnCreate ()
		{
			base.OnCreate ();
			Log.Debug (this._LogTag, "OnCreate called in the Task Service");
		}

		// This gets called when StartService is called in our App class
		public override StartCommandResult OnStartCommand (Intent intent, StartCommandFlags flags, int startId)
		{
			Log.Debug (this._LogTag, "Task started");

			return StartCommandResult.Sticky;
		}

		/// <summary>
		/// Execute the Task
		/// </summary>
		public void StartTask(AppTask t)
		{
			Log.Debug(this._LogTag, "StartTask");
			var id = t.JobID;
			t.task
				.ContinueWith (x => {
					Log.Debug(this._LogTag, "After service execute");
					var b = this._binder as TaskBinder;
					if (x.Exception != null)
					{
						t.Error(x.Exception);
					}
					else if (x.IsCanceled)
					{
						t.Error(new System.Threading.Tasks.TaskCanceledException());
					}
					else
					{
						t.Complete(x.Result);
					}

					var jobstop = new Intent (TaskBinder.JobEnded); 
					jobstop.PutExtra("id", t.JobID);
					jobstop.PutExtra("ok", x.Exception == null && x.IsCanceled == false);
					SendOrderedBroadcast (jobstop, null);

					// dispose of the task
					t.task.Dispose();
			});
		}

		// This gets called once, the first time any client bind to the Service
		// and returns an instance of the LocationServicethis._binder. All future clients will
		// reuse the same instance of the this._binder
		public override IBinder OnBind (Intent intent)
		{
			Log.Debug (this._LogTag, "Client now bound to service");

			this._binder = new TaskBinder (this);
			return this._binder;
		}

		public override bool StopService (Intent name)
		{
			Log.Debug (this._LogTag, "StopService called in the Task Service");
			return base.StopService (name);
		}
	}
}

