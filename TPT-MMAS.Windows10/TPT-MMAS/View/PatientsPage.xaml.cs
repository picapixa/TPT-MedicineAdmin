using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TPT_MMAS.Model;
using TPT_MMAS.Shared.Common;
using TPT_MMAS.Shared.Interface;
using TPT_MMAS.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
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
    public sealed partial class PatientsPage : Page
    {
        private NavigationHelper navigationHelper;

        private PatientsViewModel VM { get; set; }
        

        public PatientsPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;

            navigationHelper = new NavigationHelper(this);
            navigationHelper.LoadState += NavigationHelper_LoadState;
            navigationHelper.SaveState += NavigationHelper_SaveState;

            if (App.IsRunningOnIOT)
            {
                RootGrid.Children.Remove(CB_Patients);
                RootGrid.RowDefinitions.RemoveAt(0);
                Grid.SetRow(gv_patients, 0);
                //Grid.SetRow(MainHub, 0);
            }

            VM = DataContext as PatientsViewModel;

        }

        private void OnPatientItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(PatientProfilePage), e.ClickedItem);
        }

        #region Admissions Text Box event handlers
        private void AdmissionsTextBoxChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                string text = sender.Text.ToUpper();
                sender.ItemsSource = VM.AdmissionsSuggestion.Where(item =>
                    item.Room.Contains(text) ||
                    item.Patient.FirstName.ToUpper().StartsWith(text) ||
                    item.Patient.LastName.ToUpper().StartsWith(text) ||
                    item.Patient.FullNameAbbreviated.ToUpper().StartsWith(text)
               );
            }
        }

        private void OnAdmissionsGotFocus(object sender, RoutedEventArgs e)
        {
            VM.GetAdmissionsFromHospitalAsync();
            ASB_Patients.IsSuggestionListOpen = true;
        }


        private void OnAdmissionsTextBoxSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            sender.Text = (args.SelectedItem as Admission).Patient.FullNameAbbreviated;
        }

        private void OnAdmissionsTextBoxQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                VM.AddAdmissionToImsAsync(args.ChosenSuggestion);

                //navigate to frame
                Frame.Navigate(typeof(PatientProfilePage), args.ChosenSuggestion);
            }
            else
            {
                //try to search the inputted query from the suggestions. if it's not there, welp.
            }
        }
        #endregion

        #region NavigationHelper states

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
