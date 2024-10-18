using Aht10.Domain.Enums;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfAht10Client.Views.Log.Converters
{
    public class LogStatusToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is LogLevel logLevel)
            {
                return (logLevel is LogLevel.Success) ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2f5ebd")) :
                    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#bd3030")); ;
            }
            return new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
