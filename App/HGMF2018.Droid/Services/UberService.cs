using System.Threading.Tasks;
using Android.Content;
using Android.Content.PM;
using HGMF2018.Core;
using HGMF2018.Droid;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using strings = HGMF2018.Core.Constants;

[assembly: Dependency(typeof(UberService))]
namespace HGMF2018.Droid
{
    public class UberService : IUberService
    {
        const string UBER_PACKAGE = "com.ubercab";

        public async Task Open()
        {
            if (IsInstalled)
                OpenLink($"uber://?action=setPickup&pickup=my_location&client_id={Settings.UberClientId}");
            else
            {
                await DependencyService.Get<IUserDialogService>().ShowConfirmOrCancelDialog(
                    strings.INSTALL_UBER_TITLE,
                    strings.INSTALL_UBER_MESSAGE,
                    strings.OK, 
                    strings.CANCEL,
                    () => OpenLink("https://play.google.com/store/apps/details?id=com.ubercab"));
            }
        }

        public bool IsInstalled
        {
            get
            {
                PackageManager pm = CrossCurrentActivity.Current.Activity.PackageManager;

                try
                {
                    pm.GetPackageInfo(UBER_PACKAGE, PackageInfoFlags.Activities);
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
            Intent urlIntent = new Intent(Intent.ActionView);
            urlIntent.AddFlags(ActivityFlags.NewTask);
            urlIntent.SetData(Android.Net.Uri.Parse(link));
            CrossCurrentActivity.Current.Activity.StartActivity(urlIntent);
        }
    }
}
