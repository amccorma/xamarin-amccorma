using System;

using Xamarin.Forms;

namespace FrameBorder
{
	public class FrameTest : ContentPage
	{
		private StackLayout AddControlToStack(params View[] items)
		{
			var stack = new StackLayout {
				Padding = new Thickness (5, 0, 5, 0),
			};


			foreach (var item in items) {
				stack.Children.Add (item);
			}

			return stack;
		}

		private void AddFrameChildren(MyFrame frame, params View[] items)
		{
			var stack = new StackLayout { 	
				Padding = new Thickness(0,1,0,1),
				Spacing = 0
			};

			var innerStack = new StackLayout {
				BackgroundColor = Color.Transparent, // Color.FromHex ("E9EAED"),
				Padding = new Thickness (0, 0, 0, 0)
			};

			foreach (var item in items) {
				innerStack.Children.Add (item);
			}

			stack.Children.Add (innerStack);

			frame.Content = stack;
		}

		public FrameTest ()
		{
//			var Frame1 = new MyFrame {
//				Padding = new Thickness(0, 0, 0, 0),
//				OutlineColor = Color.Black,
//				StrokeThickness = 5,
//				Radius = 10,
//				BackgroundColor = Color.White,
//				Borders = new FrameRect(0,0,0,1),
//				ClassId = "Frame1",
//				HasShadow = false
//			};


			var Frame1 = new MyFrame {
				Padding = new Thickness(0, 0, 0, 0),
				OutlineColor = Color.Black,
				StrokeThickness = 5,
				Radius = 3,
				BackgroundColor = Color.Pink,
				Borders = new FrameRect(1,1,1,1),
				ClassId = "Frame1",
				HasShadow = false
			};

			var Frame2 = new MyFrame {
				Padding = new Thickness(0, 0, 0, 0),
				OutlineColor = Color.Black,
				StrokeThickness = 5,
				Radius = 3,
				BackgroundColor = Color.Pink,
				Borders = new FrameRect(1,1,1,1),
				ClassId = "Frame2",
				HasShadow = false
			};


			var Frame3 = new MyFrame {
				Padding = new Thickness(0, 0, 0, 0),
				OutlineColor = Color.Black,
				StrokeThickness = 5,
				Radius = 15,
				BackgroundColor = Color.White,
				Borders = new FrameRect(1,1,1,1),
				ClassId = "Frame3",
				HasShadow = true,
				ShadowColor = Color.Blue.AddLuminosity(.2),
				ShadowBorders = new FrameRect(1,1,1,1),
				ShadowRadius = 5,

				// iOS properties
				ShadowOpacity = 1,
				ShadowOffset = new Point(5,5)
			};

			var Frame4 = new MyFrame {
				Padding = new Thickness(0, 0, 0, 0),
				OutlineColor = Color.Black,
				StrokeThickness = 4,
				Radius = 0,
				BackgroundColor = Color.White,
				Borders = new FrameRect(0,1,0,1),

				// Android Property
				StrokeType = StrokeType.Dashed
			};

			var Frame5 = new MyFrame {
				OutlineColor = Color.Red,
				StrokeThickness = 2,
				Borders = new FrameRect(0, 1, 0, 1),
				BackgroundColor = Color.Yellow,
				Radius = 0
			};

			var Label1 = new Label {
				Text = "Border Bottom",
				TextColor = Color.Black
			};

			var Label2  = new Label {
				Text = "Ah meta descriptions… the last bastion of traditional marketing! The only cross-over point between marketing and search engine optimisation! The knife edge between beautiful branding and an online suicide note!",
				TextColor = Color.Black
			};

			var Label3 = new Label {
				Text = "Full Black Border, Pink Background, Radius:3",
				TextColor = Color.Black
			};

			var Label4  = new Label {
				Text = "Ah meta descriptions… the last bastion of traditional marketing! The only cross-over point between marketing and search engine optimisation! The knife edge between beautiful branding and an online suicide note!",
				TextColor = Color.Black
			};

			var Label5 = new Label {
				Text = "Black Border with Shadow",
				TextColor = Color.Black
			};

			var Label6  = new Label {
				Text = "Ah meta descriptions… the last bastion of traditional marketing! The only cross-over point between marketing and search engine optimisation! The knife edge between beautiful branding and an online suicide note!",
				TextColor = Color.Black
			};

			var Label7 = new Label {
				Text = "Top/Bottom Dashed Border. Radius = 0. iOS only solid border",
				TextColor = Color.Black
			};

			var Label8  = new Label {
				Text = "Ah meta descriptions… the last bastion of traditional marketing! The only cross-over point between marketing and search engine optimisation! The knife edge between beautiful branding and an online suicide note!",
				TextColor = Color.Black
			};

			var Label9 = new Label {
				Text = "Red Border, Border: top, bottom, Yellow Background, Radius 0, Stroke: 6",
				TextColor = Color.Black
			};

			var Label10  = new Label {
				Text = "Ah meta descriptions… the last bastion of traditional marketing! The only cross-over point between marketing and search engine optimisation! The knife edge between beautiful branding and an online suicide note!",
				TextColor = Color.Black
			};

			AddFrameChildren (Frame1, AddControlToStack (Label1), Line (), AddControlToStack (Label2));					
			AddFrameChildren (Frame2, AddControlToStack (Label3), Line (), AddControlToStack (Label4));
			AddFrameChildren (Frame3, AddControlToStack (Label5), Line (), AddControlToStack (Label6));
			AddFrameChildren (Frame4, AddControlToStack (Label7), Line (), AddControlToStack (Label8));
			AddFrameChildren (Frame5, AddControlToStack (Label9), Line (), AddControlToStack (Label10));


			Content = new ScrollView { 
				Padding = new Thickness(0, 25, 0, 0),
				Content = new StackLayout
				{
					BackgroundColor = Color.Gray,
					Padding = new Thickness(5, 5, 3, 0),
					Children = {
					//	Frame1,
					//	Frame2,
					//	Frame3,
					//	Frame4,
						Frame5
					}
				}
			};
		}

		private BoxView Line()
		{
			return new BoxView {
				Color = Color.FromHex("d8d8d8"),
				HeightRequest = 1,
				HorizontalOptions = LayoutOptions.FillAndExpand
			};
		}

		private BoxView Line(Xamarin.Forms.Color color)
		{
			return new BoxView {				
				BackgroundColor = color,
				Color = color,
				HeightRequest = 1,
				HorizontalOptions = LayoutOptions.FillAndExpand
			};
		}
	}
}


