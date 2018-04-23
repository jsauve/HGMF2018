using System;
using Android.App;
using Android.Content;
using Android.Media;
using Android.Util;
using Firebase.Messaging;
using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Graphics;
using Plugin.CurrentActivity;
using HGMF2018.Core;

namespace HGMF2018.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class HGMFFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "HGMFFirebaseMessagingService";

        NotificationManager _NotificationManager;
        NotificationManager NotificationManager
        {
            get
            {
                if (_NotificationManager == null)
                    _NotificationManager = (NotificationManager)GetSystemService(NotificationService);
                return _NotificationManager;
            }
        }

        NotificationChannel _FestivalUpdatesChannel;
        NotificationChannel FestivalUpdatesChannel
        {
            get
            {
                if (_FestivalUpdatesChannel == null)
                {
                    _FestivalUpdatesChannel = new NotificationChannel(Constants.ANDROID_NOTIFICATION_CHANNEL_GENERAL, "Festival Updates", NotificationImportance.Default)
                    {
                        LightColor = Color.Red,
                        LockscreenVisibility = NotificationVisibility.Private
                    };
                    _FestivalUpdatesChannel.EnableLights(true);
                    _FestivalUpdatesChannel.EnableVibration(true);
                    var audioAttributes = (new AudioAttributes.Builder().SetUsage(AudioUsageKind.Notification).SetContentType(AudioContentType.Sonification)).Build();
                    _FestivalUpdatesChannel.SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification), audioAttributes);
                    NotificationManager.CreateNotificationChannel(_FestivalUpdatesChannel);
                    _FestivalUpdatesChannel = NotificationManager.GetNotificationChannel(_FestivalUpdatesChannel.Id);
                }
                return _FestivalUpdatesChannel;
            }
        }

        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);

            Log.Debug(TAG, "From: " + message.From);

            if (message.GetNotification() != null)
            {
                //These is how most messages will be received
                Log.Debug(TAG, "Notification Message Body: " + message.GetNotification().Body);
                SendNotification(message.GetNotification().Body, message.Data);
            }
            else
            {
                //Only used for debugging simple payloads sent from the Azure portal
                SendNotification(message.Data.Values.First());
            }
        }

        void SendNotification(string messageBody, IDictionary<string, string> data = null)
        {
            var intent = new Intent(this, typeof(MainActivity));

            intent.SetFlags(ActivityFlags.ClearTop);

            if (data != null)
                foreach (string key in data.Keys)
                    intent.PutExtra(key, data[key]);

            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.CancelCurrent);

            var notificationBuilder = new Notification.Builder(this, FestivalUpdatesChannel.Id)
                .SetSmallIcon(Resource.Drawable.icon_notification)
                .SetContentTitle("Cluck, cluck...news from The Homegrown Chicken!")
                .SetContentText(messageBody)
                .SetContentIntent(pendingIntent);

            var notificationManager = NotificationManager.FromContext(this);
            notificationManager.Notify(0, notificationBuilder.Build());


            //Settings.FestivalUpdateNotificationPending = true;
        }

        void Vibrate()
        {
            var vibrator = (Vibrator)ApplicationContext.GetSystemService(VibratorService);
            if ((int)Build.VERSION.SdkInt >= 26)
            {
                vibrator.Vibrate(VibrationEffect.CreateOneShot(150, 10));
            }
            else
            {
                vibrator.Vibrate(150);
            }
        }
    }
}
