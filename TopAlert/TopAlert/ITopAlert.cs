using System;
using System.Threading.Tasks;

namespace TopAlert
{
	public interface ITopAlert
	{
		void Show(TopAlert alert);

		void Kill();
	}
}

