using System;
namespace HGMF2018.Core
{
    public interface INotificationNavigationService
    {
        event EventHandler NotificationReceived;

        void OnNotificationReceived();
    }
}
