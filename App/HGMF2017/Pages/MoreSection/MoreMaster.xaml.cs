using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace HGMF2018
{
	public partial class MoreMaster : ContentPage
	{
		public ObservableCollection<MoreItem> Names { get; set; }

		public MoreMaster()
		{
			BindingContext = this;

			Names = new ObservableCollection<MoreItem>() {
				new MoreItem("Tickets", "Tickets"),
				new MoreItem("About Homegrown", "About"),
				new MoreItem("Contact", "PaperPlane"),
				new MoreItem("News", "News"),
				new MoreItem("About this app", "Smartphones"),
				new MoreItem("Privacy Policy", "Privacy"),
				new MoreItem("Settings", "Settings")
			};

			InitializeComponent();
		}

		/// <summary>
		/// The action to take when a list item is tapped.
		/// </summary>
		/// <param name="sender"> The sender.</param>
		/// <param name="e">The ItemTappedEventArgs</param>
		void ItemTapped(object sender, ItemTappedEventArgs e)
		{
			var name = ((MoreItem)e.Item).Name;

			switch (name)
			{
			case "Tickets":
				this.Navigation.PushAsync(new MoreTickets());
				break;
			case "About Homegrown":
				this.Navigation.PushAsync(new MoreAboutHomegrown());
				break;
			case "Contact":
				this.Navigation.PushAsync(new MoreContact());
				break;
			case "News":
				this.Navigation.PushAsync(new MoreNews());
				break;
			case "About this app":
				this.Navigation.PushAsync(new MoreAboutThisApp());
				break;
			case "Privacy Policy":
				this.Navigation.PushAsync(new MorePrivacyPolicy());
				break;
			case "Settings":
				this.Navigation.PushAsync(new MoreSettings());
				break;
			}

			// prevents the list from displaying the navigated item as selected when navigating back to the list
			((ListView)sender).SelectedItem = null;
		}
	}

	public class MoreItem
	{
		public string Name { get; set; }
		public string Icon { get; set; }

		public MoreItem(string name, string icon)
		{
			Name = name;
			Icon = icon;
		}
	}
}
