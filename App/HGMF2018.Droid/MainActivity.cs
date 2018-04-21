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

namespace HGMF2018.Droid
{
    [Activity(Label = "HGMF2018", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        public const string TAG = "MainActivity";

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);


            UserDialogs.Init(this);

            Forms.Init(this, bundle);

            CachedImageRenderer.Init(true);

            CarouselViewRenderer.Init();

            if (Intent.Extras != null)
            {
                foreach (var key in Intent.Extras.KeySet())
                {
                    if (key != null)
                    {
                        var value = Intent.Extras.GetString(key);
                        Log.Debug(TAG, "Key: {0} Value: {1}", key, value);
                    }
                }
            }

            LoadApplication(new App());
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

        public bool IsPlayServicesAvailable ()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable (this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError (resultCode))
                    notificationTextPlaceholderVar = GoogleApiAvailability.Instance.GetErrorString (resultCode);
                else
                {
                    notificationTextPlaceholderVar = "This device is not supported";
                    Finish ();
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
