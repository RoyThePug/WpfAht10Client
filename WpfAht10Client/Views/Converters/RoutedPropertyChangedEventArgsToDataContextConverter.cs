using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace WpfAht10Client.Views.Converters
{
    public class RoutedPropertyChangedEventArgsToDataContextConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is RoutedPropertyChangedEventArgs<object> data)
            {
                return data.NewValue;
            }

            if (value is RoutedPropertyChangedEventArgs<byte[]> source)
            {
                return source.NewValue;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
