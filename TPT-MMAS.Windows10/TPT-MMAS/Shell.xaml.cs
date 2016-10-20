using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TPT_MMAS.Shared.Common.TPT;
using TPT_MMAS.Shared.Interface;
using TPT_MMAS.Shared.Model;
using TPT_MMAS.View;
using TPT_MMAS.View.Devices;
using TPT_MMAS.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TPT_MMAS
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Shell : Page, INotifyPropertyChanged
    {
        private Type currentPage;
        private object passedParameter;

        private Dictionary<UIElement, Type> NavigationDictionary { get; set; }

        private Personnel _currentUser;
        public Personnel CurrentUser
        {
            get { return _currentUser; }
            set { Set(nameof(CurrentUser), ref _currentUser, value); }
        }

        private ShellViewModel VM { get; set; }

        public Shell(Type page, object parameter = null)
        {
            InitializeComponent();
            Loaded += OnShellLoaded;
            Unloaded += OnShellUnloaded;

            VM = DataContext as ShellViewModel;
            CurrentUser = App.LoggedUser;

            currentPage = page;
            passedParameter = parameter;

            ShellFrame.Loaded += ShellFrame_Loaded;
            ShellFrame.Navigated += ShellFrame_Navigated;
            btn_menu.Click += OnHamburgerButtonClick;

            SystemNavigationManager.GetForCurrentView().BackRequested += Shell_BackRequested;
            
            PrepareMainNavigation();
        }

        /// <summary>
        /// Happens when the menu buttton is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHamburgerButtonClick(object sender, RoutedEventArgs e)
        {
            ShellSplitView.IsPaneOpen = (ShellSplitView.IsPaneOpen) ? false : true;
        }

        private void OnShellLoaded(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as INavigable;
            if (vm != null)
                vm.Activate(null);
        }

        private void OnShellUnloaded(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as INavigable;
            if (vm != null)
                vm.Deactivate(null);
        }

        private void PrepareMainNavigation()
        {
            // Start prepping links of controls to pages
            NavigationDictionary = new Dictionary<UIElement, Type>()
            {
                { RB_Patients, typeof(PatientsPage) },
                { RB_Medicines, typeof(MedicinesPage) },
                { RB_Settings, typeof(SettingsPage) }
            };
            
            //Hide stuff when we need to
            var navItems = SP_MainNav.Children;
            foreach (UIElement control in navItems)
            {
                Type value;
                NavigationDictionary.TryGetValue(control, out value);

                if (value == null)
                    control.Visibility = Visibility.Collapsed;
            }
        }

        private void Shell_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (ShellFrame.CanGoBack)
            {
                ShellFrame.GoBack();
                e.Handled = true;
            }
        }

        #region Shell Frame

        private void ShellFrame_Loaded(object sender, RoutedEventArgs e)
        {
            var f = sender as Frame;
            if (f.Content == null)
            {
                f.Navigate(currentPage, passedParameter);
            }
        }

        private void ShellFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (NavigationDictionary.ContainsValue(ShellFrame.CurrentSourcePageType))
            {
                RadioButton rb = NavigationDictionary.FirstOrDefault(x => x.Value == ShellFrame.CurrentSourcePageType).Key as RadioButton;
                rb.IsChecked = true;

                if (ShellFrame.CanGoBack)
                    ShellFrame.BackStack.Clear();
            }

            IRefreshable vm = (ShellFrame.Content as Page).DataContext as IRefreshable;
            VM.CurrentViewModel = vm;

            UpdateAppViewBackButtonVisibility(ShellFrame);
        }

        #endregion

        public static void UpdateAppViewBackButtonVisibility(Frame frame)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = (frame.CanGoBack) ?
                AppViewBackButtonVisibility.Visible :
                AppViewBackButtonVisibility.Collapsed;
        }

        private void OnNavButtonClick(object sender, RoutedEventArgs e)
        {
            RadioButton selectedNav = sender as RadioButton;
            if (NavigationDictionary[selectedNav] != ShellFrame.CurrentSourcePageType)
            {
                ShellFrame.Navigate(NavigationDictionary[selectedNav]);
            }
        }

        private async void OnLogoutButtonClick(object sender, RoutedEventArgs e)
        {
            ContentDialog modal = new ContentDialog();
            modal.Title = "Are you sure you want to log out?";
            modal.PrimaryButtonText = "Logout";
            modal.SecondaryButtonText = "Cancel";
            modal.PrimaryButtonClick += Logout;

            await modal.ShowAsync();
        }

        private void Logout(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            App.LoggedUser = null;
            Window.Current.Content = new LoginPage();
        }

        #region INPC implementation
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool Set<T>(string propertyName, ref T storage, T value)
        {
            if (Equals(storage, value))
                return false;

            storage = value;
            RaisePropertyChanged(propertyName);
            return true;
        }
        #endregion

    }
}
