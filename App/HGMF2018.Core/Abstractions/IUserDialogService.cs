using System;
using System.Threading.Tasks;

namespace HGMF2018.Core
{
    public interface IUserDialogService
    {
        Task ShowConfirmOrCancelDialog(string title, string message, string confirmButtonText, string cancelButtonText, Action confirmAction);
        Task ShowAlert(string title, string message, string buttonText);
    }
}
