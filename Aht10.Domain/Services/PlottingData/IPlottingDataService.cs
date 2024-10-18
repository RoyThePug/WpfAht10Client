
using System;
using System.Threading.Tasks;

namespace Aht10.Domain.Services
{
    public interface IPlottingDataService
    {
        Task<bool> SaveMeasurementImageBytes(string jsonData);
    }
}

