using System;
using Android.Content;
using Android.Runtime;
using Android.Util;

namespace Xamarin.Forms.Platform.Android
{
	public class ShellToolbar : AndroidX.AppCompat.Widget.Toolbar
	{
		internal ShellTitleView TitleViewContainer { get; set; }

		public ShellToolbar(Context context) : base(context)
		{
		}

		public ShellToolbar(Context context, IAttributeSet attrs) : base(context, attrs)
		{
		}

		public ShellToolbar(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
		}

		protected ShellToolbar(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}

		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

			if (TitleViewContainer != null)
			{
				TitleViewContainer.ToolbarHeightPx = MeasuredHeight;
				TitleViewContainer.ToolbarWidthPx = MeasuredWidth;
				TitleViewContainer.SetupLayoutParameters();
				TitleViewContainer.LayoutViews();
			}
		}

		protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
		{
			base.OnLayout(changed, left, top, right, bottom);

			if (TitleViewContainer != null)
			{
				TitleViewContainer.SetupLayoutParameters();
				TitleViewContainer.LayoutViews();
			}
		}
	}
}
