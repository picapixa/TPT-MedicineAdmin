using System;
using System.ComponentModel;

namespace TPT_MMAS.Shared.Common.TPT
{
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool Set<T>(string propertyName, ref T storage, T value, Action raiseChanged = null, bool checkForEquality = true)
        {
            if (checkForEquality && Equals(storage, value))
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

    }
}
