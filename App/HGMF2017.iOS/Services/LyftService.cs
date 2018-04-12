using System;
using HGMF2018.iOS;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(LyftService))]
namespace HGMF2018.iOS
{
	public class LyftService : ILyftService
	{
        const string LYFT_CLIENT_ID = "";

		public void OpenLyft()
		{
			if (LyftIsInstalled)
				UIApplication.SharedApplication.OpenUrl(new NSUrl($"lyft://partner={LYFT_CLIENT_ID}"));
			else
				UIApplication.SharedApplication.OpenUrl(new NSUrl($"https://www.lyft.com/signup/SDKSIGNUP?clientId={LYFT_CLIENT_ID}&sdkName=iOS_direct"));
				
		}

		bool LyftIsInstalled => UIApplication.SharedApplication.CanOpenUrl(new NSUrl("lyft://"));
	}
}
