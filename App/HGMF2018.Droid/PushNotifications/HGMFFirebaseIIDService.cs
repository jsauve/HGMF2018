using Android.App;
using Firebase.Iid;
using Android.Util;
using WindowsAzure.Messaging;
using HGMF2018.Core;
using System.Collections.Generic;
using System;
using Xamarin.Forms;

namespace HGMF2018.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class HGMFFirebaseIIDService : FirebaseInstanceIdService
    {
        const string TAG = "HGMFFirebaseIIDService";
        NotificationHub _Hub;

        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            Log.Debug(TAG, "Refreshed FCM token: " + refreshedToken);
            SendRegistrationToServer(refreshedToken);
        }
        void SendRegistrationToServer(string token)
        {
            var hubName = String.Empty;
            var hubConnectionString = String.Empty;

#if DEBUG
            hubName = Settings.AzureNotifcationHubNameSandbox;
            hubConnectionString = Settings.AzureNotifcationHubConnectionStringSandbox;
#else
            hubName = Settings.AzureNotifcationHubNameProd;
            hubConnectionString = Settings.AzureNotifcationHubConnectionStringProd;
#endif

            _Hub = new NotificationHub(hubName, hubConnectionString, this);

            var tags = new List<string>() { };
            var regID = _Hub.Register(token, tags.ToArray()).RegistrationId;

            Log.Debug(TAG, $"Successful registration of ID {regID}");
        }
    }
}
