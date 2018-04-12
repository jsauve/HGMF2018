using System;
using Android.Content;
using Android.Content.PM;
using HGMF2018.Droid;
using Plugin.CurrentActivity;
using Xamarin.Forms;

[assembly: Dependency(typeof(UberService))]
namespace HGMF2018.Droid
{
	public class UberService : IUberService
	{
		static readonly string UBER_PACKAGE = "com.ubercab";

		public void OpenUber()
		{
			if (IsPackageInstalled)
				OpenLink($"uber://?action=setPickup&pickup=my_location&client_id={Settings.UBER_CLIENT_ID}");
			else
				OpenLink($"https://m.uber.com/sign-up?client_id={Settings.UBER_CLIENT_ID}");
		}

		void OpenLink(string link)
		{
			Intent playStoreIntent = new Intent(Intent.ActionView);
			playStoreIntent.AddFlags(ActivityFlags.NewTask);
			playStoreIntent.SetData(Android.Net.Uri.Parse(link));
			CrossCurrentActivity.Current.Activity.StartActivity(playStoreIntent);
		}

		bool IsPackageInstalled
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
	}
}
