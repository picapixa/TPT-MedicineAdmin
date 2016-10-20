using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace TPT_MMAS.Shared.Converter
{
    /// <summary>
    /// Returns Visibility.Visible if the binded input matches what's in the converter parameter; Collapsed otherwise.
    /// </summary>
    public class IntegerToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var param = int.Parse(parameter as string);
            
            return ((int)value == param) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
