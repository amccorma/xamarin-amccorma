using System;
using System.Threading.Tasks;

namespace DeviceTask
{
	public interface ILongTask
	{
		void RunTask(LongTask task);

		bool IsMainThread();

		string ThreadID ();

		JobResult GetResult (string JobID);
	}
}

