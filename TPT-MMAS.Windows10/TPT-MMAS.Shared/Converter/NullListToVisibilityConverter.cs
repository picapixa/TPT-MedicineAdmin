using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace TPT_MMAS.Shared.Converter
{
    /// <summary>
    /// Checks the IEnumerable<> data type if it's null or empty, and returns Visibility.Collapsed if true, Visible otherwise.
    /// 
    /// An option to return an inverse value is provided if the IEnumerable<> data type is null or empty by specifying "reverse" as the parameter.
    /// </summary>
    public class NullListToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool param = (parameter as string == "reverse");
            bool isEmpty = (value != null && !((value as IEnumerable<object>).Any()));

            if (param)
                return (value == null  || isEmpty) ? Visibility.Visible : Visibility.Collapsed;
            else
                return (value == null || isEmpty) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
