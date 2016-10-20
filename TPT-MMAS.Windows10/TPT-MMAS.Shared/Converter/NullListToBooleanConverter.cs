using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace TPT_MMAS.Shared.Converter
{
    /// <summary>
    /// Checks the IEnumerable<> data type if it's null or empty.
    /// 
    /// An option to return false is provided if the IEnumerable<> data type is null or empty by specifying "reverse" as the parameter.
    /// </summary>
    public class NullListToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool isReverse = ((parameter != null) && parameter.ToString().ToLower() == "reverse");
            bool isEmpty = (value != null && !((value as IEnumerable<object>).Any()));
                        
            if (isReverse)
                return !(value == null || isEmpty);
            else
                return (value == null || isEmpty);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
