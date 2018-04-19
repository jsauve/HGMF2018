using System;
using System.Threading.Tasks;

namespace HGMF2018.Core
{
    public interface IUberService
    {
        Task Open();
        bool IsInstalled { get; }
    }
}
