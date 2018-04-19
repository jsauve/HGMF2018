using System;
using HGMF2018.iOS;
using Foundation;
using UIKit;
using Xamarin.Forms;
using HGMF2018.Core;
using strings = HGMF2018.Core.Constants;
using System.Threading.Tasks;

[assembly: Dependency(typeof(LyftService))]
namespace HGMF2018.iOS
{
    public class LyftService : ILyftService
    {
        public async Task Open()
        {
            if (IsInstalled)
                UIApplication.SharedApplication.OpenUrl(new NSUrl($"lyft://partner={Settings.LYFT_CLIENT_ID}"));
            else
            {
                await DependencyService.Get<IUserDialogService>().ShowConfirmOrCancelDialog(
                    strings.INSTALL_LYFT_TITLE, 
                    strings.INSTALL_LYFT_MESSAGE, 
                    strings.OK, 
                    strings.CANCEL, 
                    () => UIApplication.SharedApplication.OpenUrl(new NSUrl("https://itunes.apple.com/us/app/lyft/id529379082")));
            }

        }

        public bool IsInstalled => UIApplication.SharedApplication.CanOpenUrl(new NSUrl("lyft://"));
    }
}
