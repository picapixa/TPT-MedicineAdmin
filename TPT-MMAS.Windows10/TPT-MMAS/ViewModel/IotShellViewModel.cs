using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPT_MMAS.Shared.Interface;
using Windows.ApplicationModel.Core;
using Windows.System.Threading;

namespace TPT_MMAS.ViewModel
{
    public class IotShellViewModel : ViewModelBase, INavigable
    {
        private DateTime _currentTime;

        public DateTime CurrentTime
        {
            get { return _currentTime; }
            set { Set(nameof(CurrentTime), ref _currentTime, value); }
        }

        private void LoadClock()
        {
            ThreadPoolTimer clock = null;
            clock = ThreadPoolTimer.CreatePeriodicTimer(_timerTick, TimeSpan.FromMilliseconds(1000));
        }

        private async void _timerTick(ThreadPoolTimer timer)
        {
            var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
            await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                CurrentTime = DateTime.Now;
            });
        }

        public void Activate(object parameter)
        {
            LoadClock();
        }

        public void Deactivate(object parameter)
        {

        }
    }
}
