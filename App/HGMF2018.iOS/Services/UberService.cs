using System;
using Foundation;
using HGMF2018.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(UberService))]
namespace HGMF2018.iOS
{
	public class UberService : IUberService
	{
		public void OpenUber()
		{
			if (LyftIsInstalled)
                UIApplication.SharedApplication.OpenUrl(new NSUrl($"uber://?action=setPickup&pickup=my_location&client_id={Settings.UBER_CLIENT_ID}"));
			else
                UIApplication.SharedApplication.OpenUrl(new NSUrl($"https://m.uber.com/sign-up?client_id={Settings.UBER_CLIENT_ID}"));

		}

		bool LyftIsInstalled => UIApplication.SharedApplication.CanOpenUrl(new NSUrl("uber://"));
	}
}
