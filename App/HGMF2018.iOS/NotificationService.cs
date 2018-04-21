using System;
using Foundation;
using ObjCRuntime;
using UserNotifications;

namespace HGMF2018.iOS
{
    public class NotificationService : UNNotificationServiceExtension
    {
        protected NotificationService(NSObjectFlag t) : base(t)
        {
        }

        protected internal NotificationService(IntPtr handle) : base(handle)
        {
        }

		public override void DidReceiveNotificationRequest(UNNotificationRequest request, Action<UNNotificationContent> contentHandler)
		{
            base.DidReceiveNotificationRequest(request, contentHandler);
		}
	}
}
