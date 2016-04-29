using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TPT_MMAS.Iot.Hardware.Foundation;

namespace TPT_MMAS.Iot.Hardware
{
    public class TrayContainer : INotifyPropertyChanged
    {

        private int _id;

        public int ID
        {
            get { return _id; }
            set { Set(nameof(ID), ref _id, value); }
        }

        private bool _isIndicatorOn;

        public bool IsIndicatorOn
        {
            get { return _isIndicatorOn; }
            set { Set(nameof(IsIndicatorOn), ref _isIndicatorOn, value); }
        }

        private bool _hasItem;


        public bool HasItem
        {
            get { return _hasItem; }
            set { Set(nameof(HasItem), ref _hasItem, value); }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private bool Set<T>(string propertyName, ref T source, T value)
        {
            if (Equals(source, value))
                return false;

            source = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class TrayController 
    {
        private Arduino arduino;

        #region Bindable properties

        private bool _isTrayOpen;

        public bool IsTrayOpen
        {
            get { return _isTrayOpen; }
            set { Set(nameof(IsTrayOpen), ref _isTrayOpen, value, RaiseTrayStatusChanged); }
        }

        private string _rawData;

        public string RawData
        {
            get { return _rawData; }
            set { Set(nameof(RawData), ref _rawData, value); }
        }

        private ObservableCollection<TrayContainer> _traySlots;

        public ObservableCollection<TrayContainer> TraySlots
        {
            get { return _traySlots; }
            set { Set(nameof(TraySlots), ref _traySlots, value, RaiseSlotStateChanged); }
        }


        #endregion

        #region Singleton implementation
        static readonly TrayController _instance = new TrayController();
        public static TrayController Instance
        {
            get { return _instance; }
        }
        #endregion

        TrayController()
        {
            arduino = new Arduino();
            arduino.ContentChanged += Arduino_ContentChanged;

            List<TrayContainer> slots = new List<TrayContainer>();

            for (int i = 0; i < 8; i++)
            {
                var slot = new TrayContainer()
                {
                    ID = i + 1,
                    IsIndicatorOn = false,
                    HasItem = false
                };
                //slot.PropertyChanged += TraySlot_PropertyChanged;
                slots.Add(slot);
            }

            TraySlots = new ObservableCollection<TrayContainer>(slots);
            TraySlots.CollectionChanged += TraySlots_CollectionChanged;
        }

        #region Tray slots event handlers
        private void TraySlots_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (TrayContainer item in e.NewItems)
                    item.PropertyChanged += TraySlot_PropertyChanged;
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (TrayContainer item in e.OldItems)
                    item.PropertyChanged -= TraySlot_PropertyChanged;
            }

        }

        private void TraySlot_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TrayContainer slot = sender as TrayContainer;
        }

        #endregion

        public async void GetStatusAsync()
        {
            await arduino.WriteDataAsync("A");
        }

        /// <summary>
        /// Turns the LED indicator on for a given slot.
        /// </summary>
        /// <param name="n"></param>
        public async Task<bool> EnableLEDAsync(int n)
        {
            try
            {
                if (IsTrayOpen == false)
                {
                    try
                    {
                        if ((Enumerable.Range(1, 8).Contains(n)))
                        {
                            await arduino.WriteDataAsync(n.ToString());
                            //await arduino.WriteToStreamAsync(n.ToString());
                            await Task.Delay(50);

                            TrayContainer slot = TraySlots.Single(s => s.ID == n);
                            return slot.IsIndicatorOn;
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }return false;             
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Sets the indicator status at the TraySlots observable collection.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="state"></param>
        private void SetIndicatorStatus(string response)
        {
            int n = (int)Char.GetNumericValue(response[3]);
            string state = response.Substring(5);

            TrayContainer slot = TraySlots.First(s => s.ID == n);
            TraySlots.Remove(slot);
            slot.IsIndicatorOn = (state == "ON");
            TraySlots.Add(slot);
        }

        /// <summary>
        /// Sets the HasItem property of a TraySlot at TraySlots.
        /// </summary>
        /// <param name="response"></param>
        private void SetSwitchStatus(string response)
        {
            int n = (int)Char.GetNumericValue(response[7]);
            string state = response.Substring(9);

            TrayContainer slot = TraySlots.First(s => s.ID == n);
            TraySlots.Remove(slot);
            slot.HasItem = (state == "ON");
            TraySlots.Add(slot);
        }

        private void UpdateAllTraySlotPresenceData(string response)
        {
            string data = response.Substring(5, 8);
            List<TrayContainer> slots = TraySlots.OrderBy(s => s.ID).ToList();
            
            foreach (TrayContainer item in slots)
            {
                int i = item.ID;

                var currentItem = TraySlots.First(s => s.ID == i);
                TraySlots.Remove(item);

                item.HasItem = (data[i - 1] == 1);

                TraySlots.Add(item);

            }

        }

        private async void Arduino_ContentChanged(object sender, PropertyChangedEventArgs e)
        {
            string content = arduino.StreamContent;

            string[] contentArray = Regex.Split(content, "\r\n");

            foreach (var item in contentArray)
            {
                if (item == "")
                    continue;

                if (item.StartsWith("DATA_TRAY"))
                    IsTrayOpen = (item == "DATA_TRAY_OPEN");
                else if (item.StartsWith("DATA_SW"))
                    SetSwitchStatus(item);
                else if (item.StartsWith("DATA_"))
                    UpdateAllTraySlotPresenceData(item);
                else if (item.StartsWith("LED"))
                    SetIndicatorStatus(item);

                RawData = item;
                await Task.Delay(50);
            }
        }

        #region Property changed implementations
        public PropertyChangedEventHandler PropertyChanged;
        public PropertyChangedEventHandler TrayStatusChanged;
        public PropertyChangedEventHandler SlotStateChanged;

        private void RaiseTrayStatusChanged()
        {
            TrayStatusChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        private void RaiseSlotStateChanged()
        {
            SlotStateChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        private bool Set<T>(string propertyName, ref T storage, T value, Action raiseChanged = null)
        {
            if (Equals(storage, value))
                return false;

            storage = value;
            if (raiseChanged == null)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            else
                raiseChanged.Invoke();

            return true;
        }

        #endregion

    }
}
