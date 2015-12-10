using System;
using System.Collections.Generic;

namespace Masked.Library
{
	public class MaskRules
	{
		/// <summary>
		/// start Position of Mask
		/// </summary>
		/// <value>The start.</value>
		public Int16 Start { get; set; }

		/// <summary>
		/// End Position of Mask
		/// </summary>
		/// <value>The end.</value>
		public Int16 End { get; set; }

		/// <summary>
		/// Mask, see examples 
		/// </summary>
		/// <value>The mask.</value>
		public string Mask { get; set; }

		public List<Validation> Rules { get; set; }
	}
}

