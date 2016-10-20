using Humanizer;
using System;
using Windows.UI.Xaml.Data;

namespace TPT_MMAS.Shared.Converter.Humanizr
{
    /// <summary>
    /// Pluralizes the text in the converter parameter and adds it with the value depending on the binded value.
    /// 
    /// This converter utilizes the Humanizer library, which is under the MIT License.
    /// Code: https://github.com/Humanizr/Humanizer
    /// License: https://github.com/Humanizr/Humanizer/blob/master/LICENSE
    /// </summary>

    public class QuantityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var item = parameter as string;
            return item.ToQuantity((int)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
