using System;
using Xamarin.Forms;

namespace FrameBorder
{
	public class FrameRectConverter : TypeConverter
	{
		public FrameRectConverter ()
		{
		}

		public override bool CanConvertFrom (Type sourceType)
		{
			if (sourceType == null) {
				throw new ArgumentException ("FrameRectConverter sourceType = null");
			}
			return sourceType == typeof(string);
		}

		/// <param name="culture">To be added.</param>
		/// <param name="value">To be added.</param>
		/// <summary>
		/// format: L, T, R, B or H,V
		/// </summary>
		/// <returns>The from.</returns>
		public override object ConvertFrom (System.Globalization.CultureInfo culture, object value)
		{
			if (value == null)
				return (object)null;

			var str = value as string;
			if (str != null) {
				var arr = str.Split (',');
				switch (arr.Length) {
				case 2:
					return new FrameRect (Convert.ToInt32 (arr [0]), Convert.ToInt32 (arr [1]));
				case 4:
					return new FrameRect (Convert.ToInt32 (arr [0]), Convert.ToInt32 (arr [1]), Convert.ToInt32(arr[2]), Convert.ToInt32(arr[3]));
				default:
					throw new Exception ("FrameRectConverter accepts string in format 1,2 or 1,2,3,4");
				}
			}

			throw new InvalidOperationException (String.Format ("Cannot convert \"{0}\" into {1}", new object[2] {
				value,
				(object)typeof(FrameRectConverter)
			}));
		}
	}
}

