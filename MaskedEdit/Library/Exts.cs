using System;
using System.Threading.Tasks;
using System.Threading;

namespace Masked.Library
{
	public static class Exts
	{
		public static string Replace(this string s, char[] separators, string newVal)
		{
			return String.Join(newVal, s.Split(separators, StringSplitOptions.RemoveEmptyEntries));
		}
	}
}



