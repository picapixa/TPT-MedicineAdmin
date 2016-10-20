using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace TPT_MMAS.Iot.Hardware
{
    public class RfidReader
    {

        private int _d0;
        private int _d1;
        private int bitCount = 0;
        private long cardTemp = 0;
        private long timeLastBit = 0;


        private long _rfData = 0;

        public long RfData
        {
            get { return _rfData; }
            private set { Set(nameof(RfData), ref _rfData, value, RaiseRfDataChanged); }

        }



        private GpioController Gpio { get; set; }
        private Stopwatch Stopwatch { get; set; }

        private CancellationTokenSource cancellationTokenSource;

        public RfidReader(int d0, int d1)
        {
            _d0 = d0;
            _d1 = d1;

            //passed values are likely d0 = gpio23 and d1 = gpio24
            Gpio = GpioController.GetDefault();
            if (Gpio == null)
                return;

            //set pins d0 and d1 as input
            GpioPin data0 = Gpio.OpenPin(_d0, GpioSharingMode.SharedReadOnly);
            //data0.SetDriveMode(GpioPinDriveMode.Input);
            data0.ValueChanged += Data0_ValueChanged;

            GpioPin data1 = Gpio.OpenPin(_d1, GpioSharingMode.SharedReadOnly);
            //data1.SetDriveMode(GpioPinDriveMode.Input);
            data1.ValueChanged += Data1_ValueChanged;

            cancellationTokenSource = new CancellationTokenSource();

            Stopwatch = Stopwatch.StartNew();
            
        }

        public bool IsRfDataAvailable()
        {
            bool isConverted = IsConvertedToWiegand();
            return isConverted;
        }

        private bool IsConvertedToWiegand()
        {
            long sysTick = Stopwatch.ElapsedMilliseconds;

            if ((sysTick - timeLastBit) > 25)
            {
                if (bitCount == 26)
                {
                    cardTemp >>= 1;

                    RfData = GetCardId(cardTemp);
                    bitCount = 0;
                    cardTemp = 0;
                    return true;
                }
                else //just probably noise
                {
                    timeLastBit = sysTick;
                    bitCount = 0;
                    cardTemp = 0;
                    return false;
                }
            }
            else
                return false;
        }

        private long GetCardId(long codeLow)
        {
            long cardId = (codeLow & 0x1FFFFFE) >> 1;
            return cardId;
        }
        

        public async Task<string> GetRfidDataAsync()
        {
            string c = "";
            c = await Task.Run(() =>
            {
                string code = null;
                bool isDone = false;

                while (!isDone)
                {
                    if (IsRfDataAvailable())
                    {
                        code = RfData.ToString();
                        isDone = true;
                    }
                }

                return code;

            });
            return c;
        }

        private void Data0_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            if (args.Edge != GpioPinEdge.FallingEdge)
                return;

            bitCount++;
            cardTemp <<= 1;

            //note the time
            timeLastBit = Stopwatch.ElapsedMilliseconds;
        }

        private void Data1_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            if (args.Edge != GpioPinEdge.FallingEdge)
                return;

            bitCount++;

            cardTemp |= 1;
            cardTemp <<= 1;

            //note the time
            timeLastBit = Stopwatch.ElapsedMilliseconds;
        }


        #region INPC implementations
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangedEventHandler RfDataChanged;
        
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void RaiseRfDataChanged()
        {
            RfDataChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        private bool Set<T>(string propertyName, ref T storage, T value, Action handler = null)
        {
            if (Equals(storage, value))
                return false;

            storage = value;
            if (handler != null)
                handler.Invoke();
            else
                RaisePropertyChanged(propertyName);

            return true;            
        }

        #endregion


        ~RfidReader()
        {
            if ((cancellationTokenSource != null) && cancellationTokenSource.IsCancellationRequested)
                cancellationTokenSource.Cancel();

            using (GpioPin data0 = Gpio.OpenPin(_d0))
                data0.ValueChanged -= Data0_ValueChanged;
            using (GpioPin data1 = Gpio.OpenPin(_d1))
                data1.ValueChanged -= Data1_ValueChanged;

            if (Stopwatch.IsRunning)
                Stopwatch.Stop();

        }

    }
}
