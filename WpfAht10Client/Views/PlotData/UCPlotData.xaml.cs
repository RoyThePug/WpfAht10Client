using System.Collections;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Extensions.DependencyInjection;
using ScottPlot;
using WpfAht10Client.ViewModels;
using Color = ScottPlot.Color;

namespace WpfAht10Client.Views.PlotData;

public partial class UCPlotData
{
    #region Fileds

    private bool _calculatedX;
    private bool _calculatedY;

    #endregion

    #region Dependency Properties

    public static readonly DependencyProperty XdataSourceProperty = DependencyProperty.Register(
        nameof(XdataSource), typeof(IEnumerable), typeof(UCPlotData), new PropertyMetadata(default(IEnumerable), XdataSourceChanged));

    public IEnumerable XdataSource
    {
        get => (IEnumerable)GetValue(XdataSourceProperty);
        set => SetValue(XdataSourceProperty, value);
    }

    private static void XdataSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is UCPlotData control)
        {
            if (control.IsLoaded)
            {
                control._calculatedX = true;

                if (control._calculatedY)
                {
                    control.CalculateData();
                }
            }
        }
    }

    public static readonly DependencyProperty YdataSourceProperty = DependencyProperty.Register(
        nameof(YdataSource), typeof(IEnumerable), typeof(UCPlotData), new PropertyMetadata(default(IEnumerable), YdataSourceChanged));

    public IEnumerable YdataSource
    {
        get => (IEnumerable)GetValue(YdataSourceProperty);
        set => SetValue(YdataSourceProperty, value);
    }

    private static void YdataSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is UCPlotData control)
        {
            if (control.IsLoaded)
            {
                control._calculatedY = true;

                if (control._calculatedX)
                {
                    control.CalculateData();
                }
            }
        }
    }

    public static readonly DependencyProperty SelectedMeasurementProperty = DependencyProperty.Register(
        nameof(SelectedMeasurement), typeof(DateTime), typeof(UCPlotData), new PropertyMetadata(default(DateTime), OnSelectedMeasurementChanged));

    public DateTime SelectedMeasurement
    {
        get => (DateTime)GetValue(SelectedMeasurementProperty);
        set => SetValue(SelectedMeasurementProperty, value);
    }

    private static void OnSelectedMeasurementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is UCPlotData control)
        {
            if (control.IsLoaded)
            {
                if (e.NewValue is DateTime date)
                {
                    control.RaiseEvent(new RoutedPropertyChangedEventArgs<object>(e.OldValue, date, OnMeasurementDateChangedEvent));
                }
            }
        }
    }

    public static readonly DependencyProperty SendImageDbCommandProperty = DependencyProperty.Register(
        nameof(SendImageDbCommand), typeof(ICommand), typeof(UCPlotData));

    public ICommand SendImageDbCommand
    {
        get => (ICommand)GetValue(SendImageDbCommandProperty);
        set => SetValue(SendImageDbCommandProperty, value);
    }

    #endregion

    #region Routed Events

    public static readonly RoutedEvent OnMeasurementDateChangedEvent =
        EventManager.RegisterRoutedEvent(nameof(OnMeasurementDateChanged), RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(UCPlotData));

    public event RoutedPropertyChangedEventHandler<object> OnMeasurementDateChanged
    {
        add => AddHandler(OnMeasurementDateChangedEvent, value);
        remove => RemoveHandler(OnMeasurementDateChangedEvent, value);
    }

    public static readonly RoutedEvent OnMeasurementImageUpdatedEvent =
        EventManager.RegisterRoutedEvent(nameof(OnMeasurementImageUpdated), RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<byte[]>), typeof(UCPlotData));

    public event RoutedPropertyChangedEventHandler<byte[]> OnMeasurementImageUpdated
    {
        add => AddHandler(OnMeasurementImageUpdatedEvent, value);
        remove => RemoveHandler(OnMeasurementImageUpdatedEvent, value);
    }

    #endregion

    public UCPlotData()
    {
        InitializeComponent();

        if (App.ServiceProvider != null)
        {
            DataContext = App.ServiceProvider.GetService<PlottingViewModel>();

            if (Application.Current.Resources["ItemMainColor"] is System.Windows.Media.Color color)
            {
                Plot.Plot.FigureBackground = new() { Color = Color.FromColor(System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B)) };
            }

            // plot.Plot.DataBackground = new() { Color = Colors.LightSkyBlue };
            Plot.Plot.Axes.DateTimeTicksBottom();
            Plot.Plot.Axes.Color(Color.FromHex("#ffffff"));
            
            Plot.Plot.RenderManager.RenderStarting += (s, e) =>
            {
                var ticks = Plot.Plot.Axes.Bottom.TickGenerator.Ticks;
                
                for (int i = 0; i < ticks.Length; i++)
                {
                    DateTime dt = DateTime.FromOADate(ticks[i].Position);
                    var label = $"{dt:HH:mm}";
                    ticks[i] = new Tick(ticks[i].Position, label);
                }
            };
        }
    }

    #region Methods

    void CalculateData()
    {
        var xsource = XdataSource as DateTime[];
        var ysource = YdataSource as double[];

        if (xsource != null && ysource != null)
        {
            Plot.Plot.Clear();

            var sp1 = Plot.Plot.Add.Scatter(xsource, ysource);

            if (Application.Current.Resources["ControlSubColor"] is System.Windows.Media.Color color)
            {
                sp1.Color = Color.FromColor(System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B));
            }

            sp1.LineWidth = 2;
            sp1.MarkerSize = 6;
            Plot.Refresh();
            Plot.Plot.Axes.AutoScaleX();
            Plot.Plot.Axes.AutoScaleY();

            _calculatedX = false;
            _calculatedY = false;
        }
    }

    public BitmapImage ConvertToBitmap(byte[] imageData)
    {
        var bmp = new BitmapImage();

        using (MemoryStream strm = new MemoryStream())
        using (MemoryStream ms = new MemoryStream())
        {
            strm.Write(imageData, 0, imageData.Length);
            strm.Position = 0;

            System.Drawing.Image img = System.Drawing.Image.FromStream(strm);

            img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);

            bmp.BeginInit();
            bmp.CacheOption = BitmapCacheOption.OnLoad;
            bmp.StreamSource = ms;
            bmp.EndInit();
            bmp.Freeze();

            return bmp;
        }
    }

    #endregion

    #region Event Handlers

    private void btnSaveDbImg_Click(object sender, RoutedEventArgs e)
    {
        var actualWidth = Convert.ToInt32(Plot.ActualWidth);
        var actualHeight = Convert.ToInt32(Plot.ActualHeight);

        if (actualWidth > 0 && actualHeight > 0)
        {
            var imageBytes = Plot.Plot.GetImageBytes(600, 400);

            RaiseEvent(new RoutedPropertyChangedEventArgs<byte[]>(null, imageBytes, OnMeasurementImageUpdatedEvent));
        }
    }

    #endregion
}