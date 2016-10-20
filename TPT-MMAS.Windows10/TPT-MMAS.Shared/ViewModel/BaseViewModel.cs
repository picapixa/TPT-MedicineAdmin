using GalaSoft.MvvmLight;
using TPT_MMAS.Shared.Common.TPT;

namespace TPT_MMAS.Shared.ViewModel
{
    /// <summary>
    /// The foundation of all view models, with provision for calling API settings and the bindable IsLoading property.
    /// </summary>
    public class BaseViewModel : ViewModelBase
    {
        public ApiSettings ApiSettings { get; set; }

        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set { Set(nameof(IsLoading), ref _isLoading, value); }
        }

    }
}
