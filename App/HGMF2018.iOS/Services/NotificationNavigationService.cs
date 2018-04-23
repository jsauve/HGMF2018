using System;
using HGMF2018.Core;
using HGMF2018.iOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(NotificationNavigationService))]
namespace HGMF2018.iOS
{
    public class NotificationNavigationService : INotificationNavigationService
    {
        public event EventHandler NotificationReceived;

        public void OnNotificationReceived()
        {
            NotificationReceived?.Invoke(this, null);
        }
    }
}
