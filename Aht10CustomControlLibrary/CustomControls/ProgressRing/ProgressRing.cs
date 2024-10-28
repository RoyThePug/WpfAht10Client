using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace Aht10CustomControlLibrary.CustomControls.ProgressRing
{
    [TemplateVisualState(Name = "Large",GroupName = "SizeStates")]
    [TemplateVisualState(Name = "Small",GroupName = "SizeStates")]
    [TemplateVisualState(Name = "Inactive",GroupName = "SizeStates")]
    [TemplateVisualState(Name = "Active",GroupName = "SizeStates")]
    public class ProgressRing : Control
    {
        #region Dependency Properties

        private static readonly DependencyPropertyKey BindableWidthPropertyKey = DependencyProperty.RegisterReadOnly(nameof(BindableWidth),
            typeof(double), typeof(ProgressRing), new PropertyMetadata(default(double), OnBindableWidthPropertyChanged));

        /// <summary>Identifies the <see cref="BindableWidth"/> dependency property.</summary>
        public static readonly DependencyProperty BindableWidthProperty = BindableWidthPropertyKey.DependencyProperty;

        public double BindableWidth
        {
            get => (double)GetValue(BindableWidthProperty);
            protected set => SetValue(BindableWidthPropertyKey, value);
        }

        private static void OnBindableWidthPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (!(dependencyObject is ProgressRing ring))
            {
                return;
            }

            var action = new Action(
                () =>
                {
                    ring.SetEllipseDiameter((double)dependencyPropertyChangedEventArgs.NewValue);
                    ring.SetEllipseOffset((double)dependencyPropertyChangedEventArgs.NewValue);
                    ring.SetMaxSideLength((double)dependencyPropertyChangedEventArgs.NewValue);
                });

            if (ring._deferredActions != null)
            {
                ring._deferredActions.Add(action);
            }
            else
            {
                action();
            }
        }

        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register(nameof(IsActive), typeof(bool), typeof(ProgressRing), new PropertyMetadata(BooleanBoxes.TrueBox, OnIsActivePropertyChanged));

        public bool IsActive
        {
            get => (bool)GetValue(IsActiveProperty);
            set => this.SetValue(IsActiveProperty, BooleanBoxes.Box(value));
        }

        private static void OnIsActivePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var ring = dependencyObject as ProgressRing;

            ring?.UpdateActiveState();
        }

        public static readonly DependencyProperty IsLargeProperty = DependencyProperty.Register(nameof(IsLarge),
            typeof(bool), typeof(ProgressRing), new PropertyMetadata(BooleanBoxes.TrueBox, OnIsLargePropertyChanged));

        public bool IsLarge
        {
            get => (bool)GetValue(IsLargeProperty);
            set => SetValue(IsLargeProperty, BooleanBoxes.Box(value));
        }

        private static void OnIsLargePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var ring = dependencyObject as ProgressRing;

            ring?.UpdateLargeState();
        }

        public static readonly DependencyProperty EllipseDiameterScaleProperty = DependencyProperty.Register(nameof(EllipseDiameterScale),
            typeof(double), typeof(ProgressRing), new PropertyMetadata(1D));

        public double EllipseDiameterScale
        {
            get => (double)GetValue(EllipseDiameterScaleProperty);
            set => SetValue(EllipseDiameterScaleProperty, value);
        }

        #endregion

        #region Readonly Dependency Properties

        public static readonly DependencyPropertyKey EllipseDiameterPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(EllipseDiameter), typeof(double), typeof(ProgressRing), new PropertyMetadata(default(double)));


        public static readonly DependencyProperty EllipseDiameterProperty = EllipseDiameterPropertyKey.DependencyProperty;

        public double EllipseDiameter
        {
            get => (double)GetValue(EllipseDiameterProperty);
            set => SetValue(EllipseDiameterProperty, value);
        }

        public static readonly DependencyPropertyKey EllipseOffsetPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(EllipseOffset), typeof(Thickness), typeof(ProgressRing), new PropertyMetadata());

        public static readonly DependencyProperty EllipseOffsetProperty = EllipseDiameterPropertyKey.DependencyProperty;

        public Thickness EllipseOffset
        {
            get => (Thickness)GetValue(EllipseOffsetProperty);
            protected set => SetValue(EllipseOffsetProperty, value);
        }

        public static readonly DependencyPropertyKey MaxSideLengthPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MaxSideLength), typeof(double), typeof(ProgressRing), new PropertyMetadata());

        public static readonly DependencyProperty MaxSideLengthProperty = MaxSideLengthPropertyKey.DependencyProperty;

        public double MaxSideLength
        {
            get => (double)GetValue(MaxSideLengthProperty);
            protected set => SetValue(MaxSideLengthProperty, value);
        }

        #endregion

        private List<Action>? _deferredActions = new();

        static ProgressRing()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressRing), new FrameworkPropertyMetadata(typeof(ProgressRing)));
            VisibilityProperty.OverrideMetadata(
                typeof(ProgressRing),
                new FrameworkPropertyMetadata(
                    (ringObject, e) =>
                    {
                        if (e.NewValue != e.OldValue)
                        {
                            var ring = ringObject as ProgressRing;

                            ring?.SetCurrentValue(IsActiveProperty, BooleanBoxes.Box((Visibility)e.NewValue == Visibility.Visible));
                        }
                    }));
        }

        public ProgressRing()
        {
            SizeChanged += OnSizeChanged;
        }

        #region Methods

        public override void OnApplyTemplate()
        {
            // make sure the states get updated
            UpdateLargeState();
            UpdateActiveState();

            base.OnApplyTemplate();

            if (_deferredActions != null)
            {
                foreach (var action in _deferredActions)
                {
                    action();
                }
            }

            _deferredActions = null;
        }

        private void UpdateActiveState()
        {
            Action action;

            if (IsActive)
            {
                action = () => VisualStateManager.GoToState(this, "Active", true);
            }
            else
            {
                action = () => VisualStateManager.GoToState(this, "Inactive", true);
            }

            if (_deferredActions != null)
            {
                _deferredActions.Add(action);
            }
            else
            {
                action();
            }
        }

        private void UpdateLargeState()
        {
            Action action;

            if (IsLarge)
            {
                action = () => VisualStateManager.GoToState(this, "Large", true);
            }
            else
            {
                action = () => VisualStateManager.GoToState(this, "Small", true);
            }

            if (_deferredActions != null)
            {
                _deferredActions.Add(action);
            }
            else
            {
                action();
            }
        }

        private void SetMaxSideLength(double width)
        {
            SetValue(MaxSideLengthPropertyKey, width <= 20d ? 20d : width);
        }

        private void SetEllipseDiameter(double width)
        {
            SetValue(EllipseDiameterPropertyKey, (width / 8) * EllipseDiameterScale);
        }

        private void SetEllipseOffset(double width)
        {
            SetValue(EllipseOffsetPropertyKey, new Thickness(0, width / 2, 0, 0));
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new ProgressRingAutomationPeer(this);
        }

        #endregion

        #region Event Handlers

        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            SetValue(BindableWidthPropertyKey, ActualWidth);
        }

        #endregion
    }
}