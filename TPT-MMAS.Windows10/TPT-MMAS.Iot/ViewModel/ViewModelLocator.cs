using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using TPT_MMAS.Iot.Model;

namespace TPT_MMAS.Iot.ViewModel
{
    public class ViewModelLocator
    {

        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            RegisterNavigationService();
            RegisterServices();
            RegisterViewModels();

        }

        public static void RegisterViewModels()
        {
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ShellViewModel>();
            SimpleIoc.Default.Register<DebuggingViewModel>();
        }

        public static void RegisterServices()
        {
            SimpleIoc.Default.Register<IDialogService, DialogService>();

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
            }
            else
            {
                SimpleIoc.Default.Register<IDataService, DataService>();
            }
        }

        #region Navigation services

        public const string SecondPageKey = "SecondPage";
        public static void RegisterNavigationService()
        {
            var nav = new NavigationService();
            //nav.Configure(SecondPageKey, typeof(SecondPage));
            SimpleIoc.Default.Register<INavigationService>(() => nav);

        }

        #endregion

        #region ViewModel properties

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public ShellViewModel Shell => ServiceLocator.Current.GetInstance<ShellViewModel>();
        public DebuggingViewModel Debugging => ServiceLocator.Current.GetInstance<DebuggingViewModel>();
        #endregion
    }
}
