using System;
using Android.OS;

namespace DeviceTask.Droid
{
	public class TaskBinder : Binder
	{
		protected TaskService service;
		public const string JobEnded = "end";

		public TaskService Service
		{
			get { return this.service; }
		} 


		public bool IsBound { get; set; }

		// constructor
		public TaskBinder (TaskService service)
		{
			this.service = service;
		}
	}
}

