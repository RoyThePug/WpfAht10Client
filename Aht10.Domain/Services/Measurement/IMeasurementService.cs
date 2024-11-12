using Aht10.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aht10.Domain.CustomEventArgs;

namespace Aht10.Domain.Services.Measurement
{
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
}