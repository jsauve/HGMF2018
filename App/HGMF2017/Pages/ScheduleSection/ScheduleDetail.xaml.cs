using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace HGMF2018
{
	public partial class ScheduleDetail : ContentPage
	{
		protected ScheduleDetailViewModel ViewModel => BindingContext as ScheduleDetailViewModel;

		public ScheduleDetail()
		{
			InitializeComponent();

			ByVenueButton.Clicked += (sender, e) => {
				SortedByVenueContainer.IsVisible = true;
				SortedByTimeContainer.IsVisible = false;
				ByVenueButton.BackgroundColor = Color.LightGray;
				ChronologicalButton.BackgroundColor = Color.White;
			};

			ChronologicalButton.Clicked += (sender, e) => {
				SortedByVenueContainer.IsVisible = false;
				SortedByTimeContainer.IsVisible = true;
				ByVenueButton.BackgroundColor = Color.White;
				ChronologicalButton.BackgroundColor = Color.LightGray;
			};

			SortedByVenueCarousel.PositionSelected += (sender, e) => {
                SortedByTimeCarousel.Position = e.NewValue;
                ViewModel.SelectedDay = ViewModel.Days[e.NewValue];
				Title = ViewModel.SelectedDay.Date;
			};

			SortedByTimeCarousel.PositionSelected += (sender, e) => { 
                SortedByVenueCarousel.Position = e.NewValue;
                ViewModel.SelectedDay = ViewModel.Days[e.NewValue];
				Title = ViewModel.SelectedDay.Date;
			};
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			var selectedItem = ViewModel.Days.Single(x => x.Date == ViewModel.SelectedDay.Date);

			SortedByVenueCarousel.Position = ViewModel.Days.IndexOf(selectedItem);
			SortedByTimeCarousel.Position = ViewModel.Days.IndexOf(selectedItem);
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
		}

		/// <summary>
		/// The action to take when a list item is tapped.
		/// </summary>
		/// <param name="sender"> The sender.</param>
		/// <param name="e">The ItemTappedEventArgs</param>
		void ItemTapped(object sender, ItemTappedEventArgs e)
		{
			// prevents the list from displaying the navigated item as selected when navigating back to the list
			((ListView)sender).SelectedItem = null;
		}
	}
}
