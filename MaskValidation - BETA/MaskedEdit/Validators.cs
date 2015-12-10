using System;
using Xamarin.Forms;

namespace Masked
{
	public enum Validators
	{
		MAX,
		ONLYCHARS,
		STARTC,
		ENDC
	}

	public class Validation
	{
		public Validation(Validators op, string arg, string error)
		{
			this.Operation = op;
			this.Arg = arg;
			this.ErrorMessage = error;
		}

		public Validators Operation { get; set; }

		public string Arg { get; set; }

		public string ErrorMessage { get; set; }

		public bool CaseCheck { get; set; }

		public Int32 ArgAsInt {
			get {
				return Convert.ToInt32 (Arg);
			}
		}
	}
}


