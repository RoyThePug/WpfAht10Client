using System.Collections;
using System.Windows;

namespace WpfAht10Client.Views.MetrologicalData;

public partial class UcMetrologicalData
{
    #region Dependency Properties

    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
        nameof(ItemsSource), typeof(IEnumerable), typeof(UcMetrologicalData), new PropertyMetadata(default(IEnumerable)));

    public IEnumerable ItemsSource
    {
        get => (IEnumerable) GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(
        nameof(ItemTemplate), typeof(DataTemplate), typeof(UcMetrologicalData), new PropertyMetadata(default(DataTemplate)));

    public DataTemplate ItemTemplate
    {
        get => (DataTemplate) GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }

    #endregion

    public UcMetrologicalData()
    {
        InitializeComponent();
    }
}