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
using HGMF2018.Core;
using WindowsAzure.Messaging;
using System;

namespace HGMF2018.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : FormsApplicationDelegate
    {
        SBNotificationHub _Hub;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            //#if !DEBUG
            //            MobileCenter.Start(Settings.MOBILECENTER_IOS_APP_ID, typeof(Analytics), typeof(Crashes));
            //#endif

            //#if ENABLE_TEST_CLOUD
            //				Xamarin.Calabash.Start();
            //#endif

            Forms.Init();

            CarouselViewRenderer.Init();

            CachedImageRenderer.Init();

            LoadApplication(new App());

            RegisterForNotifications();

            return base.FinishedLaunching(app, options);
        }

        void RegisterForNotifications()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
                       UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                       new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
            }
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            var hubName = String.Empty;
            var hubConnectionString = String.Empty;

#if DEBUG
            hubName = Settings.AZURE_NOTIFICATIONHUB_NAME_SANDBOX;
            hubConnectionString = Settings.AZURE_NOTIFICATIONHUB_CONNECTIONSTRING_SANDBOX;
#else
            hubName = Settings.AZURE_NOTIFICATIONHUB_NAME_PROD;
            hubConnectionString = Settings.AZURE_NOTIFICATIONHUB_CONNECTIONSTRING_PROD;
#endif

            _Hub = new SBNotificationHub(hubConnectionString, hubName);

            _Hub.UnregisterAllAsync(deviceToken, (error) => {
                if (error != null)
                {
                    Console.WriteLine("Error calling Unregister: {0}", error.ToString());
                    return;
                }

                NSSet tags = null; // create tags if you want
                _Hub.RegisterNativeAsync(deviceToken, tags, (errorCallback) => {
                    if (errorCallback != null)
                        Console.WriteLine("RegisterNativeAsync error: " + errorCallback.ToString());
                });
            });
		}

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            ProcessNotification(userInfo, false);
        }

        void ProcessNotification(NSDictionary options, bool fromFinishedLaunching)
        {
            // Check to see if the dictionary has the aps key.  This is the notification payload you would have sent
            if (null != options && options.ContainsKey(new NSString("aps")))
            {
                //Get the aps dictionary
                NSDictionary aps = options.ObjectForKey(new NSString("aps")) as NSDictionary;

                string alert = string.Empty;

                //Extract the alert text
                // NOTE: If you're using the simple alert by just specifying
                // "  aps:{alert:"alert msg here"}  ", this will work fine.
                // But if you're using a complex alert with Localization keys, etc.,
                // your "alert" object from the aps dictionary will be another NSDictionary.
                // Basically the JSON gets dumped right into a NSDictionary,
                // so keep that in mind.
                if (aps.ContainsKey(new NSString("alert")))
                    alert = (aps[new NSString("alert")] as NSString).ToString();

                //If this came from the ReceivedRemoteNotification while the app was running,
                // we of course need to manually process things like the sound, badge, and alert.
                if (!fromFinishedLaunching)
                {
                    //Manually show an alert
                    if (!string.IsNullOrEmpty(alert))
                    {
                        UIAlertView avAlert = new UIAlertView("Notification", alert, null, "OK", null);
                        avAlert.Show();
                    }
                }
            }
        }

		public override bool WillFinishLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            //#if !DEBUG
            //			Pyze.Initialize(Settings.PYZE_IOS_API_KEY);
            //#endif
            return base.WillFinishLaunching(uiApplication, launchOptions);
        }

        public override void OnActivated(UIApplication uiApplication)
        {
            base.OnActivated(uiApplication);

            //TODO: Put version-checking code here
        }
    }
}
