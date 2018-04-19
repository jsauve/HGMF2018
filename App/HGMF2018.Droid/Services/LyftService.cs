using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Content.PM;
using HGMF2018.Core;
using HGMF2018.Droid;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using strings = HGMF2018.Core.Constants;

[assembly: Dependency(typeof(LyftService))]
namespace HGMF2018.Droid
{
    public class LyftService : ILyftService
    {
        const string LYFT_PACKAGE = "me.lyft.android";

        public async Task Open()
        {
            if (IsInstalled)
                OpenLink("lyft://");
            else
            {
                await DependencyService.Get<IUserDialogService>().ShowConfirmOrCancelDialog(
                    strings.INSTALL_LYFT_TITLE, 
                    strings.INSTALL_LYFT_MESSAGE, 
                    strings.OK, 
                    strings.CANCEL, 
                    () => OpenLink("https://play.google.com/store/apps/details?id=me.lyft.android"));
            }
        }

        public bool IsInstalled
        {
            get
            {
                PackageManager pm = CrossCurrentActivity.Current.Activity.PackageManager;

                try
                {
                    pm.GetPackageInfo(LYFT_PACKAGE, PackageInfoFlags.Activities);
                    return true;
                }
                catch
                {
                    // intentionally ducked
                }

                return false;
            }
        }

        void OpenLink(string link)
        {
            Intent playStoreIntent = new Intent(Intent.ActionView);
            playStoreIntent.AddFlags(ActivityFlags.NewTask);
            playStoreIntent.SetData(Android.Net.Uri.Parse(link));
            CrossCurrentActivity.Current.Activity.StartActivity(playStoreIntent);
        }
    }
}
