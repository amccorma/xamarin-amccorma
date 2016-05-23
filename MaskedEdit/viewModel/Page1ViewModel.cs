using System;
using MvvmHelpers;
using System.Threading.Tasks;

namespace Masked
{
	public class Page1ViewModel : BaseViewModel
	{
		private Xamarin.Forms.Command _ButtonClick;
		string _Text, _TextUpdate;

		public Page1ViewModel ()
		{
			// ok to set text initial
			this.Text1 = "10*02019";
		}

		public Xamarin.Forms.Command ButtonClick {
			get {
				return _ButtonClick ?? (_ButtonClick = new Xamarin.Forms.Command (() => 
					ExecuteButtonClick (), () => true)); 
			}
		}

		// MyEntry.Text Value
		public string Text1
		{
			get { return _Text; }
			set { SetProperty(ref _Text, value); }
		}

		// use this to set the MaskEdit after the Page has been renderer.  DO NOT SET Text
		// maybe an easier solution?
		public string TextUpdate
		{
			get { return _TextUpdate; }
			set { 
				SetProperty(ref _TextUpdate, value);
			}
		}

		void ExecuteButtonClick()
		{
			// Page has been rendered. any changes to Text need to be set with MVVMText
			TextUpdate = "10";
		}
	}
}

