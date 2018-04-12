using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace HGMF2018
{
	public partial class ScheduleMaster : ContentPage
	{
		HttpClient _HttpClient => new HttpClient();

		bool _CheckedForNewVersion;

		bool IsAppearing;

		protected ScheduleMasterViewModel ViewModel => BindingContext as ScheduleMasterViewModel;

		public ScheduleMaster()
		{
			InitializeComponent();
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			ViewModel.NoNetworkDetected += async (sender, e) => {
				await App.DisplayNoNetworkAlert(this);
				DaysListView.EndRefresh();
			};

			ViewModel.OnError += async (sender, e) => {
				await App.DisplayErrorAlert(this);
				DaysListView.EndRefresh();
			};
		}

		protected override async void OnAppearing()
		{
			if (IsAppearing)
				return;

			IsAppearing = true;

			try
			{
				base.OnAppearing();

				if (!CrossConnectivity.Current.IsConnected)
					await App.DisplayNoNetworkAlert(this);

				if (!_CheckedForNewVersion)
				{
					ViewModel.IsBusy = true;
					var isNewerVersionAvailable = await IsNewerVersionAvailable();
					ViewModel.IsBusy = false;

					if (isNewerVersionAvailable)
					{
						var shouldLaunchAppStore = await DisplayAlert("New version available!", "There is a new version of the HGMF2017 app available. Would you like to get it now?", "Let's do it!", "Nah, maybe later");

						if (shouldLaunchAppStore)
						{
							if (Device.RuntimePlatform == "iOS")
								Device.OpenUri(new Uri(App.iOSAppStoreUrl));

							if (Device.RuntimePlatform == "Android")
								Device.OpenUri(new Uri(App.AndroidAppStoreUrl));
						}
					}

					// ensure we only check once during each app lifecycle (as long as the OS doesnt kill the app)
					_CheckedForNewVersion = true;
				}

				if (ViewModel.IsInitialized)
					return;

				await ViewModel.ExecuteLoadDaysCommand();

			}
			catch (Exception ex)
			{
				ex.ReportError();
				await App.DisplayErrorAlert(this);
			}
			finally
			{
				IsAppearing = false;
			}
		}

		/// <summary>
		/// The action to take when a list item is tapped.
		/// </summary>
		/// <param name="sender"> The sender.</param>
		/// <param name="e">The ItemTappedEventArgs</param>
		void ItemTapped(object sender, ItemTappedEventArgs e)
		{
			Navigation.PushAsync(new ScheduleDetail() { BindingContext = new ScheduleDetailViewModel(ViewModel.Days, (Day)e.Item) });

			// prevents the list from displaying the navigated item as selected when navigating back to the list
			((ListView)sender).SelectedItem = null;
		}

		async Task<bool> IsNewerVersionAvailable()
		{
			bool result = false;

			try
			{
				if (Device.RuntimePlatform == "iOS")
				{
					var availableVersionString = await _HttpClient.GetStringAsync($"https://duluthhomegrown2017.azurewebsites.net/api/CurrentiOSVersion?code={Settings.AZURE_FUNCTION_IOSVERSION_API_KEY}");
					var currentVersionString = DependencyService.Get<IVersionRetrievalService>().Version;

					double availableVersion;
					double currentVersion;
					if (double.TryParse(availableVersionString, out availableVersion) && double.TryParse(currentVersionString, out currentVersion))
					{
						return (availableVersion > currentVersion);
					}
				}

				if (Device.RuntimePlatform == "Android")
				{
					var availableVersionString = await _HttpClient.GetStringAsync($"https://duluthhomegrown2017.azurewebsites.net/api/CurrentAndroidVersion?code={Settings.AZURE_FUNCTION_ANDROIDVERSION_API_KEY}");
					var currentVersionString = DependencyService.Get<IVersionRetrievalService>().Version;

					double availableVersion;
					double currentVersion;
					if (double.TryParse(availableVersionString, out availableVersion) && double.TryParse(currentVersionString, out currentVersion))
					{
						return (availableVersion > currentVersion);
					}

				}
			}
			catch (Exception ex)
			{
				ex.ReportError();
				await App.DisplayErrorAlert(this);
			}

			return result;
		}
	}
}
