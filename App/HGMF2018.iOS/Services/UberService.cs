using System;
using System.Threading.Tasks;
using Foundation;
using HGMF2018.Core;
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
            if (IsInstalled)
                UIApplication.SharedApplication.OpenUrl(new NSUrl($"uber://?action=setPickup&pickup=my_location&client_id={Settings.UBER_CLIENT_ID}"));
            else
            {
                await DependencyService.Get<IUserDialogService>().ShowConfirmOrCancelDialog(
                    strings.INSTALL_UBER_TITLE, 
                    strings.INSTALL_UBER_MESSAGE, 
                    strings.OK, 
                    strings.CANCEL, 
                    () => UIApplication.SharedApplication.OpenUrl(new NSUrl("https://itunes.apple.com/us/app/uber/id368677368")));
            }
        }

        public bool IsInstalled => UIApplication.SharedApplication.CanOpenUrl(new NSUrl("uber://"));
    }
}
