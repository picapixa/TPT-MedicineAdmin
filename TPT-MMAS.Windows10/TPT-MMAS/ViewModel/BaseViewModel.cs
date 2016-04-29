using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPT_MMAS.ViewModel
{
    public class BaseViewModel : ViewModelBase
    {
        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set { Set(nameof(IsLoading), ref _isLoading, value); }
        }

    }
}
