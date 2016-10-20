using Microsoft.IoT.Lightning.Providers;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using Windows.Devices;
using Windows.Devices.Gpio;

namespace TPT_MMAS.Iot.Hardware
{
    public enum Pulse
    {
        Data0 = 0,
        Data1 = 1
    }

    public class WiegandReader : IDisposable
    {
        private const double TIME_DEFAULTDEBOUNCE = 0; // in ms
        private const string DEBUG_CAT = "WiegandReader";
        private TimeSpan TIME_IDLEFLAG = TimeSpan.FromSeconds(1);

        private int _d0;
        private int _d1;
        private int bitCount;
        private bool isAddBitMethodLocked = false;
        private long timeLastBit;
        private ulong valueBuffer;
        
        private TimeSpan debounceTimeout;
        private GpioPin data0;
        private GpioPin data1;

        private Stopwatch swReadingTimer;
        private Timer timerWatcher;

        private GpioController Gpio { get; set; }
        
        private ulong _rfData = 0;
        public ulong RfData
        {
            get { return _rfData; }
            private set
            {
                _rfData = value;
                RaiseRfDataReceived();
            }
        }

        public ulong SiteCode { get; set; }

        public ulong UserCode { get; set; }

        private bool _isEnabled = false;

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (Equals(_isEnabled, value))
                    return;

                _isEnabled = value;
                RaiseIsEnabledChanged();
            }
        }


        public WiegandReader(int d0, int d1, bool isEnabledByDefault = true, TimeSpan? debounce = null)
        {
            _d0 = d0;
            _d1 = d1;

            debounceTimeout = (debounce == null) ?
                TimeSpan.FromMilliseconds(TIME_DEFAULTDEBOUNCE) :
                debounce.Value;

            Initialize(isEnabledByDefault);
        }

        public void Initialize(bool isEnabledByDefault)
        {
            if (LightningProvider.IsLightningEnabled)
            {
                LowLevelDevicesController.DefaultProvider = LightningProvider.GetAggregateProvider();
            }

            Gpio = GpioController.GetDefault();
            if (Gpio == null)
                return;

            //set pins d0 and d1 as input
            data0 = Gpio.OpenPin(_d0);
            data0.SetDriveMode(GpioPinDriveMode.InputPullUp);
            data0.DebounceTimeout = debounceTimeout;

            data1 = Gpio.OpenPin(_d1);
            data1.SetDriveMode(GpioPinDriveMode.InputPullUp);
            data1.DebounceTimeout = debounceTimeout;

            IsEnabledChanged += OnIsEnabledChanged;
            IsEnabled = isEnabledByDefault;
            
            swReadingTimer = new Stopwatch();
            Debug.WriteLine("Initialized", DEBUG_CAT);
        }

        private void OnIsEnabledChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsEnabled)
            {
                data0.ValueChanged += Data0_ValueChanged;
                data1.ValueChanged += Data1_ValueChanged;
                Debug.WriteLine("Enabled", DEBUG_CAT);
            }
            else
            {
                data0.ValueChanged -= Data0_ValueChanged;
                data1.ValueChanged -= Data1_ValueChanged;
                Debug.WriteLine("Disabled", DEBUG_CAT);
            }
        }

        void Data0_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            AddBit(args.Edge);
        }

        void Data1_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            AddBit(args.Edge, Pulse.Data1);
        }

        void AddBit(GpioPinEdge edge, Pulse pulse = Pulse.Data0)
        {
            if (edge != GpioPinEdge.FallingEdge)
                return;

            if (!swReadingTimer.IsRunning)
            {
                swReadingTimer.Start();
                timerWatcher = CreateWatcherTimer();
            }

            if (isAddBitMethodLocked == false)
            {
                isAddBitMethodLocked = true;
                
                //left-shift bits
                valueBuffer <<= 1;

                if (pulse == Pulse.Data1)
                    valueBuffer |= 1;

                bitCount++;

                timerWatcher.Change(TIME_IDLEFLAG, TimeSpan.FromMilliseconds(-1));
                isAddBitMethodLocked = false;
            }

            CheckReceivedData();
        }

        void CheckReceivedData()
        {
            timeLastBit = swReadingTimer.ElapsedMilliseconds;

            if (bitCount == 26)
            {
                ulong sansParity = valueBuffer & 0x1FFFFE;
                ulong idFinal = sansParity >> 1;

                RfData = idFinal;

                SiteCode = GetSiteCode(idFinal);
                UserCode = GetUserCode(idFinal);
                Debug.WriteLine($@"ValueBuffer: {valueBuffer}   ID: {idFinal}    SiteCode: {SiteCode}    UserCode: {UserCode}", DEBUG_CAT);

                //timerWatcher.Dispose();
                swReadingTimer.Reset();

                ClearValues(preserveData: true);
            }
        }

        Timer CreateWatcherTimer()
        {
            return new Timer(TimerCheck, null, TimeSpan.Zero, TIME_IDLEFLAG);
        }

        void TimerCheck(object state)
        {
            if (bitCount > 0 && bitCount != 26) // if data has been scanned
            {                
                //timerWatcher.Dispose();
                swReadingTimer.Reset();

                Debug.WriteLine($@"Data incomplete. Current bitCount: {bitCount} Last value: {valueBuffer}", DEBUG_CAT);

                ClearValues();
            }
        }        
                
        ulong GetSiteCode(ulong middleData)
        {
            ulong filtered = middleData & 0xFF0000;
            return filtered >> 16;
        }

        ulong GetUserCode(ulong middleData)
        {
            return middleData & 0x00FFFF;
        }

        #region INPC implementations
        public event PropertyChangedEventHandler RfDataReceived;
        public event PropertyChangedEventHandler IsEnabledChanged;
        
        protected void RaiseRfDataReceived()
        {
            RfDataReceived?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        protected void RaiseIsEnabledChanged()
        {
            IsEnabledChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        #endregion

        void ClearValues(bool preserveData = false)
        {
            bitCount = 0;
            valueBuffer = 0;

            if (!preserveData)
            {
                RfData = 0;
                SiteCode = 0;
                UserCode = 0;
            }
        }

        public void Dispose()
        {
            Debug.WriteLine("Object destroyed via Dispose", DEBUG_CAT);
            
            IsEnabled = false;
            IsEnabledChanged -= OnIsEnabledChanged;

            data0.Dispose();
            data1.Dispose();

            ClearValues();
        }
    }

}
