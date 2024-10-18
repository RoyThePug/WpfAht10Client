
using System.Windows;
using System.Windows.Controls;
using WpfAht10Client.Views.MeasurementData;

namespace WpfAht10Client.Views.Log
{
    public partial class UCLogItem : UserControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
              nameof(Message), typeof(string), typeof(UCLogItem));

        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        #endregion

        public UCLogItem()
        {
            InitializeComponent();
        }
    }
}
