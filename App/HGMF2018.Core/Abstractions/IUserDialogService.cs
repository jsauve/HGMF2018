using System;
using System.Threading.Tasks;

namespace HGMF2018.Core.Abstractions
{
    public interface IUserDialogService
    {
        Task ShowConfirmOrCancelDialog(string title, string message, string confirmButtonText, string cancelButtonText, Action confirmAction);
    }
}
