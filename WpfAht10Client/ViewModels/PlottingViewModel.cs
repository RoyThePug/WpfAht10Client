using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Aht10.Domain.CustomEventArgs;
using Aht10.Domain.Models;
using Aht10.Domain.Services;
using Aht10.Domain.Services.Measurement;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace WpfAht10Client.ViewModels
{
    public partial class PlottingViewModel : ObservableObject
    {
        #region Fields

        private readonly IMeasurementService _measurementService;
        private readonly IPlottingDataService _plottingDataService;

        #endregion

        #region Properties

        [ObservableProperty] public bool isBusy;
        [ObservableProperty] private double _minTemperature;
        [ObservableProperty] private double _maxTemperature;
        [ObservableProperty] private DateTime _minMeasurementDate;
        [ObservableProperty] private DateTime _maxMeasurementDate;
        [ObservableProperty] private IEnumerable<double> _temperatureDataSource;
        [ObservableProperty] private DateTime[] _datetimeDataSource;
        [ObservableProperty] public MeasurementModel selectedMeasurement;

        public ObservableCollection<PlotMeteorologicalModel> MeteorologicalSource { get; }

        #endregion

        public PlottingViewModel(IMeasurementService measurementService, IPlottingDataService plottingDataService)
        {
            _measurementService = measurementService ?? throw new ArgumentNullException(nameof(measurementService));

            _measurementService.OnReceiveData += MetereologicalReceiveDataHandler;

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

                        UpdatePlottingData(metrologicalSource);
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
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;

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
            finally
            {
                IsBusy = false;
            }
        }

        #endregion

        #region Methods

        private void UpdatePlottingData(IEnumerable<MeteorologicalModel> metrologicalSource)
        {
            if (metrologicalSource != null)
            {
                MinTemperature = metrologicalSource.Min(x => x.Temperature);

                MaxTemperature = metrologicalSource.Max(x => x.Temperature);

                MinMeasurementDate = metrologicalSource.Min(x => x.MeteorologicalTime);

                MaxMeasurementDate = metrologicalSource.Max(x => x.MeteorologicalTime);

                TemperatureDataSource = metrologicalSource.Select(x => (double) x.Temperature).ToArray();

                DatetimeDataSource = metrologicalSource.Select(x => x.MeteorologicalTime).ToArray();
            }
        }

        #endregion

        #region Event Handlers

        private void MetereologicalReceiveDataHandler(object? sender, MeteorologicalEventArgs e)
        {
            UpdatePlottingData(e.Data);
        }

        #endregion
    }
}