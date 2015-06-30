using System;
using System.Threading.Tasks;

namespace DeviceTask
{
	public interface IAppTask
	{
		void RunTask(AppTask task);

		bool IsMainThread();

		string ThreadID ();

		JobResult GetResult (string JobID);
	}
}

