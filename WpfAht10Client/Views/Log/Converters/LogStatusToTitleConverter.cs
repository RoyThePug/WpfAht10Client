
using Aht10.Domain.Enums;
using System.Globalization;
using System.Windows.Data;

namespace WpfAht10Client.Views.Log.Converters
{
    public class LogStatusToTitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is LogLevel logLevel) 
            {
                return (logLevel is LogLevel.Success) ? "Success" : "Error";
            }

            return "Undefined";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
