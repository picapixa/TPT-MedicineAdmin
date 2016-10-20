using System;
using System.ComponentModel;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TPT_MMAS.Iot.Views.Dialogs
{
    public sealed partial class ErrorDialog : INotifyPropertyChanged
    {
        private MediaPlayer errorSoundPlayer;

        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { Set(nameof(ErrorMessage), ref _errorMessage, value); }
        }

        public ErrorDialog()
        {
            InitializeComponent();

            errorSoundPlayer = new MediaPlayer()
            {
                Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/ErrorSound.mp3")),
                AudioCategory = MediaPlayerAudioCategory.Alerts
            };
        }
        private void OnErrorDialogLoaded(object sender, RoutedEventArgs e)
        {
            // Error sound source: http://soundbible.com/1127-Computer-Error.html
            errorSoundPlayer.Play();
        }
        private void OnErrorDialogUnloaded(object sender, RoutedEventArgs e)
        {
            errorSoundPlayer.Dispose();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            sender.Hide();
        }

        /// <summary>
        ///  NOTE: This continues to implement INPC and not the modularized NotifyPropertyChanged
        ///  class on Common because it's part of the base class. We can't have multiple base classes
        ///  on C# :(
        /// </summary>

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

    }
}
