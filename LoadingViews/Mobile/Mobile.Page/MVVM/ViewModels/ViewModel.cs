using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using mobile.models.MVVM.Library;
using Xamarin.Forms;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using mobile.models.MVVM.ViewModels;
using System.Net;

namespace mobile.models.ViewModels
{
	public abstract class ViewModel : ObservableObject
    {
		private bool _isBusy;
		private string _Title;
		private DateTime _LastUpdated;
		private bool _IsDataDirty;

		private WeakReference<IPage> _CurrentPage;

		//private bool _ErrorLoadingVisible;
		//private string _ErrorLoadingMessage;
		//private bool _IsUnAuthorized;

		private bool _CanGoBack = true;

		private Thickness _PagePadding;
		// job, subjob, complete, Did Prompt


		public ViewModel()
		{
			ShowPromptOnUpdate = false;
			//_ErrorLoadingVisible = false;
			this._PagePadding = new Thickness (-100, -100);
			this.IsAuthorized = true;
			this.IsMaster = false;
			this.IsFirstLoad = true;
		}

		public bool IsMaster  {
			get;
			set;
		} 

		public virtual void HandleLoginEvents(bool IsLogout)
		{

		}
			
		/// <summary>
		/// IsAuthorized.
		/// </summary>
		/// <value><c>true</c> if this instance is authorized; otherwise, <c>false</c>.</value>
		public bool IsFirstLoad {get;set;}

		/// <summary>
		/// IsAuthorized.
		/// </summary>
		/// <value><c>true</c> if this instance is authorized; otherwise, <c>false</c>.</value>
		public bool IsAuthorized {get;set;}

		/// <summary>
		/// enable Hardware back button
		/// </summary>
		/// <value><c>true</c> if this instance can go back; otherwise, <c>false</c>.</value>
		public bool CanGoBack
		{
			get
			{
				return _CanGoBack;
			}
			set
			{
				_CanGoBack = value;
			}
		}

				/// <summary>
		/// Cache Data is Dirty
		/// </summary>
		/// <value><c>true</c> if this instance is data dirty; otherwise, <c>false</c>.</value>
		public bool IsDataDirty { 
			get { return _IsDataDirty; }
			set {
				SetProperty(ref _IsDataDirty, value);
			}
		}

		/// <summary>
		/// show user prompt on cache change
		/// </summary>
		/// <value><c>true</c> if show prompt on update; otherwise, <c>false</c>.</value>
		public bool ShowPromptOnUpdate {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the navigation.
		/// </summary>
		/// <value>The navigation.</value>
		public ViewModelNavigation Navigation { get; set; }
	
		public DateTime LastUpdated {
			get { return _LastUpdated; }
			set {
				SetProperty<DateTime> (ref _LastUpdated, value);
			}
		}

		protected void Start()
		{
			this.IsBusy = true;
		}

		public string Title {
			get { 
				return _Title;
			}
			set {
				SetProperty<string> (ref _Title, value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance is busy.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is busy; otherwise, <c>false</c>.
		/// </value>
		public bool IsBusy
		{
			get { return _isBusy; }
			set
			{
				SetProperty<bool>(ref _isBusy, value);
			}
		}
			
		/// <summary>
		/// Currentpage as IPage
		/// </summary>
		/// <returns>The page.</returns>
		public IPage CurrentPage {
			get {
				IPage p = null;
				if (this._CurrentPage != null && this._CurrentPage.TryGetTarget(out p))
				{
					return p;
				}
				return p;
			}
		}

		protected IPage SetCurrentPage {
			set {
				this._CurrentPage = new WeakReference<IPage> (value);
			}
		}


		protected T Get<T>(Func<T> obj)
		{
			try
			{
				return obj.Invoke();
			}
			catch (Exception) {
				return default(T);
			}
		}

		protected void Set(Action obj, [CallerMemberName] string method = "")
		{
			try
			{
				obj.Invoke();
				NotifyPropertyChanged(method);
			}
			catch {

			}
		}

		public virtual Thickness PagePadding {
			get {
				return _PagePadding;
			}
			set {
				_PagePadding = value;
			}
		}

		public bool IsInit {get;set;}
    }
}

