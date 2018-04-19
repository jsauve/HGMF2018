using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Xamarin.Forms;
using HGMF2018.Core.Services;

[assembly: Dependency(typeof(UserDialogService))]
namespace HGMF2018.Core.Services
{
    public class UserDialogService : IUserDialogService
    {
        public async Task ShowConfirmOrCancelDialog(string title, string message, string confirmButtonText, string cancelButtonText, Action confirmAction)
        {
            var config = new ConfirmConfig() { Title = title, Message = message, OkText = confirmButtonText, CancelText = cancelButtonText };

            var result = await UserDialogs.Instance.ConfirmAsync(config);

            if (result)
                confirmAction.Invoke();
        }

        public async Task ShowAlert(string title, string message, string buttonText)
        {
            var config = new AlertConfig() { Title = title, Message = message, OkText = buttonText };

            await UserDialogs.Instance.AlertAsync(config);
        }
    }
}
