using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfAht10Client.Views.MeasurementData
{
    /// <summary>
    /// Interaction logic for UCMeasurementData.xaml
    /// </summary>
    public partial class UCMeasurementData : UserControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            nameof(ItemsSource), typeof(IEnumerable), typeof(UCMeasurementData), new PropertyMetadata(default(IEnumerable)));

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
     nameof(SelectedItem), typeof(object), typeof(UCMeasurementData), new PropertyMetadata(default(object)));

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        #endregion

        #region Routed Events

        public static readonly RoutedEvent SelectedChangedEvent =
            EventManager.RegisterRoutedEvent(nameof(SelectedChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UCMeasurementData));

        public event RoutedEventHandler SelectedChanged
        {
            add => AddHandler(SelectedChangedEvent, value);
            remove => RemoveHandler(SelectedChangedEvent, value);
        }

        #endregion

        public UCMeasurementData()
        {
            InitializeComponent();

            dataGrid.SelectionChanged += DataGrid_SelectionChanged;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(SelectedChangedEvent));
        }
    }
}
