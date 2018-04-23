using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using HGMF2018.Core;

namespace HGMF2018.Droid
{
    public class NotificationHelper : ContextWrapper
    {
        //public const string PRIMARY_CHANNEL = "default";
        //public const string SECONDARY_CHANNEL = "second";

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

        int SmallIcon => Android.Resource.Drawable.StatNotifyChat;

        public NotificationHelper(Context context) : base(context)
        {
            var channel1 = new NotificationChannel(Constants.ANDROID_NOTIFICATION_CHANNEL_GENERAL, "Festival Updates", NotificationImportance.Default)
            {
                LightColor = Color.Red,
                LockscreenVisibility = NotificationVisibility.Private
            };
            NotificationManager.CreateNotificationChannel(channel1);

            //var chan2 = new NotificationChannel(SECONDARY_CHANNEL,
            //        GetString(Resource.String.noti_channel_second), NotificationImportance.High);
            //chan2.LightColor = Color.Blue;
            //chan2.LockscreenVisibility = NotificationVisibility.Public;
            //Manager.CreateNotificationChannel(chan2);
        }

        public Notification.Builder GetNotification(String title, String body)
        {
            return new Notification.Builder(ApplicationContext, Constants.ANDROID_NOTIFICATION_CHANNEL_GENERAL)
                     .SetContentTitle(title)
                     .SetContentText(body)
                     .SetSmallIcon(SmallIcon)
                     .SetAutoCancel(true);
        }

        //public Notification.Builder GetNotification2(String title, String body)
        //{
        //    return new Notification.Builder(ApplicationContext, SECONDARY_CHANNEL)
        //             .SetContentTitle(title)
        //             .SetContentText(body)
        //             .SetSmallIcon(SmallIcon)
        //             .SetAutoCancel(true);
        //}

        public void Notify(int id, Notification.Builder notification)
        {
            NotificationManager.Notify(id, notification.Build());
        }

    }
}