using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TPT_MMAS.Shared.Control.UserControls
{
    /// <summary>
    ///  NOTE: This continues to implement INPC and not the modularized NotifyPropertyChanged
    ///  class on Common because it's part of the base class. We can't have multiple base classes
    ///  on C# :(
    /// </summary>
    public sealed partial class TrayView : UserControl, INotifyPropertyChanged
    {
        private Color highlightedColor = Color.FromArgb(255,168,0,0);
        private Color presentColor = Color.FromArgb(255, 33, 66, 0);

        private int _selectedContainer;
        public int SelectedContainer
        {
            get { return _selectedContainer; }
            set { Set(nameof(SelectedContainer), ref _selectedContainer, value); }
        }

        private Color _selectedBackgroundColor;
        public Color SelectedBackgroundColor
        {
            get { return _selectedBackgroundColor; }
            set { Set(nameof(SelectedBackgroundColor), ref _selectedBackgroundColor, value); }
        }

        private bool _containerHasItem = false;

        public bool ContainerHasItem
        {
            get { return _containerHasItem; }
            set { Set(nameof(ContainerHasItem), ref _containerHasItem, value); }
        }
        
        public TrayView()
        {
            InitializeComponent();
            SelectedBackgroundColor = highlightedColor;

            PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ContainerHasItem):
                    UpdateContainerVisuals();
                    break;
                default:
                    break;
            }
        }

        private void UpdateContainerVisuals()
        {
            //Grid rootContainerGrid = RootGrid.FindName("grid" + SelectedContainer) as Grid;
            SelectedBackgroundColor = (ContainerHasItem) ? presentColor : highlightedColor;
        }

        #region INPC implementation
        public event PropertyChangedEventHandler PropertyChanged;

        private bool Set<T>(string propertyName, ref T storage, T value)
        {
            if (Equals(storage, value))
                return false;

            storage = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
