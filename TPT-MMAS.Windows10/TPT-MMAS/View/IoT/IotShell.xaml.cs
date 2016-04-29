using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TPT_MMAS.Shared.Interface;
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

namespace TPT_MMAS.View.IoT
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class IotShell : Page
    {

        public IotShell()
        {
            this.InitializeComponent();
            this.Loaded += IotShell_Loaded;
            this.Unloaded += IotShell_Unloaded;
        }

        private void IotShell_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as INavigable;
            if (vm != null)
                vm.Activate(null);
        }


        private void IotShell_Unloaded(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as INavigable;
            if (vm != null)
                vm.Deactivate(null);
        }

        private void iotShellFrame_Loaded(object sender, RoutedEventArgs e)
        {
            var f = sender as Frame;
            if (f.Content == null)
            {
                f.Navigate(typeof(DebugPage));
            }
        }
    }
}