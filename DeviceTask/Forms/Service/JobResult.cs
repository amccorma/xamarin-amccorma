using System;

namespace DeviceTask
{
	public class JobResult
	{
		public JobResult ()
		{
		}

		public string JobID { get; set; }

		public bool HasError { get; set; }

		public bool IsRunning { get; set; }

		public bool OK {
			get {
				return HasError == false;
			}
		}
	}
}

