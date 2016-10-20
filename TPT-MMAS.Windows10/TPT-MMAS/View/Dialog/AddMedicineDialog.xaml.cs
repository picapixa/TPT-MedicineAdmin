using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TPT_MMAS.Shared.Model;
using TPT_MMAS.Shared.Model.DataService;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TPT_MMAS.View.Dialog
{
    public enum AddMedicineResult
    {
        Success,
        AddFailed,
        Cancelled
    }

    public sealed partial class AddMedicineDialog : ContentDialog
    {
        public MedicineInventory NewMedicine { get; set; }
        public AddMedicineResult Result { get; set; }

        public AddMedicineDialog()
        {
            this.InitializeComponent();
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            MedicineInventory newItem = new MedicineInventory()
            {
                GenericName = tbx_gen.Text,
                BrandName = tbx_brd.Text,
                Dosage = tbx_dsg.Text,
                StocksLeft = int.Parse(tbx_amt.Text),
                TimeLastAdded = DateTime.Now
            };

            ContentDialogButtonClickDeferral deferral = args.GetDeferral();
            try
            {
                var result = await AddMedicineViaDataServiceAsync(newItem);
                NewMedicine = result;
                Result = AddMedicineResult.Success;
            }
            catch (Exception)
            {
                Result = AddMedicineResult.AddFailed;
            }
            deferral.Complete();
        }

        private async Task<MedicineInventory> AddMedicineViaDataServiceAsync(MedicineInventory newItem)
        {
            try
            {
                ImsDataService imsSvc = new ImsDataService(App.ApiSettings);
                var result = await imsSvc.AddNewMedicineInventoryAsync(newItem);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Result = AddMedicineResult.Cancelled;
        }
        

        private void OnTextBoxChanged(object sender, TextChangedEventArgs e)
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


            if (!string.IsNullOrEmpty(tbx_gen.Text) &&
                !string.IsNullOrEmpty(tbx_dsg.Text) &&
                !string.IsNullOrEmpty(tbx_amt.Text))
            {
                IsPrimaryButtonEnabled = true;
            }
            else
                IsPrimaryButtonEnabled = false;

        }
    }
}
