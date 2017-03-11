using System;
using System.Globalization;
using Xamarin.Forms;

namespace DemoAzureOfflineSync.Converters
{
    internal class FontPendingColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool val = (bool)value;

            Color pending = Color.Red;
            Color done = Color.Black;

            return (val) ? pending : done;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}