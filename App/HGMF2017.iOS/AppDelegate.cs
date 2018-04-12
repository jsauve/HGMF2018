using CarouselView.FormsPlugin.iOS;
using Foundation;
using pyze.xamarin.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;
using FFImageLoading.Forms.Touch;

namespace HGMF2018.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
#if !DEBUG
            MobileCenter.Start(Settings.MOBILECENTER_IOS_APP_ID, typeof(Analytics), typeof(Crashes));
#endif

#if ENABLE_TEST_CLOUD
				Xamarin.Calabash.Start();
#endif

			Forms.Init();

			CarouselViewRenderer.Init();

			CachedImageRenderer.Init();

			LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}

		public override bool WillFinishLaunching(UIApplication uiApplication, NSDictionary launchOptions)
		{
#if !DEBUG
			Pyze.Initialize(Settings.PYZE_IOS_API_KEY);
#endif
			return base.WillFinishLaunching(uiApplication, launchOptions);
		}

		public override void OnActivated(UIApplication uiApplication)
		{
			base.OnActivated(uiApplication);

            //TODO: Put version-checking code here
		}
	}
}
