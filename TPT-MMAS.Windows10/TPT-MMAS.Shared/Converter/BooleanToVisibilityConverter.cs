using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace TPT_MMAS.Shared.Converter
{
    /// <summary>
    /// Converts a boolean value to a visibility enum value: true returns a Visible property, false otherwise.
    /// 
    /// Adding "false" into the converter parameter reverses the result.
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool v = (bool)value;
            bool param = ((parameter as string) != "false");

            if (param == false)
                return (v == true) ? Visibility.Collapsed : Visibility.Visible;
            else
                return (v == true) ? Visibility.Visible : Visibility.Collapsed;

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
