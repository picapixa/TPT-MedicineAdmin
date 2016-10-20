using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPT_MMAS.Shared.Interface;
using TPT_MMAS.Shared.Model;
using TPT_MMAS.Shared.Model.DataService;
using TPT_MMAS.Shared.ViewModel;

namespace TPT_MMAS.ViewModel
{
    public class MedicinesViewModel : BaseViewModel, INavigable
    {
        #region bindable properties
        private ObservableCollection<MedicineInventory> _medicineInventories;

        public ObservableCollection<MedicineInventory> MedicineInventories
        {
            get { return _medicineInventories; }
            set { Set(nameof(MedicineInventories), ref _medicineInventories, value); }
        }
        #endregion

        private ImsDataService imsSvc;

        private void LoadSampleData()
        {
            var dummyData = new List<MedicineInventory>()
            {
                new MedicineInventory()
                {
                    ID = 1,
                    GenericName = "Paracetamol",
                    BrandName = "Biogesic",
                    Dosage = "500mg",
                    StocksLeft = 10,
                    TimeLastAdded =  new DateTime(2016, 4,7, 15, 14, 00)
                },
                new MedicineInventory()
                {
                    ID = 2,
                    GenericName = "Ibuprofen",
                    BrandName = "Advil",
                    Dosage = "200mg",
                    StocksLeft = 20,
                    TimeLastAdded = new DateTime(2016, 4, 30, 14, 30, 00)

                }
            };

            MedicineInventories = new ObservableCollection<MedicineInventory>(dummyData);
        }

        private async void GetMedicinesAsync()
        {
            List<MedicineInventory> medicines = await imsSvc.GetMedicineInventoryListAsync();
            MedicineInventories = new ObservableCollection<MedicineInventory>(medicines.OrderBy(inv => inv.GenericName));

        }

        public void Activate(object parameter)
        {
            //LoadSampleData();
            imsSvc = new ImsDataService(ApiSettings);
            GetMedicinesAsync();
        }

        public void Deactivate(object parameter)
        {
        }
    }
}
