using System;
using Windows.UI.Xaml.Data;

namespace Familjefejden.Klasser
{
    internal class DatumKonvertering : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime dateTime)
            {
                return dateTime.ToString("yyyy-MM-dd");
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string dateString && DateTime.TryParse(dateString, out DateTime dateTime))
            {
                return dateTime;
            }
            return value;
        }
    }
}