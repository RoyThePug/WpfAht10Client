
using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using WpfAht10Client.ViewModels;

namespace WpfAht10Client.Views.Log
{
    /// <summary>
    /// Interaction logic for UCLog.xaml
    /// </summary>
    public partial class UCLog : UserControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            nameof(ItemsSource), typeof(IEnumerable), typeof(UCLog), new PropertyMetadata(default(IEnumerable)));

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(
          nameof(ItemTemplate), typeof(DataTemplate), typeof(UCLog));

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        #endregion

        public UCLog()
        {
            InitializeComponent();

            if (App.ServiceProvider != null)
            {
                DataContext = App.ServiceProvider.GetService<LoginViewModel>();
            }
        }
    }
}
