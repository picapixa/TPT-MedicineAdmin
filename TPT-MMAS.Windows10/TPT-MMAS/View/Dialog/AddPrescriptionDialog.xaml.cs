using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

    public sealed partial class AddPrescriptionDialog : ContentDialog, INotifyPropertyChanged
    {
        private ObservableCollection<MedicineInventory> _suggestedMedicines;

        public ObservableCollection<MedicineInventory> SuggestedMedicines
        {
            get { return _suggestedMedicines; }
            set { Set(nameof(SuggestedMedicines), ref _suggestedMedicines, value); }
        }

        private AdmittedPatient Patient { get; set; }
        public Prescription AddedItem { get; set; }
        public List<Prescription> AddedItems { get; set; }
        public bool IsRecurring { get; set; } = false;

        public AddPrescriptionDialog(AdmittedPatient patient)
        {
            InitializeComponent();
            Loaded += OnDialogLoaded;
            Patient = patient;

            var defaultDate = DateTime.Now.AddHours(1);
            cdp_startDate.Date = defaultDate.Date;
            tp_startTime.Time = new TimeSpan(DateTime.Now.Hour + 1, 0, 0);
        }

        private async void OnDialogLoaded(object sender, RoutedEventArgs e)
        {
            ImsDataService imsSvc = new ImsDataService(App.ApiSettings);
            var inventory = await imsSvc.GetMedicineInventoryListAsync();
            SuggestedMedicines = new ObservableCollection<MedicineInventory>(inventory);
        }
        
        private void OnMedicinesComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ComboBox;
            it_amount.MaxValue = (cb.SelectedItem as MedicineInventory).StocksLeft;

            IsPrimaryButtonEnabled = true;
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var addedItem = new Prescription()
            {
                Medicine = cb_medicine.SelectedItem as MedicineInventory,
                Amount = it_amount.Value,
                AddedBy = App.LoggedUser.Username,
                AddedOn = DateTime.Now
            };
            addedItem.Schedule = ConvertFromDateTimeOffset(cdp_startDate.Date.Value.Add(tp_startTime.Time));
            
            int uploadMode = pv_options.SelectedIndex;
            ImsDataService imsSvc = new ImsDataService(App.ApiSettings);

            ContentDialogButtonClickDeferral def = args.GetDeferral();
            if (uploadMode == 0)
            {
                var item = await imsSvc.AddNewPrescriptionAsync(Patient.ID, addedItem);
                AddedItem = item;
            }
            else
            {
                var items = await imsSvc.SetRecurringPrescriptionsAsync(Patient.ID, addedItem, it_times.Value, it_days.Value);
                AddedItems = items;
                IsRecurring = true;
            }
            def.Complete();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
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
        
        static DateTime ConvertFromDateTimeOffset(DateTimeOffset dateTime)
        {
            if (dateTime.Offset.Equals(TimeSpan.Zero))
                return dateTime.UtcDateTime;
            else if (dateTime.Offset.Equals(TimeZoneInfo.Local.GetUtcOffset(dateTime.DateTime)))
                return DateTime.SpecifyKind(dateTime.DateTime, DateTimeKind.Local);
            else
                return dateTime.DateTime;
        }
    }
}
