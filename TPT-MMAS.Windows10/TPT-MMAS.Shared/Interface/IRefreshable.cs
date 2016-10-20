using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPT_MMAS.Shared.Interface
{
    public interface IRefreshable
    {
        Task RefreshDataAsync();
    }
}
