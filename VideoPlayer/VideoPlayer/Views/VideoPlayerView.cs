/** 
 * 
TODO:  Add property for Play and Pause Button Images
TEST Portrait mode and update layout
RECALACULATE Play Button Height, or assign property
*/

using System;
using Xamarin.Forms;
using System.Linq.Expressions;
using LabSamples.Controls;
using LabSamples.EXT;

namespace LabSamples.Views
{
	public class VideoPlayerView : ContentView
	{
		/// <summary>
		/// attached event handlers to this object
		/// </summary>
		private MyVideoPlayer _VideoPlayer;

		public MyVideoPlayer VideoPlayer {
			get {
				return this._VideoPlayer;
			}
		}

		public Image PlayButton {
			get;
			set;
		}

		public VideoPlayerView ()
		{
            this._VideoPlayer = new MyVideoPlayer();
			this.HorizontalOptions = LayoutOptions.FillAndExpand;
			this.VerticalOptions = LayoutOptions.FillAndExpand;

			this.Content = this._VideoPlayer;
		}

		protected override void OnParentSet ()
		{
			base.OnParentSet ();
            this.VideoPlayer.FileSource = FileSource;
		}

		public static readonly BindableProperty FileSourceProperty = 
			BindableProperty.Create<VideoPlayerView,string>(
				p => p.FileSource, "");

		public string FileSource {
			get { return (string)GetValue (FileSourceProperty); }
			set { SetValue (FileSourceProperty, value); }
		}

	}
}

