using System;
using Xamarin.Forms;

namespace todo_app_xamarin.screens.todo
{
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool) value ? Color.LawnGreen : Color.LightSkyBlue;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}