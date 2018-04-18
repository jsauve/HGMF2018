using System;
using HGMF2018.iOS;
using Foundation;
using UIKit;
using Xamarin.Forms;
using HGMF2018.Core;
using HGMF2018.Core.Abstractions;
using strings = HGMF2018.Core.Constants;
using System.Threading.Tasks;

[assembly: Dependency(typeof(LyftService))]
namespace HGMF2018.iOS
{
	public class LyftService : ILyftService
	{
		public async Task Open()
		{
            if (LyftIsInstalled)
                UIApplication.SharedApplication.OpenUrl(new NSUrl($"lyft://partner={Settings.LYFT_CLIENT_ID}"));
            else
            {
                var uds = DependencyService.Get<IUserDialogService>();
                await uds.ShowConfirmOrCancelDialog(strings.INSTALL_LYFT_TITLE, strings.INSTALL_LYFT_MESSAGE, strings.OK, strings.CANCEL, () => UIApplication.SharedApplication.OpenUrl(new NSUrl($"https://www.lyft.com/signup/SDKSIGNUP?clientId={Settings.LYFT_CLIENT_ID}&sdkName=iOS_direct")));
            }
				
		}

		bool LyftIsInstalled => UIApplication.SharedApplication.CanOpenUrl(new NSUrl("lyft://"));
	}
}
