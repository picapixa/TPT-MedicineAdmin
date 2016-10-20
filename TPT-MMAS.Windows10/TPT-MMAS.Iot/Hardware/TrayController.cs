using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TPT_MMAS.Iot.Hardware.Foundation;
using TPT_MMAS.Shared.Common;
using TPT_MMAS.Shared.Common.TPT;

namespace TPT_MMAS.Iot.Hardware
{
    public class TrayContainer : NotifyPropertyChanged
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

        private int _onStates;

        public int OnStates
        {
            get { return _onStates; }
            set { Set(nameof(OnStates), ref _onStates, value); }
        }


        private int _offStates;

        public int OffStates
        {
            get { return _offStates; }
            set { Set(nameof(OffStates), ref _offStates, value); }
        }
    }

    public class TrayController : NotifyPropertyChanged
    {
        private Arduino arduino;

        #region Bindable properties

        private bool _isTrayOpen;
        public bool IsTrayOpen
        {
            get { return _isTrayOpen; }
            set { Set(nameof(IsTrayOpen), ref _isTrayOpen, value); }
        }

        private string _rawData;
        public string RawData
        {
            get { return _rawData; }
            set { Set(nameof(RawData), ref _rawData, value); }
        }

        private string _containersRawStatus;
        public string ContainersRawStatus
        {
            get { return _containersRawStatus; }
            set { Set(nameof(ContainersRawStatus), ref _containersRawStatus, value, checkForEquality: false); }
        }
        
        private ObservableCollection<TrayContainer> _trayContainers;
        public ObservableCollection<TrayContainer> TrayContainers
        {
            get { return _trayContainers; }
            set { Set(nameof(TrayContainers), ref _trayContainers, value); }
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
                slot.PropertyChanged += OnContainersChanged;
                slots.Add(slot);
            }

            TrayContainers = new ObservableCollection<TrayContainer>(slots);
            TrayContainers.CollectionChanged += TrayContainers_CollectionChanged;
        }

        #region Tray slots event handlers
        private void TrayContainers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (TrayContainer item in e.NewItems)
                    item.PropertyChanged += OnContainersChanged;
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (TrayContainer item in e.OldItems)
                    item.PropertyChanged -= OnContainersChanged;
            }
        }

        private void OnContainersChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(e.PropertyName);
        }

        #endregion

        /// <summary>
        /// Gets the latest presence status of the tray controllers.
        /// </summary>
        /// <returns>A raw string response from the tray controller.</returns>
        public async Task<string> GetPresenceStatusAsync()
        {
            await arduino.WriteDataAsync("A");
            return ContainersRawStatus;
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
                            await Task.Delay(50);

                            TrayContainer slot = TrayContainers.Single(s => s.ID == n);
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

            TrayContainers.First(s => s.ID == n).IsIndicatorOn = (state == "ON");
        }

        /// <summary>
        /// Sets the HasItem property of a TraySlot at TraySlots.
        /// </summary>
        /// <param name="response"></param>
        private void SetSwitchStatus(string response)
        {
            int n = (int)Char.GetNumericValue(response[7]);
            string state = response.Substring(9);
            //bool isOn = (state == "ON");

            TrayContainers.First(s => s.ID == n).HasItem = (state == "ON");
            if (state == "ON")
                TrayContainers.First(s => s.ID == n).OnStates++;
            else
                TrayContainers.First(s => s.ID == n).OffStates++;
        }

        private void UpdateAllTrayContainerPresenceData(string response)
        {
            string data = response.Substring(5, 8);
            List<TrayContainer> slots = TrayContainers.OrderBy(s => s.ID).ToList();

            foreach (TrayContainer item in slots)
            {
                int i = item.ID;
                int hasItem = (int)char.GetNumericValue(data[i - 1]);

                TrayContainers.First(s => s.ID == i).HasItem = (hasItem == 0);

            }

        }

        /// <summary>
        /// This runs upon receipt of data from the tray controller.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Arduino_ContentChanged(object sender, PropertyChangedEventArgs e)
        {
            string content = arduino.StreamContent;

            string[] contentArray = Regex.Split(content, "\r\n");

            foreach (var item in contentArray)
            {
                if (item == "")
                    continue;

                Debug.WriteLine("TrayController.RawData: " + item);

                if (item.StartsWith("DATA_TRAY"))
                    IsTrayOpen = (item == "DATA_TRAY_OPEN");
                else if (item.StartsWith("DATA_SW"))
                    SetSwitchStatus(item);
                else if (item.StartsWith("DATA_"))
                {
                    ContainersRawStatus = item;
                    UpdateAllTrayContainerPresenceData(item);
                }
                else if (item.StartsWith("LED"))
                    SetIndicatorStatus(item);
                
                RawData = item;
            }
        }

        #region Property changed implementations
        //public PropertyChangedEventHandler TrayStatusChanged;
        //public PropertyChangedEventHandler ContainersChanged;
        //public PropertyChangedEventHandler ContainerStateChanged;

        //private void RaiseTrayStatusChanged()
        //{
        //    TrayStatusChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        //}

        //private void RaiseContainersChanged()
        //{
        //    ContainersChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        //}

        //private void RaiseContainerStateChanged()
        //{
        //    ContainerStateChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        //}


        #endregion

    }
}
