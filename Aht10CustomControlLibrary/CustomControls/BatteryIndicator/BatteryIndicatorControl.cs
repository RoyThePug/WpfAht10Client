using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Aht10CustomControlLibrary.CustomControls.BatteryIndicator;

public class BatteryIndicatorControl : Control
{
    #region Fields

    private Rectangle? _rectangleValue;
    private double _valueRenderWidth;

    #endregion

    #region Dependency Properties

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        nameof(Value), typeof(double), typeof(BatteryIndicatorControl), new PropertyMetadata(default(double), OnValueChanged));

    public double Value
    {
        get => (double) GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is BatteryIndicatorControl control)
        {
            if (control.IsLoaded)
            {
                control.RenderCapacity();
            }
        }
    }

    public static readonly DependencyProperty NormalCapacityLevelColorProperty = DependencyProperty.Register(
        nameof(NormalCapacityLevelColor), typeof(Brush), typeof(BatteryIndicatorControl), new PropertyMetadata(default(Brush)));

    public Brush NormalCapacityLevelColor
    {
        get => (Brush) GetValue(NormalCapacityLevelColorProperty);
        set => SetValue(NormalCapacityLevelColorProperty, value);
    }

    public static readonly DependencyProperty LowCapacityLevelColorProperty = DependencyProperty.Register(
        nameof(LowCapacityLevelColor), typeof(Brush), typeof(BatteryIndicatorControl), new PropertyMetadata(default(Brush)));

    public Brush LowCapacityLevelColor
    {
        get => (Brush) GetValue(LowCapacityLevelColorProperty);
        set => SetValue(LowCapacityLevelColorProperty, value);
    }

    #endregion

    static BatteryIndicatorControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(BatteryIndicatorControl), new FrameworkPropertyMetadata(typeof(BatteryIndicatorControl)));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _valueRenderWidth = 50;

        _rectangleValue = Template.FindName("PART_RectangleValue", this) as Rectangle;

        RenderCapacity();
    }

    private void RenderCapacity()
    {
        if (_rectangleValue != null)
        {
            var renderWidth = Value / 100 * _valueRenderWidth;

            if (Value <= 20)
            {
                _rectangleValue.Fill = LowCapacityLevelColor;
            }

            else
            {
                _rectangleValue.Fill = NormalCapacityLevelColor;
            }

            var storyboard = new Storyboard();

            var widthAnimation = new DoubleAnimation {From = 0, To = renderWidth, Duration = TimeSpan.FromMilliseconds(500)};

            Storyboard.SetTarget(widthAnimation, _rectangleValue);

            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(WidthProperty));
            storyboard.Children.Add(widthAnimation);

            storyboard.Begin(_rectangleValue);
        }
    }
}