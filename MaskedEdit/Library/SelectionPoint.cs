using System;

namespace Masked.Library
{
	public class SelectionPoint
	{
		public SelectionPoint(Int32 start, Int32 end)
		{
			this.Start = start;
			this.End = end;
		}

		public SelectionPoint(Int32 start)
		{
			this.Start = start;
			this.End = -1;
		}

		public Int32 Start {
			get;
			set;
		}

		public Int32 End {
			get;
			set;
		}
	}
}

