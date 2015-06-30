using System;
using Xamarin.Forms;
using UIKit;
using Foundation;
using System.Collections.Concurrent;
using System.Threading.Tasks;

[assembly: Dependency(typeof(DeviceTask.iOS.Service.iOSTasks))]
namespace DeviceTask.iOS.Service
{
	public class iOSTasks : IAppTask
    {
		private AppTask _Task;
		private nint _FiniteTaskID;
		protected static ConcurrentDictionary<string, JobResult> _Jobs;

        public double TimeRemaining
        {
            get
            {
                return UIApplication.SharedApplication.BackgroundTimeRemaining;
            }
        }

        public iOSTasks()
        {
			
        }

		private void AddJob(DeviceTask.AppTask task)
		{
			if (_Jobs == null) {
				_Jobs = new ConcurrentDictionary<string, JobResult> ();
			}				

			var temp = new JobResult { IsRunning = true };

			_Jobs.AddOrUpdate(task.JobID,  temp,  (z, x) => {
				return temp;
			});

			this._Task = task;
		}

		private void UpdateJob(bool HasError)
		{
			var temp = this._Task;
			if (temp != null) {
				JobResult result;
				if (_Jobs != null && _Jobs.TryGetValue (temp.JobID, out result)) {
					result.IsRunning = false;
					result.JobID = temp.JobID;
					result.HasError = HasError;
					_Jobs.TryUpdate (temp.JobID, result, result);
				} 
			}
		}

		#region IAppTask implementation

		public void RunTask (AppTask task)
		{			
			AddJob (task);
			FiniteLengthTask (task);
		}

		public bool IsMainThread ()
		{
			return NSThread.IsMain;
		}

		public string ThreadID ()
		{
			return System.Threading.Thread.CurrentThread.ManagedThreadId.ToString ();
		}

		public JobResult GetResult (string JobID)
		{
			JobResult result;
			if (_Jobs != null && _Jobs.TryGetValue (JobID, out result)) {
				return result;
			}
			return null;
		}

		#endregion


        public void FiniteLengthTask(AppTask task)
        {
			task.IsRunning = true;
            this._FiniteTaskID = UIApplication.SharedApplication.BeginBackgroundTask(FiniteTaskEnd);
            try
            {
				task.task
					.ContinueWith( t => {

						Console.WriteLine("ContinueWith:=" + TimeRemaining);
						if (t.Exception != null)
						{
							Console.WriteLine("ContinueWith/Exception:=" + TimeRemaining);
							UpdateJob (true);
							this._Task.Error(t.Exception.Flatten());
						}
						else if (t.IsCanceled || t.IsFaulted)
						{
							UpdateJob (true);
							this._Task.Error(new TaskCanceledException());
						}
						else
						{
							Console.WriteLine("ContinueWith/ok:=" + TimeRemaining);
							UpdateJob (false);
							this._Task.Complete(t.Result);
						}
						task.task.Dispose();
						task.IsRunning = false;
						UIApplication.SharedApplication.EndBackgroundTask(this._FiniteTaskID);
						this._FiniteTaskID = -1;
					});
            }
			catch (OperationCanceledException cancel)
            {
				UpdateJob (true);
				this._Task.Error (cancel);
            }
			catch (Exception ex) {
				UpdateJob (true);
				this._Task.Error (ex);
			}
        }

		/// <summary>
		/// Called if Operating System is Cancelling the Task, gets called about 3 seconds before end
		/// </summary>
        private void FiniteTaskEnd()
        {
			//UpdateJob (true);
			Console.WriteLine("FiniteTaskEnd called");
            this._Task.Token.Cancel();
        }
    }
}

