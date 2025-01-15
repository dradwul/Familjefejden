using System;
using Windows.UI.Xaml.Data;

namespace Familjefejden.Klasser
{
    internal class TidsKonvertering : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is TimeSpan timeSpan)
            {
                return timeSpan.ToString(@"hh\:mm");
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string timeString && TimeSpan.TryParseExact(timeString, @"hh\:mm", null, out TimeSpan timeSpan))
            {
                return timeSpan;
            }
            return value;
        }
    }
}