using System;

namespace iOSMaskedEdit
{
	public class TextHolder
	{
		public TextHolder()
		{

		}

		public TextHolder(string t, Int32 start, Int32 end = -1)
		{
			this.Text = t;
			this.SelectionStart = start;
			this.SelectionEnd = end;
		}

		public Int32 SelectionStart { get; set; }

		public Int32 SelectionEnd { get; set; }

		public string Text { get; set; }

		public string RemovedBlock { get; set; }

		public Int32 RemovedBlockCount { get; set; }

		public override string ToString ()
		{
			return Text;
		}
	}
}

