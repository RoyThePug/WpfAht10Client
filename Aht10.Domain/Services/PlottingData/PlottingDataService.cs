
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Aht10.Domain.Services
{
    public class PlottingDataService : IPlottingDataService
    {
        #region Fields

        private readonly IHttpClientFactory _httpClientFactory;

        #endregion

        public PlottingDataService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<bool> SaveMeasurementImageBytes(string jsonData)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                await client.PostAsync(Constants.Url + $"/add_measurement_image", new StringContent(jsonData, Encoding.UTF8, Application.Json));

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}