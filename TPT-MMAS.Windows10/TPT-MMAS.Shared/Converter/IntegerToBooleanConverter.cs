using System;
using Windows.UI.Xaml.Data;

namespace TPT_MMAS.Shared.Converter
{
    /// <summary>
    /// Returns true if the binded input matches what's in the converter parameter; false otherwise.
    /// </summary>
    public class IntegerToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int i;
            int.TryParse(value.ToString(), out i);

            int param;
            int.TryParse(parameter.ToString(), out param);

            return (i == param);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
