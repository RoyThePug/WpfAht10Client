using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace WpfAht10Client.Views.MeasurementSelection
{
    public partial class UCMeasurementSelection
    {
        #region Dependency Properties

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            nameof(ItemsSource), typeof(IEnumerable), typeof(UCMeasurementSelection), new PropertyMetadata(default(IEnumerable)));

        public IEnumerable ItemsSource
        {
            get => (IEnumerable) GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            nameof(SelectedItem), typeof(object), typeof(UCMeasurementSelection), new PropertyMetadata(default(object)));

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(
            nameof(ItemTemplate), typeof(DataTemplate), typeof(UCMeasurementSelection), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate) GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        #endregion

        #region Routed Events

        public static readonly RoutedEvent SelectedChangedEvent =
            EventManager.RegisterRoutedEvent(nameof(SelectedChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UCMeasurementSelection));

        public event RoutedEventHandler SelectedChanged
        {
            add => AddHandler(SelectedChangedEvent, value);
            remove => RemoveHandler(SelectedChangedEvent, value);
        }

        #endregion

        public UCMeasurementSelection()
        {
            InitializeComponent();

            SelectionDateCmbx.SelectionChanged += SelectionDateCmbxOnSelectionChanged;
        }

        private void SelectionDateCmbxOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(SelectedChangedEvent));
        }
    }
}