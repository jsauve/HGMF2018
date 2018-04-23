using System;
using HGMF2018.Core;
using HGMF2018.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(NotificationNavigationService))]
namespace HGMF2018.Droid
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