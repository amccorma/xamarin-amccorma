using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using System.Collections.Generic;
using System.Collections.Concurrent;

[assembly: Xamarin.Forms.Dependency(typeof(DeviceTask.Droid.DroidTask))]
namespace DeviceTask.Droid
{
	public class DroidTask : BroadcastReceiver, IAppTask 
	{
		protected static ConcurrentDictionary<string, JobResult> _Jobs;

		#region implemented abstract members of BroadcastReceiver

		public override void OnReceive (Context context, Intent intent)
		{
			var ok = intent.GetBooleanExtra ("ok", false);
			var id = intent.GetStringExtra ("id");
			JobResult result;
			if (_Jobs != null && _Jobs.TryGetValue (id, out result)) {
				result.IsRunning = false;
				result.JobID = id;
				result.HasError = !ok;
				_Jobs.TryUpdate (id, result, result);
			}
		}

		#endregion

		public DroidTask ()
		{
		}

        private void AddJob(AppTask task)
		{
			if (_Jobs == null) {
				_Jobs = new ConcurrentDictionary<string, JobResult> ();
				var intentFilter = new IntentFilter (TaskBinder.JobEnded){Priority = (int)IntentFilterPriority.HighPriority};
				Xamarin.Forms.Forms.Context.RegisterReceiver (this, intentFilter);
			}				

			var temp = new JobResult { IsRunning = true };

			_Jobs.AddOrUpdate(task.JobID,  temp,  (z, x) => {
				return temp;
			});
		}

		#region ILongTask implementation

		public void RunTask(AppTask task)
		{
			AddJob (task);

			// fire the task
			Console.WriteLine ("long task");

			Android.App.Application.Context.StartService (new Intent (Android.App.Application.Context, typeof(TaskService)));
			var TaskServiceConnection = new TaskConnection (null, task);

			Intent TaskServiceIntent = new Intent (Android.App.Application.Context, typeof(TaskService));

			Android.App.Application.Context.BindService (TaskServiceIntent, TaskServiceConnection, Bind.AutoCreate);

		}

		public JobResult GetResult(string JobID)
		{
			JobResult result;
			if (_Jobs != null && _Jobs.TryGetValue (JobID, out result)) {
				return result;
			}
			return null;
		}

		public string ThreadID()
		{
			return System.Threading.Thread.CurrentThread.ManagedThreadId.ToString ();
		}

		public bool IsMainThread()
		{
			return Looper.MainLooper.Thread == Java.Lang.Thread.CurrentThread (); 
		}

		#endregion
	}
}

