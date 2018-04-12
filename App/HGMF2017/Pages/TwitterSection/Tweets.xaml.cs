using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FFImageLoading.Forms;
using LinqToTwitter;
using Xamarin.Forms;

namespace HGMF2018
{
	public partial class Tweets : ContentPage
	{
		protected TweetsViewModel ViewModel => BindingContext as TweetsViewModel;

		bool IsPresentingModally;

		public Tweets()
		{
			InitializeComponent();

			TweetsListView.ItemAppearing += async (sender, e) => { 
				if (ViewModel.IsBusy || TweetsListView.ItemsSource.ToEnumerable().Count() == 0)
				return;

				var lastItem = TweetsListView.ItemsSource.Cast<TweetWrapper>().OrderBy(x => x.Status.CreatedAt).First();

				if (((TweetWrapper)e.Item).Status.StatusID == lastItem.Status.StatusID && ViewModel.CanLoadMore)
				{
					await ViewModel.ExecuteLoadTweetsCommand();
				}
			};
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			ViewModel.NoNetworkDetected += async (sender, e) => {
				TweetsListView.EndRefresh();
				await App.DisplayNoNetworkAlert(this);
			};

			ViewModel.OnError += async (sender, e) => {
				TweetsListView.EndRefresh();
				await App.DisplayErrorAlert(this);
			};
		}

		protected override async void OnAppearing()
		{
			if (IsPresentingModally)
			{
				IsPresentingModally = false;
				return;
			}

			base.OnAppearing();

			if (ViewModel.IsInitialized)
				return;

			await ViewModel.ExecuteLoadTweetsCommand();
		}

		/// <summary>
		/// The action to take when a list item is tapped.
		/// </summary>
		/// <param name="sender"> The sender.</param>
		/// <param name="e">The ItemTappedEventArgs</param>
		void ItemTapped(object sender, ItemTappedEventArgs e)
		{
			var tweetWrapper = (TweetWrapper)e.Item;

			if (tweetWrapper.HasUrl)
				try
				{
					var url = tweetWrapper.StatusUrl;

					if (url != null)
						Device.OpenUri(new Uri(url));
				}
				catch
				{
					Task.Factory.StartNew(async () => { await App.DisplayNoNetworkAlert(this); });
					
				}

			// prevents the list from displaying the navigated item as selected when navigating back to the list
			((ListView)sender).SelectedItem = null;
		}
	}
}
