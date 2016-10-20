using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using TPT_MMAS.Shared.ViewModel;

namespace TPT_MMAS.ViewModel
{
    public class ViewModelLocator
    {
        public const string SecondPageKey = "SecondPage";


        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            RegisterInstancesForNavigation();
            RegisterDialogServices();
            RegisterDataServices();
            RegisterViewModels();
        }

        private static void RegisterViewModels()
        {
            SimpleIoc.Default.Register<MedicinesViewModel>();
            SimpleIoc.Default.Register<DevicesMainViewModel>();
            SimpleIoc.Default.Register<PatientsViewModel>();
            SimpleIoc.Default.Register<ShellViewModel>();

            // Shared VMs
            SimpleIoc.Default.Register<PatientProfileViewModel>();
        }

        #region ViewModels as properties
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MedicinesViewModel Medicines => ServiceLocator.Current.GetInstance<MedicinesViewModel>();
        public DevicesMainViewModel DevicesMain => ServiceLocator.Current.GetInstance<DevicesMainViewModel>();
        public PatientsViewModel Patients => ServiceLocator.Current.GetInstance<PatientsViewModel>();
        public PatientProfileViewModel Patient => ServiceLocator.Current.GetInstance<PatientProfileViewModel>();
        public ShellViewModel Shell => ServiceLocator.Current.GetInstance<ShellViewModel>();
        #endregion

        private static void RegisterInstancesForNavigation()
        {
            var nav = new NavigationService();

            //nav.Configure(SecondPageKey, typeof(SecondPage));

            SimpleIoc.Default.Register<INavigationService>(() => nav);
        }

        private static void RegisterDataServices()
        {
            //if (ViewModelBase.IsInDesignModeStatic)
            //{
            //    SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
            //}
            //else
            //{
            //    SimpleIoc.Default.Register<IDataService, MVVMDataService>();
            //}
        }

        private static void RegisterDialogServices()
        {
            SimpleIoc.Default.Register<IDialogService, DialogService>();

        }
    }
}
