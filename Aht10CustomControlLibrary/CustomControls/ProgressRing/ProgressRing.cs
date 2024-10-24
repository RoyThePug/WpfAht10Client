using System.Windows;
using System.Windows.Controls;

namespace Aht10CustomControlLibrary.CustomControls.ProgressRing
{
    public class ProgressRing : Control
    {
        #region Dependency Properties

        public static readonly DependencyProperty BindableWidthProperty =
        DependencyProperty.Register(nameof(BindableWidth), typeof(double), typeof(ProgressRing));

        public double BindableWidth
        {
            get { return (double)GetValue(BindableWidthProperty); }
            set { SetValue(BindableWidthProperty, value); }
        }

        public static readonly DependencyProperty IsActiveProperty =
         DependencyProperty.Register(nameof(IsActive), typeof(bool), typeof(ProgressRing));

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public static readonly DependencyProperty IsLargeProperty =
        DependencyProperty.Register(nameof(IsLarge), typeof(bool), typeof(ProgressRing));

        public bool IsLarge
        {
            get { return (bool)GetValue(IsLargeProperty); }
            set { SetValue(IsLargeProperty, value); }
        }

        public static readonly DependencyProperty EllipseDiameterScaleProperty =
            DependencyProperty.Register(nameof(EllipseDiameterScale), typeof(double), typeof(ProgressRing), new PropertyMetadata(1D));

        public double EllipseDiameterScale
        {
            get => (double)this.GetValue(EllipseDiameterScaleProperty);
            set => this.SetValue(EllipseDiameterScaleProperty, value);
        }

        #endregion

        static ProgressRing()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressRing), new FrameworkPropertyMetadata(typeof(ProgressRing)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }
}
