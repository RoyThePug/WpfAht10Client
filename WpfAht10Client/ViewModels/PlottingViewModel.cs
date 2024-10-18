using System.Collections.ObjectModel;
using System.Text.Json;
using Aht10.Domain.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WpfAht10Client.Models;
using WpfAht10Client.Services.Measurement;

namespace WpfAht10Client.ViewModels;

public partial class PlottingViewModel : ObservableObject
{
    #region Fields

    private readonly IMeasurementService _measurementService;
    private readonly IPlottingDataService _plottingDataService;

    #endregion

    #region Properties

    [ObservableProperty] private double minTemperature;
    [ObservableProperty] private double maxTemperature;
    [ObservableProperty] private DateTime minMeasurementDate;
    [ObservableProperty] private DateTime maxMeasurementDate;
    [ObservableProperty] private IEnumerable<double> temperatureDataSource;
    [ObservableProperty] private DateTime[] datetimeDataSource;
    [ObservableProperty] public MeasurementModel selectedMeasurement;

    public ObservableCollection<PlotMeteorologicalModel> MeteorologicalSource { get; }

    #endregion

    public PlottingViewModel(IMeasurementService measurementService, IPlottingDataService plottingDataService)
    {
        _measurementService = measurementService ?? throw new ArgumentNullException(nameof(measurementService));

        _plottingDataService = plottingDataService ?? throw new ArgumentNullException(nameof(plottingDataService));

        MeteorologicalSource = new ObservableCollection<PlotMeteorologicalModel>();
    }

    #region Commands

    [RelayCommand]
    public async Task ChangeSelectedMeasurementDateAsync(object param)
    {
        try
        {
            if (param is DateTime date)
            {
                SelectedMeasurement = await _measurementService.GetMeasurementByDataAsync(date);

                if (SelectedMeasurement != null)
                {
                    var metrologicalSource = (await _measurementService.GetMeteorologicalDataByDateAsync(SelectedMeasurement.Id)).ToList();

                    MinTemperature = metrologicalSource.Min(x => x.Temperature);

                    MaxTemperature = metrologicalSource.Max(x => x.Temperature);

                    MinMeasurementDate = metrologicalSource.Min(x => x.MeteorologicalTime);

                    MaxMeasurementDate = metrologicalSource.Max(x => x.MeteorologicalTime);

                    TemperatureDataSource = metrologicalSource.Select(x => (double)x.Temperature).ToArray();

                    DatetimeDataSource = metrologicalSource.Select(x => x.MeteorologicalTime).ToArray();
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }


    [RelayCommand]
    public async Task UpdateMeasurementImageAsync(object param)
    {
        try
        {
            if (param is byte[] source && SelectedMeasurement != null)
            {
                var json = JsonSerializer.Serialize(new
                {
                    MeasurementId = SelectedMeasurement.Id,
                    ImageData = source
                });

                await _plottingDataService.SaveMeasurementImageBytes(json);
            }
        }
        catch (Exception)
        {

        }
    }

    #endregion
}