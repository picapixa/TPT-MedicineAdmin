using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TPT_MMAS.Shared.Common;
using TPT_MMAS.Shared.Interface;
using TPT_MMAS.View.Dialog;
using TPT_MMAS.ViewModel;
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
    public sealed partial class MedicinesPage : Page
    {
        private NavigationHelper navigationHelper;
        private MedicinesViewModel VM { get; set; }
        private int? CurrentFlyoutStock { get; set; }

        public MedicinesPage()
        {
            InitializeComponent();

            navigationHelper = new NavigationHelper(this);
            navigationHelper.LoadState += NavigationHelper_LoadState;
            navigationHelper.SaveState += NavigationHelper_SaveState;

            VM = DataContext as MedicinesViewModel;
            VM.ApiSettings = App.ApiSettings;
        }
        private async void OnAddMedicineClickAsync(object sender, RoutedEventArgs e)
        {
            AddMedicineDialog dialog = new AddMedicineDialog();
            await dialog.ShowAsync();

            switch (dialog.Result)
            {
                case AddMedicineResult.Success:
                    VM.MedicineInventories.Add(dialog.NewMedicine);
                    break;
                case AddMedicineResult.AddFailed:
                    MessageDialog md = new MessageDialog("Failed adding the new item to the inventory. Please try again.", "POST error");
                    await md.ShowAsync();
                    break;
                case AddMedicineResult.Cancelled:
                    break;
                default:
                    break;
            }

            //pu_addMedicine.IsOpen = true;
        }

        #region navigation helpers
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

        private void OnInventoryRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var element = sender as Grid;
            fl_medInventory.ShowAt(element);
            var inventory = ((FrameworkElement)e.OriginalSource).DataContext;
        }

        private void OnStockFlyoutAddButtonClick(object sender, RoutedEventArgs e)
        {
            var value = int.Parse(tb_flyoutStocks.Text);
            value++;
            tb_flyoutStocks.Text = value.ToString();
        }

        private void OnStockButtonFlyoutMinusButtonClick(object sender, RoutedEventArgs e)
        {
            var value = int.Parse(tb_flyoutStocks.Text);
            value--;
            tb_flyoutStocks.Text = value.ToString();
        }

        private void OnStocksTextBoxChanged(object sender, TextChangedEventArgs e)
        {
            int result;
            if (!int.TryParse(tb_flyoutStocks.Text, out result))
            {
                tb_flyoutStocks.Text = CurrentFlyoutStock.ToString();
                return;
            }
            else if (result <= CurrentFlyoutStock)
            {
                tb_flyoutStocks.Text = CurrentFlyoutStock.ToString();
                btn_flyoutMinus.IsEnabled = false;
                return;
            }
            else
            {
                btn_flyoutMinus.IsEnabled = true;
            }
        }

        private void OnStockFlyoutClosed(object sender, object e)
        {
            CurrentFlyoutStock = null;
        }

        private void OnStockFlyoutOpened(object sender, object e)
        {
            CurrentFlyoutStock = int.Parse(tb_flyoutStocks.Text);
        }

        private void OnAddMedicineTextBoxChanged(object sender, TextChangedEventArgs e)
        {
            var boxName = (sender as TextBox).Name;

            if (boxName == "tbx_amt")
            {
                var content = (sender as TextBox).Text;

                int value;
                if (!int.TryParse(content, out value))
                {
                    (sender as TextBox).Text = "";
                    return;
                }
            }


            //if (!string.IsNullOrEmpty(tbx_gen.Text) &&
            //    !string.IsNullOrEmpty(tbx_dsg.Text) &&
            //    !string.IsNullOrEmpty(tbx_amt.Text))
            //{
            //    IsPrimaryButtonEnabled = true;
            //}
            //else
            //    IsPrimaryButtonEnabled = false;
        }
    }
}
