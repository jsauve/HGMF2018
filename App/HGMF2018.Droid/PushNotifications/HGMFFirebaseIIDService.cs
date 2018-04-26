using System;
using System.Collections.Generic;
using Android.App;
using Android.Util;
using Firebase.Iid;
using HGMF2018.Core;
using WindowsAzure.Messaging;

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
            hubName = Settings.AzureNotificationHubNameSandbox;
            hubConnectionString = Settings.AzureNotificationHubConnectionStringSandbox;
#else
            hubName = Settings.AzureNotificationHubNameProd;
            hubConnectionString = Settings.AzureNotificationHubConnectionStringProd;
#endif

            _Hub = new NotificationHub(hubName, hubConnectionString, this);

            var tags = new List<string>() { };
            var regID = _Hub.Register(token, tags.ToArray()).RegistrationId;

            Log.Debug(TAG, $"Successful registration of ID {regID}");
        }
    }
}
