using System;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Views.Animations;
using Android.Animation;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Content.Res;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency (typeof (TopAlert.Droid.TopAlertRenderer))]
namespace TopAlert.Droid
{
	public class TopAlertRenderer : ITopAlert
	{
		private static Lazy<TopAlertView> _Instance = new Lazy<TopAlertView> ();

		public void Kill()
		{
			
		}

		public void Show(TopAlert alert)
		{
			_Instance.Value.Show (alert);
		}
	}
}