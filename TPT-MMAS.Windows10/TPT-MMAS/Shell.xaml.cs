using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TPT_MMAS.View;
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
    public sealed partial class Shell : Page
    {
        private Dictionary<Control, Type> NavigationDictionary { get; set; }
        
        public Shell()
        {
            this.InitializeComponent();
            this.SetNavigationDictionary();
            ShellFrame.Loaded += ShellFrame_Loaded;
            ShellFrame.Navigated += ShellFrame_Navigated;
            btn_menu.Click += OnHamburgerButtonClick;

            SystemNavigationManager.GetForCurrentView().BackRequested += Shell_BackRequested;
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

        private void ShellFrame_Loaded(object sender, RoutedEventArgs e)
        {
            var f = sender as Frame;
            if (f.Content == null)
            {
                f.Navigate(typeof(PatientsPage));
            }
        }

        private void SetNavigationDictionary()
        {
            NavigationDictionary = new Dictionary<Control, Type>()
            {
                { RB_Patients, typeof(PatientsPage) },
                { RB_Medicines, typeof(MedicinesPage) },
                { RB_Systems, typeof(SystemsPage) },
                { RB_Debug, typeof(DebugPage) }
            };
        }

        private void Shell_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (ShellFrame.CanGoBack)
            {
                ShellFrame.GoBack();
                e.Handled = true;
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


            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = (ShellFrame.CanGoBack) ?
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
    }
}
