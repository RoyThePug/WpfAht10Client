using System.Collections.ObjectModel;
using System.Windows.Data;
using Aht10.Domain.Enums;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WpfAht10Client.Common;
using WpfAht10Client.Models;
using WpfAht10Client.Services.Measurement;

namespace WpfAht10Client.ViewModels;

public partial class MainViewModel : ObservableObject
{
    #region Fields

    private readonly IMeasurementService _measurementService;
    private readonly object _itemsLock;
    private readonly LoginViewModel _loginViewModel;

    #endregion

    #region Properties

    public bool IsConnected { get; set; }

    public string CurrentDate => DateTime.Now.Date.ToShortDateString();

    public bool IsNotBusy => !IsBusy;

    [ObservableProperty] public bool isBusy;

    [ObservableProperty] public int meteorologicalCount; 

    [ObservableProperty] public MeasurementModel selectedMeasurement;

    [ObservableProperty] public double batteryCapacity;

    public ObservableCollection<MeteorologicalModel> MeteorologicalSource { get; }

    public ObservableCollection<MeasurementModel> MeasurementSource { get; }

    #endregion

    public MainViewModel(IMeasurementService measurementService, LoginViewModel loginViewModel)
    {
        _measurementService = measurementService ?? throw new ArgumentNullException(nameof(measurementService));

        _measurementService.OnReceiveData += MetereologicalReceiveDataHandler;

        _measurementService.OnReceiveVoltage += MeasurementServiceOnOnReceiveVoltage;

        _itemsLock = new object();

        _loginViewModel = loginViewModel ?? throw new ArgumentNullException(nameof(loginViewModel));

        MeteorologicalSource = new ObservableCollection<MeteorologicalModel>();

        MeasurementSource = new ObservableCollection<MeasurementModel>();

        BindingOperations.EnableCollectionSynchronization(MeasurementSource, _itemsLock);
        BindingOperations.EnableCollectionSynchronization(MeteorologicalSource, _itemsLock);

        OnPropertyChanged(nameof(CurrentDate));
    }

    #region Commands

    [RelayCommand]
    public async Task ConnectAsync()
    {
        try
        {
            IsConnected = await _measurementService.ConnectAsync();

            OnPropertyChanged(nameof(IsConnected));

            _loginViewModel.LogSource.Add(new LogModel(LogLevel.Success, $"Connected to Server"));
        }
        catch (Exception)
        {
            // ignored
        }
    }


    [RelayCommand]
    public async Task DisconnectAsync()
    {
        try
        {
            IsConnected = await _measurementService.CloseAsync();

            OnPropertyChanged(nameof(IsConnected));

            _loginViewModel.LogSource.Add(new LogModel(LogLevel.Success, $"Connection Closed"));
        }
        catch (Exception)
        {
            // ignored
        }
    }

    [RelayCommand]
    public async Task LoadMetrologicalDataAsync()
    {
        try
        {
            await GetMetrologicalSourceByDateAsync();

            _loginViewModel.LogSource.Add(new LogModel(LogLevel.Success, "metrological data received"));
        }
        catch (Exception)
        {
            _loginViewModel.LogSource.Add(new LogModel(LogLevel.Success, "error metrological data received"));
        }
    }

    [RelayCommand]
    public async Task LoadMeasurementDataAsync()
    {
        try
        {
            var data = await _measurementService.GetMeasurementDataAsync();

            var activeMeasurement = SelectedMeasurement;

            if (MeasurementSource.Any())
            {
                MeasurementSource.Clear();
            }

            foreach (var item in data)
            {
                MeasurementSource.Add(item);
            }

            if (activeMeasurement is null)
            {
                SelectedMeasurement = MeasurementSource.Last();

                _loginViewModel.LogSource.Add(new LogModel(LogLevel.Success, "measurement data received"));
            }
            else
            {
                SelectedMeasurement = activeMeasurement;
            }

            OnPropertyChanged(nameof(SelectedMeasurement));
        }
        catch (Exception)
        {
            _loginViewModel.LogSource.Add(new LogModel(LogLevel.Success, "error measurement data received"));
        }
    }

    [RelayCommand]
    public async Task ChangeSelectedMeasurementDateAsync()
    {
        try
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            await GetMetrologicalSourceByDateAsync();
        }
        catch (Exception)
        {
            // ignored
        }

        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public Task RetrieveBatteryCapacityAsync()
    {
        BatteryCapacity = new Random().Next(0, 100);

        return Task.CompletedTask;
    }

    [RelayCommand]
    public async Task TestAsync()
    {
        try
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            OnPropertyChanged(nameof(IsNotBusy));

            await LoadMetrologicalDataCommand.ExecuteAsync(null);

            await LoadMeasurementDataCommand.ExecuteAsync(null);
        }
        finally
        {
            IsBusy = false;

            OnPropertyChanged(nameof(IsNotBusy));
        }
    }

    #endregion

    #region Methods

    private async Task GetMetrologicalSourceByDateAsync()
    {
        try
        {
            if (SelectedMeasurement != null)
            {
                var data = await _measurementService.GetMeteorologicalDataByDateAsync(SelectedMeasurement.Id);

                if (MeteorologicalSource.Any())
                {
                    MeteorologicalSource.Clear();
                }

                foreach (var item in data)
                {
                    MeteorologicalSource.Add(item);
                }

                MeteorologicalCount = MeteorologicalSource.Count;

                _loginViewModel.LogSource.Add(new LogModel(LogLevel.Success, "metrological data by selected measurement received"));
            }
        }
        catch (Exception)
        {
            _loginViewModel.LogSource.Add(new LogModel(LogLevel.Error, "metrological data by selected measurement received"));
        }
    }

    #endregion

    #region Event Handler

    private void MetereologicalReceiveDataHandler(object? sender, MeteorologicalEventArgs e)
    {
        lock (_itemsLock)
        {
            if (MeteorologicalSource.Any())
            {
                MeteorologicalSource.Clear();
            }

            foreach (var item in e.Data)
            {
                MeteorologicalSource.Add(item);
            }

            MeteorologicalCount = MeteorologicalSource.Count;

            _loginViewModel.LogSource.Add(new LogModel(LogLevel.Success, "metrological data received"));
        }
    }

    private void MeasurementServiceOnOnReceiveVoltage(object? sender, VoltageEventArgs e)
    {
        lock (_itemsLock)
        {
            BatteryCapacity = e.Voltage;
        }
    }

    #endregion
}