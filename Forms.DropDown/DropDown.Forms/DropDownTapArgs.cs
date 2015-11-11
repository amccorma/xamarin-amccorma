using System;

using Xamarin.Forms;

namespace DropDown.Forms
{
	public class DropDownTapArgs
	{
		public DropDownTapArgs (float x, float y)
		{
			this.X = x;
			this.Y = y;
		}

		public float X { get; set; }

		public float Y { get; set; }
	}
}


