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

namespace HGMF2018.Droid
{
    [Activity(Label = "HGMF2018", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        public const string TAG = "MainActivity";

        Lazy<App> _LazyApp = new Lazy<App>(() => new App());

        INotificationNavigationService _NotificationNavigationService;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            if (Intent != null && Intent.Extras != null && Intent.Extras.Size() > 0)
            {
                Log.Debug(TAG, "Received extras in onCreate()");

                Bundle extras = Intent.Extras;

                foreach (var key in extras.KeySet())
                {
                    if (key != null)
                    {
                        Log.Debug(TAG, "Key: {0} Value: {1}", key, extras.GetString(key));
                        if (key == "tweetUrl")
                        {
                            //_NotificationNavigationService.OnNotificationReceived();
                        }
                    }
                }
            }

            UserDialogs.Init(this);

            Forms.Init(this, bundle);

            _NotificationNavigationService = DependencyService.Get<INotificationNavigationService>();

            CachedImageRenderer.Init(true);

            CarouselViewRenderer.Init();

            LoadApplication(_LazyApp.Value);

            //if (Intent != null && Intent.Extras != null && Intent.Extras.Size() > 0)
            //{
            //    Log.Debug(TAG, "Received extras in onCreate()");

            //    Bundle extras = Intent.Extras;

            //    foreach (var key in extras.KeySet())
            //    {
            //        if (key != null)
            //        {
            //            Log.Debug(TAG, "Key: {0} Value: {1}", key, extras.GetString(key));
            //            if (key == "tweetUrl")
            //                _NotificationNavigationService.OnNotificationReceived();
            //        }
            //    }
            //}
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            Log.Debug(TAG, "onNewIntent() called.");

            Intent = intent;

            if (intent != null && intent.Extras != null && intent.Extras.Size() > 0)
            {
                Log.Debug(TAG, "Received extras in onNewIntent()");

                Bundle extras = Intent.Extras;

                foreach (var key in extras.KeySet())
                {
                    if (key != null)
                    {
                        Log.Debug(TAG, "Key: {0} Value: {1}", key, extras.GetString(key));
                        if (key == "tweetUrl")
                            _NotificationNavigationService.OnNotificationReceived();
                    }
                }
            }
        }

        //void NavigateUponNotificationIntent(Intent intent)
        //{
        //    var keySet = intent?.Extras?.KeySet()?.ToList();
        //    if (keySet != null)
        //    {
        //        foreach (var key in keySet)
        //        {
        //            if (key != null)
        //            {
        //                if (key == "tweetUrl")
        //                {
        //                    var value = intent.Extras.GetString(key);
        //                    Log.Debug(TAG, "Key: {0} Value: {1}", key, value);
        //                    _NotificationNavigationService.OnNotificationReceived();

        //                }
        //            }
        //        }
        //    }
        //}

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnStop()
        {
            base.OnStop();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override void OnActivityReenter(int resultCode, Intent data)
        {
            base.OnActivityReenter(resultCode, data);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
        }

        public override void StartActivity(Intent intent)
        {
            base.StartActivity(intent);
        }

        public override void StartActivity(Intent intent, Bundle options)
        {
            base.StartActivity(intent, options);
        }

        public override bool StartActivityIfNeeded(Intent intent, int requestCode, Bundle options)
        {
            return base.StartActivityIfNeeded(intent, requestCode, options);
        }

        public override void StartActivityForResult(Intent intent, int requestCode)
        {
            base.StartActivityForResult(intent, requestCode);
        }

        public override void StartActivityForResult(Intent intent, int requestCode, Bundle options)
        {
            base.StartActivityForResult(intent, requestCode, options);
        }

        public override bool StartActivityIfNeeded(Intent intent, int requestCode)
        {
            return base.StartActivityIfNeeded(intent, requestCode);
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
