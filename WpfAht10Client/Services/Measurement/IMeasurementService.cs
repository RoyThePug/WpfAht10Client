using WpfAht10Client.Common;
using WpfAht10Client.Models;

namespace WpfAht10Client.Services.Measurement;

public interface IMeasurementService
{
    public event EventHandler<MeteorologicalEventArgs> OnReceiveData;
    
    public event EventHandler<VoltageEventArgs>? OnReceiveVoltage;
    
    public Task<IEnumerable<MeteorologicalModel>> GetMeteorologicalDataAsync();
    
    public Task<IEnumerable<MeteorologicalModel>> GetMeteorologicalDataByDateAsync(int dateId);

    public Task<MeasurementModel?> GetMeasurementByDataAsync(DateTime date);
    public Task<IEnumerable<MeasurementModel>> GetMeasurementDataAsync();

    public Task<bool> ConnectAsync();

    public Task<bool> CloseAsync();
}