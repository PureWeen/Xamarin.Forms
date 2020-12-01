using System;
using System.Collections.Generic;
using System.Text;
using Android.Content;
using LP = Android.Views.ViewGroup.LayoutParams;
using AndroidX.AppCompat.Widget;
using Android.Views;
using Google.Android.Material.AppBar;
using AndroidX.AppCompat.App;

namespace Xamarin.Forms.Platform.Android
{
	internal class ShellTitleView : ShellViewRenderer
	{
		readonly Context _context;
		double _previousWidth;
		double _previousHeight;

		public ShellTitleView(Context context, View view) : base(context, view)
		{
			_context = context;
		}

		public int ToolbarWidthPx { get; set; }
		public int ToolbarHeightPx { get; set; }
		public global::Android.Widget.FrameLayout Container { get; internal set; }

		internal void LayoutView()
		{
			int lpWidth = LP.WrapContent;
			int lpHeight = LP.WrapContent;

			if (!View.IsSet(View.HorizontalOptionsProperty) ||
				View.HorizontalOptions.Equals(LayoutOptions.Fill) ||
				View.HorizontalOptions.Expands)
			{
				lpWidth = LP.MatchParent;
			}

			if (!View.IsSet(View.VerticalOptionsProperty) ||
				View.VerticalOptions.Equals(LayoutOptions.Fill) ||
				View.VerticalOptions.Expands)
			{
				lpHeight = LP.MatchParent;
			}

			var layoutParams = new Toolbar.LayoutParams(lpWidth, lpHeight)
			{
				LeftMargin = (int)_context.ToPixels(View.Margin.Left),
				TopMargin = (int)_context.ToPixels(View.Margin.Top),
				RightMargin = (int)_context.ToPixels(View.Margin.Right),
				BottomMargin = (int)_context.ToPixels(View.Margin.Bottom)
			};

			NativeView.LayoutParameters = layoutParams;
		}

		public override void OnViewSet(View view)
		{
			_previousWidth = 0;
			_previousHeight = 0;

			base.OnViewSet(view);
		}


		public void SetupLayoutParameters()
		{
			if (ToolbarWidthPx > 0 && ToolbarHeightPx > 0)
			{
				var titleViewLayoutParams = NativeView.LayoutParameters as global::Android.Widget.FrameLayout.LayoutParams;
				int layoutWidth = LP.WrapContent;
				int layoutHeight = LP.WrapContent;
				GravityFlags gravityFlagH = GravityFlags.CenterHorizontal;
				GravityFlags gravityFlagV = GravityFlags.CenterVertical;
				int containerHeight;
				int containerWidth;

				if (View.HorizontalOptions.Equals(LayoutOptions.Center) ||
					(View.IsSet(View.HorizontalOptionsProperty) && View.HorizontalOptions.Equals(LayoutOptions.Fill)))
				{
					containerWidth = ToolbarWidthPx;

					
					int[] location = new int[2];
					Container.GetLocationOnScreen(location);

					if (location[0] != 0)
					{
						int[] locationParent = new int[2];
						(Container.Parent as global::Android.Views.View)?.GetLocationOnScreen(locationParent);

						location[0] -= locationParent[0];
						location[1] -= locationParent[1];
						Container.TranslationX = Container.TranslationX - location[0];
					}

					if (View.HorizontalOptions.Equals(LayoutOptions.Center))
					{
						layoutWidth = LP.WrapContent;
						gravityFlagH = GravityFlags.CenterHorizontal;
					}
					else if (View.HorizontalOptions.Equals(LayoutOptions.Fill))
					{
						gravityFlagH = GravityFlags.FillHorizontal;
						layoutWidth = LP.MatchParent;
					}
				}
				else
				{
					containerWidth = LP.MatchParent;
					if (View.HorizontalOptions.Equals(LayoutOptions.Start))
					{
						layoutWidth = LP.WrapContent;
						gravityFlagH = GravityFlags.Start;
					}
					else if (View.HorizontalOptions.Equals(LayoutOptions.End))
					{
						layoutWidth = LP.WrapContent;
						gravityFlagH = GravityFlags.End;
					}
					else
					{
						layoutWidth = LP.MatchParent;
						gravityFlagH = GravityFlags.FillHorizontal;
					}

					Container.TranslationX = 0;
				}

				if (View.VerticalOptions.Equals(LayoutOptions.Center) ||
					(View.IsSet(View.VerticalOptionsProperty) && View.VerticalOptions.Equals(LayoutOptions.Fill)))
				{
					containerHeight = ToolbarHeightPx;

					if (View.VerticalOptions.Equals(LayoutOptions.Center))
					{
						layoutHeight = LP.WrapContent;
						gravityFlagV = GravityFlags.CenterVertical;
					}
					else if (View.VerticalOptions.Equals(LayoutOptions.Fill))
					{
						gravityFlagV = GravityFlags.FillVertical;
						layoutHeight = LP.MatchParent;
					}
				}
				else
				{
					containerHeight = LP.MatchParent;
					if (View.VerticalOptions.Equals(LayoutOptions.Start))
					{
						layoutHeight = LP.WrapContent;
						gravityFlagV = GravityFlags.Top;
					}
					else if (View.VerticalOptions.Equals(LayoutOptions.End))
					{
						layoutHeight = LP.WrapContent;
						gravityFlagV = GravityFlags.Bottom;
					}
					else
					{
						layoutHeight = LP.MatchParent;
						gravityFlagV = GravityFlags.FillVertical;
					}
				}

				if (Container.LayoutParameters.Height != containerHeight || Container.LayoutParameters.Width != containerWidth)
				{
					Container.LayoutParameters = new Toolbar.LayoutParams(containerWidth, containerHeight);
				}

				var gravity = gravityFlagH | gravityFlagV;

				if (layoutWidth == LP.WrapContent && titleViewLayoutParams.Width > 0)
					layoutWidth = titleViewLayoutParams.Width;

				if (layoutHeight == LP.WrapContent && titleViewLayoutParams.Height > 0)
					layoutHeight = titleViewLayoutParams.Height;

				if (titleViewLayoutParams.Width != layoutWidth ||
					titleViewLayoutParams.Height != layoutHeight ||
					titleViewLayoutParams.Gravity != gravity)
				{
					NativeView.LayoutParameters =
							new global::Android.Widget.FrameLayout.LayoutParams(layoutWidth, layoutHeight, gravity);
				}
			}

		}

		public void LayoutViews()
		{
			double width = _context.FromPixels(NativeView.MeasuredWidth);
			double height = _context.FromPixels(NativeView.MeasuredHeight);
			var toolbarWidthpx = ToolbarWidthPx;
			var toolbarHeightpx = ToolbarHeightPx;
			var measureWidth = width;
			var measureHeight = height;

			if (_previousWidth != width || _previousHeight != height)
			{
				_previousWidth = width;
				_previousHeight = height;

				if (View.IsSet(View.HorizontalOptionsProperty))
				{
					if(View.HorizontalOptions.Equals(LayoutOptions.Fill) || View.HorizontalOptions.Expands)
						measureWidth = _context.FromPixels(ToolbarWidthPx);
					else
						measureWidth = double.PositiveInfinity;
				}

				if (View.IsSet(View.VerticalOptionsProperty) || View.VerticalOptions.Expands)
				{
					if (View.VerticalOptions.Equals(LayoutOptions.Fill))
						measureHeight = _context.FromPixels(ToolbarHeightPx);
					else
						measureHeight = double.PositiveInfinity;
				}

				if (measureWidth != 0 && measureHeight != 0)
				{
					LayoutView(
						measureWidth,
						measureHeight,
						_context.FromPixels(ToolbarWidthPx),
						_context.FromPixels(ToolbarHeightPx));
				}
			}
		}
	}
}
