using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects;
using Microsoft.Xaml.Behaviors;

namespace WpfAht10Client.Views.Behaviors;

public class ChangeBlurEffectBehavior : Behavior<Grid>
{
    #region Dependency Property

    public static readonly DependencyProperty EnableEffectProperty =
        DependencyProperty.Register(nameof(EnableEffect), typeof(bool), typeof(ChangeBlurEffectBehavior), new PropertyMetadata(default(bool), OnEnableEffectChanged));

    public bool EnableEffect
    {
        get => (bool)GetValue(EnableEffectProperty);
        set => SetValue(EnableEffectProperty, value);
    }

    private static void OnEnableEffectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ChangeBlurEffectBehavior behavior)
        {
            behavior.ChangeBlurEffect((bool)e.NewValue);
        }
    }

    public static readonly DependencyProperty RadiusProperty =
        DependencyProperty.Register(nameof(Radius), typeof(double), typeof(ChangeBlurEffectBehavior), new PropertyMetadata(1.0));

    public double Radius
    {
        get => (double)GetValue(RadiusProperty);
        set => SetValue(RadiusProperty, value);
    }

    public static readonly DependencyProperty KernelTypeProperty =
        DependencyProperty.Register(nameof(KernelType), typeof(KernelType), typeof(ChangeBlurEffectBehavior), new PropertyMetadata(KernelType.Box));

    public KernelType KernelType
    {
        get => (KernelType)GetValue(KernelTypeProperty);
        set => SetValue(KernelTypeProperty, value);
    }

    #endregion

    protected override void OnAttached() => ChangeBlurEffect(EnableEffect);

    private void ChangeBlurEffect(bool enableEffect)
    {
        AssociatedObject.Effect = enableEffect
            ? new BlurEffect
            {
                KernelType = KernelType,
                Radius = Radius
            }
            : null;
    }
}