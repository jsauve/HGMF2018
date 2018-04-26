using Android.App;
using Android.Content.PM;
using Android.OS;
using CarouselView.FormsPlugin.Android;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using FFImageLoading.Forms.Droid;
using Android.Views;
using System.Linq;
using HGMF2018.Core;
using Acr.UserDialogs;
using Android.Gms.Common;
using Firebase.Messaging;
using Firebase.Iid;
using Android.Util;
using Plugin.CurrentActivity;
using Android.Content;
using System.Collections.Generic;
using System;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace HGMF2018.Droid
{
    [Activity(Label = "HGMF2018", Icon = "@drawable/icon", Theme = "@style/MyTheme", LaunchMode = LaunchMode.SingleTop, MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : FormsAppCompatActivity
    {
        public const string TAG = "MainActivity";

        INotificationNavigationService _NotificationNavigationService;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

#if !DEBUG
            AppCenter.Start(Settings.AppCenterAndroidKey, typeof(Analytics), typeof(Crashes));
            //Pyze.Initialize(this);
#endif

            UserDialogs.Init(this);

            Forms.Init(this, bundle);

            _NotificationNavigationService = DependencyService.Get<INotificationNavigationService>();

            CachedImageRenderer.Init(true);

            CarouselViewRenderer.Init();

            LoadApplication(new App());
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            Log.Debug(TAG, "onNewIntent() called.");

            NavigateUponNotificationIntent(intent);
        }

        void NavigateUponNotificationIntent(Intent intent)
        {
            if (intent?.Extras?.Size() > 0)
            {
                Log.Debug(TAG, "Received extras in onNewIntent()");

                foreach (var key in intent.Extras.KeySet())
                {
                    if (key != null)
                    {
                        Log.Debug(TAG, "Key: {0} Value: {1}", key, intent.Extras.GetString(key));
                        if (key == "tweetUrl")
                            _NotificationNavigationService.OnNotificationReceived();
                    }
                }
            }
        }
        protected override void OnResume()
        {
            base.OnResume();

            // Notes for sanity:
            // The only reason that the intent contains the Extras from the 
            // notification's PendingIntent is because I've manually forwarded
            // the Intent from the SplachActivity, which is set as
            // MainLauncher = true. Background notifications always hit the 
            // launcher activity FIRST. So, you the Intent needs to be forwarded.

            NavigateUponNotificationIntent(Intent);
        }

        public override bool OnMenuOpened(int featureId, IMenu menu)
        {
            // check if the current item id 
            // is equals to the back button id
            if (featureId == 16908332)
            {
                // retrieve the current xamarin forms page instance
                var currentpage = (BackButtonPage)
                Xamarin.Forms.Application.
                Current.MainPage.Navigation.
                NavigationStack.LastOrDefault();

                // check if the page has subscribed to 
                // the custom back button event
                if (currentpage?.CustomBackButtonAction != null)
                {
                    // invoke the Custom back button action
                    currentpage?.CustomBackButtonAction.Invoke();
                    // and disable the default back button action
                    return false;
                }

                // if its not subscribed then go ahead 
                // with the default back button action
                return base.OnMenuOpened(featureId, menu);
            }

            // since its not the back button 
            // click, pass the event to the base
            return base.OnMenuOpened(featureId, menu);
        }

        public override void OnBackPressed()
        {
            // this is not necessary, but in Android user 
            // has both Nav bar back button and
            // physical back button its safe 
            // to cover the both events

            // retrieve the current xamarin forms page instance
            var currentpage = (BackButtonPage)
            Xamarin.Forms.Application.
            Current.MainPage.Navigation.
            NavigationStack.LastOrDefault();

            // check if the page has subscribed to 
            // the custom back button event
            if (currentpage?.CustomBackButtonAction != null)
            {
                currentpage?.CustomBackButtonAction.Invoke();
            }
            else
            {
                base.OnBackPressed();
            }
        }

        // Just temporary. Don't leave this is in final implementation.
        string notificationTextPlaceholderVar = string.Empty;

        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    notificationTextPlaceholderVar = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                else
                {
                    notificationTextPlaceholderVar = "This device is not supported";
                    Finish();
                }
                return false;
            }
            else
            {
                notificationTextPlaceholderVar = "Google Play Services is available.";
                return true;
            }
        }
    }
}
