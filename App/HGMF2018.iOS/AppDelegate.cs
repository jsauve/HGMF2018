using CarouselView.FormsPlugin.iOS;
using Foundation;
using pyze.xamarin.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using FFImageLoading.Forms.Touch;
using HGMF2018.Core;
using WindowsAzure.Messaging;
using System;
using AudioToolbox;
using ObjCRuntime;
using System.Linq;
using UserNotifications;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter;

namespace HGMF2018.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : FormsApplicationDelegate, IUNUserNotificationCenterDelegate
    {
        SBNotificationHub _Hub;
        INotificationNavigationService _NotificationNavigationService;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
#if !DEBUG
            AppCenter.Start(Settings.AppCenteriOSKey, typeof(Analytics), typeof(Crashes));
#endif

            //#if ENABLE_TEST_CLOUD
            //				Xamarin.Calabash.Start();
            //#endif

            Forms.Init();

            CarouselViewRenderer.Init();

            CachedImageRenderer.Init();

            LoadApplication(new App());

            RegisterForNotifications();

            _NotificationNavigationService = DependencyService.Get<INotificationNavigationService>();

            return base.FinishedLaunching(app, options);
        }

        void RegisterForNotifications()
        {
            // Request notification permissions from the user
            UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert, (approved, err) =>
            {
                InvokeOnMainThread(() =>
                {
                    // Handle approval
                    UIApplication.SharedApplication.RegisterForRemoteNotifications();

                    // Watch for notifications while the app is active
                    UNUserNotificationCenter.Current.Delegate = this;
                });
            });
        }

        // this method is called if a notification is recieved when the app is in the foreground
        [Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
        public void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            // Do something with the notification
            Console.WriteLine("Active Notification: {0}", notification);

            // Tell system to display the notification anyway or use
            // `None` to say we have handled the display locally.
            completionHandler(UNNotificationPresentationOptions.Alert);
            SystemSound.Vibrate.PlayAlertSound();
            SystemSound.Vibrate.PlaySystemSound();
        }

        // this method is called when a notification is tapped
        [Export("userNotificationCenter:didReceiveNotificationResponse:withCompletionHandler:")]
        public void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            _NotificationNavigationService.OnNotificationReceived();
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            var hubName = String.Empty;
            var hubConnectionString = String.Empty;

#if DEBUG
            hubName = Settings.AzureNotificationHubNameSandbox;
            hubConnectionString = Settings.AzureNotificationHubConnectionStringSandbox;
#else
            hubName = Settings.AzureNotificationHubNameProd;
            hubConnectionString = Settings.AzureNotificationHubConnectionStringProd;
#endif

            _Hub = new SBNotificationHub(hubConnectionString, hubName);

            _Hub.UnregisterAllAsync(deviceToken, (error) =>
            {
                if (error != null)
                {
                    Console.WriteLine("Error calling Unregister: {0}", error.ToString());
                    return;
                }

                NSSet tags = null; // create tags if you want
                _Hub.RegisterNativeAsync(deviceToken, tags, (errorCallback) =>
                {
                    if (errorCallback != null)
                        Console.WriteLine("RegisterNativeAsync error: " + errorCallback.ToString());
                });
            });
        }

        public override bool WillFinishLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
//#if !DEBUG
//            Pyze.Initialize(Settings.PyzeiOSKey);
//#endif
            return base.WillFinishLaunching(uiApplication, launchOptions);
        }
    }
}
