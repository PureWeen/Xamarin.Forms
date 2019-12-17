using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;
using System.Linq;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using System.Threading;
using System.ComponentModel;

#if UITEST
using Xamarin.UITest;
using NUnit.Framework;
using Xamarin.Forms.Core.UITests;
#endif

namespace Xamarin.Forms.Controls.Issues
{

	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 6878,
		"ShellItem.Items.Clear() crashes when the ShellItem has bottom tabs", PlatformAffected.All)]

#if UITEST
	[NUnit.Framework.Category(UITestCategories.Shell)]
#endif
	public class Issue6878 : TestShell
	{
		const string ClearShellItems = "ClearShellItems";
		const string StatusLabel = "StatusLabel";

		StackLayout _stackContent;

		protected override void Init()
		{
			_stackContent = new StackLayout()
			{
				Children =
				{
					new Label()
					{
						AutomationId = StatusLabel,
						Text = "Everything is fine 😎"
					}
				}
			};

			_stackContent.Children.Add(BuildClearButton());
			PrepareItemsPage(true);
		}

		Button BuildClearButton()
		{
			return new Button()
			{
				Text = "Click to clear ShellItem.Items",
				Command = new Command(() =>
				{
					Items.Last().Items.Clear();
					Items.Clear();
					PrepareItemsPage();
				}),
				AutomationId = ClearShellItems
			};
		}

		private void PrepareItemsPage(bool firstTime = false)
		{
			CreateContentPage().Content = _stackContent;

			CurrentItem = Items.Last();

			AddTopTab($"{(!firstTime ? "Post clear " : string.Empty)}top tab");
			AddBottomTab("bottom tab");
			Shell.SetBackgroundColor(this, Color.BlueViolet);
		}

#if UITEST
		[Test]
		public void ShellItemItemsClearTests()
		{
			RunningApp.WaitForElement(StatusLabel);
			RunningApp.Tap(ClearShellItems);

			RunningApp.Tap("Post clear top tab");
		}
#endif
	}
}
