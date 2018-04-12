using System;
using Android.Content;
using Android.Content.PM;
using HGMF2018.Droid;
using Plugin.CurrentActivity;
using Xamarin.Forms;

[assembly: Dependency(typeof(LyftService))]
namespace HGMF2018.Droid
{
	public class LyftService : ILyftService
	{
		static readonly string LYFT_PACKAGE = "me.lyft.android";

        const string LYFT_CLIENT_ID = "";

		public void OpenLyft()
		{
			if (IsPackageInstalled)
				OpenLink("lyft://");
			else
				OpenLink($"https://www.lyft.com/signup/SDKSIGNUP?clientId={LYFT_CLIENT_ID}&sdkName=android_direct");
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
	}
}
