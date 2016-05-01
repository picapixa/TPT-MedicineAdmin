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


        #region INPC handlers
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
        #endregion
    }

    public class TrayController 
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

        private void OnContainerStateChanged(object sender, PropertyChangedEventArgs e)
        {
            //RaiseContainerStateChanged();
        }

        #region Tray slots event handlers
        private void TrayContainers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (TrayContainer item in e.NewItems)
                    //item.PropertyChanged += TraySlot_PropertyChanged;
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

        //private void TraySlot_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    TrayContainer slot = sender as TrayContainer;

        //}

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
            //var container = TrayContainers.First(s => s.ID == n);
            //TrayContainers.Remove(container);

            //container.IsIndicatorOn = (state == "ON");

            //TrayContainers.Add(container);
            //TrayContainers.OrderBy(s => s.ID);
        }

        /// <summary>
        /// Sets the HasItem property of a TraySlot at TraySlots.
        /// </summary>
        /// <param name="response"></param>
        private void SetSwitchStatus(string response)
        {
            int n = (int)Char.GetNumericValue(response[7]);
            string state = response.Substring(9);
            bool isOn = (state == "ON");

            TrayContainers.First(s => s.ID == n).HasItem = (state == "ON");
            if (state == "ON")
                TrayContainers.First(s => s.ID == n).OnStates++;
            else
                TrayContainers.First(s => s.ID == n).OffStates++;

            //var container = TrayContainers.First(s => s.ID == n);
            //TrayContainers.Remove(container);

            //container.HasItem = isOn;
            //if (isOn)
            //    container.OnStates++;
            //else
            //    container.OffStates++;

            //TrayContainers.Add(container);
            //TrayContainers.OrderBy(s => s.ID);
        }

        private void UpdateAllTrayContainerPresenceData(string response)
        {
            string data = response.Substring(5, 8);
            List<TrayContainer> slots = TrayContainers.OrderBy(s => s.ID).ToList();
            
            foreach (TrayContainer item in slots)
            {
                int i = item.ID;

                TrayContainers.First(s => s.ID == i).HasItem = (data[i - 1] == 1);
            }

        }

        /// <summary>
        /// This runs upon receipt of data from the tray controller.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    UpdateAllTrayContainerPresenceData(item);
                else if (item.StartsWith("LED"))
                    SetIndicatorStatus(item);

                RawData = item;
                //await Task.Delay(50);
            }
        }

        #region Property changed implementations
        public PropertyChangedEventHandler PropertyChanged;
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

        private bool Set<T>(string propertyName, ref T storage, T value, Action raiseChanged = null)
        {
            if (Equals(storage, value))
                return false;

            storage = value;
            if (raiseChanged == null)
                RaisePropertyChanged(propertyName);
            else
                raiseChanged.Invoke();

            return true;
        }

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
