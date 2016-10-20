using System;
using TPT_MMAS.Shared.ViewModel;
using Windows.UI.Xaml.Data;

namespace TPT_MMAS.Shared.Converter
{
    /// <summary>
    /// Converts the ListFilter enumeration to a displayable string property.
    /// </summary>
    public class MedsFilterEnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return Enum.GetName(typeof(MedicineListFilter), value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return Enum.Parse(typeof(MedicineListFilter), value as string);
        }
    }
}
