using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms.Platform.UWP;
using Xamarin.Forms;

namespace Xamarin.Forms.Platform.UWP
{
	public class ShellFlyoutRenderer : ContentControl
	{
		//public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
		//	   nameof(IsSelected), typeof(bool), typeof(ShellFlyoutItemRenderer),
		//	   new PropertyMetadata(default(bool), IsSelectedChanged));

		View _content;
		public ShellFlyoutRenderer()
		{
			this.DataContextChanged += OnDataContextChanged;
		}

		protected override Windows.Foundation.Size ArrangeOverride(Windows.Foundation.Size finalSize)
		{
			var size = base.ArrangeOverride(finalSize);

			if (_content != null)
			{
				_content.HeightRequest = size.Height;
			}
			return size;
		}
		void OnDataContextChanged(Windows.UI.Xaml.FrameworkElement sender, Windows.UI.Xaml.DataContextChangedEventArgs args)
		{
			if (Content is ViewToRendererConverter.WrapperControl oldControl)
			{
				oldControl.CleanUp();
				if (_content != null)
				{
					_content.BindingContext = null;
					_content.Parent = null;
					_content = null;
				}
			}


			var bo = (BindableObject)DataContext;
			var shell = (bo as Shell);
			var shellController = (IShellController)shell;

			if (shellController == null)
				return;

			//_content = (View)Shell.GetFlyoutContentTemplate(shell).CreateContent();
			_content.BindingContext = shellController.GenerateFlyoutGrouping();



			//_content = new ContentView()
			//{
			//	Content = _content
			//};

			/*var renderer = _content.GetOrCreateRenderer();


			Layout.LayoutChildIntoBoundingRegion(_content, 
				new Rectangle(0, 0, 300, 2000));*/

			//this.Height = 1000;
			//var nativeElement = renderer as FrameworkElement;
			//_content = flyoutLayout;

			//if (dataTemplate != null)
			{
				//_content = (View)dataTemplate.CreateContent();
				_content.BindingContext = bo;
				_content.Parent = shell;
				//nativeElement.Height = 2000;
				//nativeElement.Width = 200;
				//nativeElement.Measure(new Windows.Foundation.Size(200, 2000));
				Content = new ViewToRendererConverter.WrapperControl(_content)
				{
					SizeToContainer = true
				};
			}
		}
	}
}
