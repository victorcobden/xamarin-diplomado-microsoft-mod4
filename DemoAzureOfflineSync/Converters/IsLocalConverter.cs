using System;
using System.Globalization;
using Xamarin.Forms;

namespace DemoAzureOfflineSync.Converters
{
    public class IsLocalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool val = (bool)value;

            ImageSource synced = Device.OnPlatform(
            iOS: ImageSource.FromFile("synced.png"),
            Android: ImageSource.FromFile("synced.png"),
            WinPhone: ImageSource.FromFile("synced.png"));

            ImageSource notsynced = Device.OnPlatform(
            iOS: ImageSource.FromFile("notsynced.png"),
            Android: ImageSource.FromFile("notsynced.png"),
            WinPhone: ImageSource.FromFile("notsynced.png"));

            return (val) ? notsynced : synced;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}