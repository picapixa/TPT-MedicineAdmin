using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;

namespace TPT_MMAS.Iot.Hardware.Foundation
{
    public sealed class Arduino
    {
        //Arduino MEGA
        //private const ushort deviceVid = 2341;


        private const string deviceVid = "067B";

        //ARDUINO MEGA
        //private const ushort devicePid = 0042;

        private const ushort devicePid = 2303;
        private const string deviceGuid = "86e0d1e0-8089-11d0-9ce4-08003e301f73";

        private SerialDevice serialPort = null;
        private DataReader dataReader = null;
        private DataWriter dataWriter = null;

        private CancellationTokenSource readCancellationTokenSource;

        //#region Singleton implementation
        //static readonly Arduino _instance = new Arduino();
        //public static Arduino Instance
        //{
        //    get { return _instance; }
        //}
        //#endregion

        private string _streamContent;

        public string StreamContent
        {
            get { return _streamContent; }
            set
            {
                if (_streamContent == value) return;

                _streamContent = value;
                RaiseContentChanged();
            }
        }

        public Arduino()
        {
            ConnectAsync();
        }



        public async void ConnectAsync()
        {
            string devId = (App.PluggedDevice == Device.Prototype) ?
                            @"\\?\usb#vid_10c4&pid_ea60#0001#{86e0d1e0-8089-11d0-9ce4-08003e301f73}" :
                            @"\\?\USB#VID_2341&PID_0042#75439313537351600220#{86e0d1e0-8089-11d0-9ce4-08003e301f73}";

            // device ID of development board
            //string devId = @"\\?\USB#VID_2341&PID_0042#75439313537351600220#{86e0d1e0-8089-11d0-9ce4-08003e301f73}";

            //// device ID of custom tray controller
            //string devId = @"\\?\usb#vid_10c4&pid_ea60#0001#{86e0d1e0-8089-11d0-9ce4-08003e301f73}";

            // selects the first instance of the Arduino-based tray controller
            serialPort = await SerialDevice.FromIdAsync(devId);

            
            serialPort.WriteTimeout = TimeSpan.FromMilliseconds(1000);
            serialPort.ReadTimeout = TimeSpan.FromMilliseconds(1000);
            serialPort.BaudRate = 9600;
            serialPort.Parity = SerialParity.None;
            serialPort.StopBits = SerialStopBitCount.One;
            serialPort.DataBits = 8;
            serialPort.Handshake = SerialHandshake.None;

            readCancellationTokenSource = new CancellationTokenSource();

            SetStreamAsync();
        }

        public async void SetStreamAsync()
        {
            try
            {
                if (serialPort != null)
                {
                    dataReader = new DataReader(serialPort.InputStream);


                    //keep 'em runnin'
                    while (true)
                    {
                        await ReadFromStreamAsync(readCancellationTokenSource.Token);
                    }
                }
            }
            catch (TaskCanceledException)
            {
                if (serialPort != null)
                    serialPort.Dispose();
                serialPort = null;

                throw new TaskCanceledException();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.DetachStream();
                    dataReader = null;
                }
            }
        }

        private async Task ReadFromStreamAsync(CancellationToken token)
        {
            Task<UInt32> loadAsyncTask;

            uint readBufferLength = 1024;

            token.ThrowIfCancellationRequested();

            dataReader.InputStreamOptions = InputStreamOptions.Partial;

            loadAsyncTask = dataReader.LoadAsync(readBufferLength).AsTask(token);

            UInt32 bytesRead = await loadAsyncTask;
            if (bytesRead > 0)
            {
                StreamContent = dataReader.ReadString(bytesRead);
            }
        }

        private async Task<bool> WriteToStreamAsync(string data)
        {
            Task<UInt32> storeAsyncTask;

            if (data.Length != 0)
            {
                dataWriter.WriteString(data);

                storeAsyncTask = dataWriter.StoreAsync().AsTask();

                UInt32 bytesWritten = await storeAsyncTask;
                return (bytesWritten > 0);
            }
            else
                return false;
        }

        public async Task WriteDataAsync(string data)
        {
            try
            {
                if (serialPort != null)
                {
                    dataWriter = new DataWriter(serialPort.OutputStream);

                    await WriteToStreamAsync(data);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (dataWriter != null)
                {
                    dataWriter.DetachStream();
                    dataWriter = null;
                }
            }
        }


        #region INPC implementations

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangedEventHandler ContentChanged;


        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void RaiseContentChanged()
        {
            ContentChanged?.Invoke(this, null);
        }

        #endregion

        ~Arduino()
        {
            if ((readCancellationTokenSource != null) && (readCancellationTokenSource.IsCancellationRequested))
                readCancellationTokenSource.Cancel();

            if (serialPort != null)
                serialPort.Dispose();
            serialPort = null;
        }


    }
}
