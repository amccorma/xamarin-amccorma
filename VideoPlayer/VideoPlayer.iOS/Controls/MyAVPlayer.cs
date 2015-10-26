using System;
using MonoTouch.AVFoundation;
using MonoTouch.Foundation;

namespace LabSamples.iOS
{
	public class MyAVPlayer : AVPlayer
	{
		public MyAVPlayer ()
		{
		}

		public MyAVPlayer (IntPtr handle) : base (handle)
		{
		}

		public MyAVPlayer(AVPlayerItem item) : base(item)
		{

		}
	}
}

