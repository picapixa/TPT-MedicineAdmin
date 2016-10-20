using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TPT_MMAS.Shared.Common;
using TPT_MMAS.Shared.Interface;
using TPT_MMAS.Shared.ViewModel;
using TPT_MMAS.Shared.ViewModelMessages;
using TPT_MMAS.View.Dialog;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
            VM.ApiSettings = App.ApiSettings;
            VM.PropertyChanged += VM_PropertyChanged;
        }

        private void VM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (VM.AdmittedPatient != null)
            {
                cmbx_physician.SelectedItem = VM.AdmittedPatient.Admission.AttendingPhysicians[0];
            }
        }

        private async void OnAddPrescriptionClick(object sender, RoutedEventArgs e)
        {
            AddPrescriptionDialog dialog = new AddPrescriptionDialog(VM.AdmittedPatient);
            ContentDialogResult result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                if (dialog.IsRecurring == false)
                {
                    VM.AddPrescriptionToCollection(dialog.AddedItem);
                }
                else
                {
                    await VM.RefreshDataAsync();
                }
            }
        }

        private async void OnRemovePrescriptionClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = (e.OriginalSource as FrameworkElement).DataContext;

            MessageDialog md = new MessageDialog("Removing this medicine from the scheduled prescription cannot be undone.","Are you sure you want to delete this medicine?");
            md.Commands.Add(new UICommand("Delete", (c) =>
            {
                VM.DeletePrescription(selectedItem);
            }));
            md.Commands.Add(new UICommand("Cancel"));

            await md.ShowAsync();            
        }
        
        private async void OnRemovePrescriptionGroupClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = (e.OriginalSource as FrameworkElement).DataContext;

            MessageDialog md = new MessageDialog("All medicines in this scheduled prescription will be removed. This cannot be undone.","Are you sure you want to remove this?");
            md.Commands.Add(new UICommand("Delete all", (c) =>
            {
                VM.DeletePrescriptionGroup(selectedItem);
            }));
            md.Commands.Add(new UICommand("Cancel"));

            await md.ShowAsync();
        }

        private void OnPrescriptionGridRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            FrameworkElement el = sender as FrameworkElement;
            FlyoutBase flyout = FlyoutBase.GetAttachedFlyout(el);
            flyout.ShowAt(el);
        }

        #region navigationHelper states
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            Messenger.Default.Register<ExceptionDialogMessage>(this, HandleExceptionDialogMessage);
            var vm = DataContext as INavigable;
            if (vm != null)
                vm.Activate(e.NavigationParameter);
        }

        private async void HandleExceptionDialogMessage(ExceptionDialogMessage msg)
        {
            MessageDialog md = new MessageDialog(msg.Exception.Message, "An error occured.");
            await md.ShowAsync();
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            Messenger.Default.Unregister<ExceptionDialogMessage>(this);

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

        private void OnPinPrescriptionGroupToggleButtonClick(object sender, RoutedEventArgs e)
        {
            ToggleButton btn = sender as ToggleButton;
            bool isChecked = btn.IsChecked.Value;
            btn.IsChecked = !btn.IsChecked;

            var group = (e.OriginalSource as FrameworkElement).DataContext;            
            VM.SelectPrescriptionGroup(App.LoggedUser, group, isChecked);

            btn.IsChecked = isChecked;
        }

        private void OnDeselectPrescriptionGroupClick(object sender, RoutedEventArgs e)
        {
            var group = (e.OriginalSource as FrameworkElement).DataContext;
            VM.SelectPrescriptionGroup(App.LoggedUser, group, false);

        }
    }
}
