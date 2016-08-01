using System;
using ObjCRuntime;

namespace MarqueeBinding
{
	[Native]
	public enum MarqueeType : ulong
	{
		LeftRight = 0,
		RightLeft,
		Continuous,
		ContinuousReverse
	}
}

