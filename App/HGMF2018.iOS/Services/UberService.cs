using System;
using System.Threading.Tasks;
using Foundation;
using HGMF2018.Core;
using HGMF2018.Core.Abstractions;
using HGMF2018.iOS;
using UIKit;
using Xamarin.Forms;
using strings = HGMF2018.Core.Constants;

[assembly: Dependency(typeof(UberService))]
namespace HGMF2018.iOS
{
	public class UberService : IUberService
	{
		public async Task Open()
		{
            if (UberAppIsInstalled)
                UIApplication.SharedApplication.OpenUrl(new NSUrl($"uber://?action=setPickup&pickup=my_location&client_id={Settings.UBER_CLIENT_ID}"));
            else
            {
                var uds = DependencyService.Get<IUserDialogService>();
                await uds.ShowConfirmOrCancelDialog(strings.INSTALL_UBER_TITLE, strings.INSTALL_UBER_MESSAGE, strings.OK, strings.CANCEL, () => UIApplication.SharedApplication.OpenUrl(new NSUrl($"https://m.uber.com/sign-up?client_id={Settings.UBER_CLIENT_ID}")));
            }
		}

        bool UberAppIsInstalled => UIApplication.SharedApplication.CanOpenUrl(new NSUrl("uber://"));
	}
}
