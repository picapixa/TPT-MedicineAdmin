using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TPT_MMAS.Shared.Common;
using TPT_MMAS.Shared.Interface;
using TPT_MMAS.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TPT_MMAS.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PatientProfilePage : Page
    {
        private NavigationHelper navigationHelper;
        
        private PatientProfileViewModel VM { get; set; }

        public PatientProfilePage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;

            navigationHelper = new NavigationHelper(this);
            navigationHelper.LoadState += NavigationHelper_LoadState;
            navigationHelper.SaveState += NavigationHelper_SaveState;

            VM = DataContext as PatientProfileViewModel;
        }


        #region navigationHelper states
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            var vm = DataContext as INavigable;
            if (vm != null)
                vm.Activate(e.NavigationParameter);
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            var vm = DataContext as INavigable;
            if (vm != null)
                vm.Deactivate(e.PageState);
        }
        #endregion

        #region On navigated to and from
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }
        #endregion
    }
}
