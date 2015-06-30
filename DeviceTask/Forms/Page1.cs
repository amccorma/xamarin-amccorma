using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace DeviceTask
{
	public class Page1 : ContentPage
	{
		private Button btn0, btn1, btn2, btn3;
		private Button task0, task1, task2, task3;
		public Page1 ()
		{

			btn0 = new Button ();
			btn0.Text = "Status 1";
			btn0.Clicked += async(object sender, EventArgs e) => {
				await CheckStatus("1");

			};

			btn1 = new Button ();
			btn1.Text = "Status 2";
			btn1.Clicked += async(object sender, EventArgs e) => {
				await CheckStatus("2");

			};

			btn2 = new Button ();
			btn2.Text = "Status 3";
			btn2.Clicked += async(object sender, EventArgs e) => {
				await CheckStatus("3");

			};

			btn3 = new Button ();
			btn3.Text = "Status 4";
			btn3.Clicked += async(object sender, EventArgs e) => {
				await CheckStatus("4");

			};


			task0 = new Button {
				Text = "Task1",
				Command = new Command (() => {
					RunJob ("1");
				})
			};

			task1 = new Button {
				Text = "Task2",
				Command = new Command (() => {
					RunJob ("2");
				})
			};

			task2 = new Button {
				Text = "Task3",
				Command = new Command (() => {
					RunJob ("3");
				})
			};


			task3 = new Button {
				Text = "Task4",
				Command = new Command (() => {
					RunJob ("4");
				})
			};
					
			this.Content = new StackLayout {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Children = {
					new Button
					{
						Text = "Cancel Task 1",
						Command = new Command( () => {
							if (task != null)
							{
								task.Token.Cancel(false);
							}
						})
					},
					task0,
					task1,
					task2,
					task3,
					btn0,
					btn1,
					btn2,
					btn3
				}
			};
		}

		private async Task CheckStatus(string id)
		{
			var s = DependencyService.Get<IAppTask>();
			var result = s.GetResult (id);
			if (result == null)
			{
				await DisplayAlert(null, id + " not created", "OK");
			}
			else 
			{
				if (result.IsRunning)
				{
					await DisplayAlert(null, id + " Running", "OK");
				}
				else if (result.HasError)
				{
					await DisplayAlert(null, id + " Error", "OK");;
				}
				else
				{
					await DisplayAlert(null, id + " Completed", "OK");
				}
			}
		}

		private AppTask task;

		private void RunJob(string id)
		{
			var s = DependencyService.Get<IAppTask>();
			if (id == "1") {
				task = new AppTask ();
				task.JobID = id;
				task.task = Task.Run<object> (async() => {
					for (int i = 0; i < 1000 && task.Token.IsCancellationRequested == false; i++) {
						await Task.Delay (1000, task.Token.Token);
						System.Diagnostics.Debug.WriteLine (i + "/" + id + "/[id:=" + id.ToString () + "]");
					}
					return null;
				}, task.Token.Token);
				task.Complete = (x) => {
					Xamarin.Forms.Device.BeginInvokeOnMainThread (async () => {
						await DisplayAlert ("done", "done " + task.JobID, "Ok");
					});
				};
				task.Error = (x) => {
					if (x is TaskCanceledException)
					{
						Xamarin.Forms.Device.BeginInvokeOnMainThread (async () => {
							await DisplayAlert ("Cancel", "Error " + task.JobID, "Ok");
						});
					}
					else
					{
						Xamarin.Forms.Device.BeginInvokeOnMainThread (async () => {
							await DisplayAlert ("Error", "Error " + task.JobID, "Ok");
						});
					}
				};

				s.RunTask (task);
			} else {
				var t = new AppTask ();
				t.JobID = id;
				t.task = Task.Run<object> (async() => {
					for (int i = 0; i < 100; i++) {
						await Task.Delay (1000);
						System.Diagnostics.Debug.WriteLine (i + "/" + id + "/[id:=" + id.ToString () + "]");
					}
					return null;
				});
				t.Complete = (x) => {
					Xamarin.Forms.Device.BeginInvokeOnMainThread (async () => {
						await DisplayAlert ("done", "done " + t.JobID, "Ok");
					});
				};

				s.RunTask (t);
			}
		}
	}
}

