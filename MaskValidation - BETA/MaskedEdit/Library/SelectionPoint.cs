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

		/// <summary>
		/// used by renderer
		/// </summary>
		/// <value>The start.</value>
		public Int32 Start  { get; set; }

		/// <summary>
		/// used by renderer. 
		/// </summary>
		/// <value>The end.</value>
		public Int32 End  { get; set; }

		/// <summary>
		/// used by renderer. 
		/// </summary>
		/// <value>The text.</value>
		public string Text { get; set; }
	}
}

