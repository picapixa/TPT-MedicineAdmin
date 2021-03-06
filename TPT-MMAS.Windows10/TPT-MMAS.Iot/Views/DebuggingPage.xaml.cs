﻿using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TPT_MMAS.Iot.Hardware;
using TPT_MMAS.Iot.Hardware.Foundation;
using TPT_MMAS.Iot.ViewModel;
using TPT_MMAS.Shared.Common;
using TPT_MMAS.Shared.Interface;
using Windows.Devices.SerialCommunication;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TPT_MMAS.Iot.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DebuggingPage : Page
    {
        private NavigationHelper navigationHelper;

        private DebuggingViewModel VM { get; set; }
        
        public DebuggingPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;

            navigationHelper = new NavigationHelper(this);
            navigationHelper.LoadState += NavigationHelper_LoadState;
            navigationHelper.SaveState += NavigationHelper_SaveState;

            VM = DataContext as DebuggingViewModel;
        }

        #region Page navigation methods
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }
        #endregion

        #region Tray testing

        private async void OnTrayButtonClick(object sender, RoutedEventArgs e)
        {
            ToggleButton btn = sender as ToggleButton;

            if (btn.IsChecked == true)
            {
                int n = int.Parse(btn.Content as string);
                btn.IsChecked = false;
                await VM.EnableLedAsync(n);
                btn.IsChecked = true;

                tb_statusArea.Text = $@"Tray {n} clicked with value {btn.IsChecked}";
            }
        }
        #endregion

        #region Messaging

        #endregion

        #region RF methods

        private void OnRfidTabLoaded(object sender, RoutedEventArgs e)
        {
            if (App.PluggedDevice == Device.Prototype)
                VM.LoadRfidReaderAsync();
        }



        #endregion

        private void OnSoundCheckPlayButtonCheckedChanged(object sender, RoutedEventArgs e)
        {
            ToggleButton tb = sender as ToggleButton;

            if (tb.IsChecked == true)
            {
                me_soundCheck.Play();
                tb.Content = "Stop";
            }
            else
            {
                me_soundCheck.Stop();
                tb.Content = "Play";
            }
        }

        private void OnMediaPlayerMediaEnded(object sender, RoutedEventArgs e)
        {
            if (tbtn_playback.IsChecked == true)
                tbtn_playback.IsChecked = false;
        }
    }
}
