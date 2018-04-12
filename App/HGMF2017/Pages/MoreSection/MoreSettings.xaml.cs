using System;
using System.Collections.Generic;
using FFImageLoading;
using FFImageLoading.Cache;
using Xamarin.Forms;

namespace HGMF2018
{
	public partial class MoreSettings : ContentPage
	{
		public MoreSettings()
		{
			InitializeComponent();

			ClearImageCacheButton.Clicked += async (sender, e) => { 
				await ImageService.Instance.InvalidateCacheAsync(CacheType.All);
				ClearImageCacheButton.Text = "Cache cleared!";
				ClearImageCacheButton.IsEnabled = false;
			};
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			ClearImageCacheButton.IsEnabled = true;
			ClearImageCacheButton.Text = "Clear image cache";
		}
	}
}
