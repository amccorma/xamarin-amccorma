using System;
using System.Collections.Generic;

namespace iOSMaskedEdit
{
	public class MaskProperties
	{
		public MaskProperties ()
		{
		}

		public List<MaskRules> Mask { get; set; }

		public Int32 MaxLength
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the format characters.
		/// </summary>
		/// <value>The format characters.</value>
		public string FormatCharacters
		{
			get;
			set;
		}
	}
}

