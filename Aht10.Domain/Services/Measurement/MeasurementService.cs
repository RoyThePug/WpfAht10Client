using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Aht10.Domain.Common;
using Aht10.Domain.Models;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Aht10.Domain.Dto;

namespace Aht10.Domain.Services.Measurement
{
    public class MeasurementService : IMeasurementService
    {
        #region Fields

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HubConnection _hubConnection;
        private double _maxVoltage = 3.05;
        private double _minVoltage = 2.11;

        #endregion

        public string Url { get; }
        public event EventHandler<MeteorologicalEventArgs>? OnReceiveData;
        public event EventHandler<VoltageEventArgs>? OnReceiveVoltage;

        public MeasurementService(IHttpClientFactory httpClientFactory)
        {
            Url = "http://192.168.0.7:8089";

            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));

            _hubConnection = new HubConnectionBuilder().WithUrl($"{Constants.Url}/connectionservice")
                                                       .WithAutomaticReconnect()
                                                       .Build();

            _hubConnection.On<string>("message", json =>
            {
                var source = JsonConvert.DeserializeObject<IEnumerable<MeteorologicalModel>>(json);

                if (OnReceiveData != null)
                {
                    if (source != null)
                    {
                        OnReceiveData.Invoke(this, new MeteorologicalEventArgs(source));
                    }
                }
            });

            _hubConnection.On<string>("voltage-api", json =>
            {
                var voltage = JsonConvert.DeserializeObject<BatteryVoltageDto>(json);

                if (voltage.Voltage > _maxVoltage)
                {
                    voltage.Voltage = (float)_maxVoltage;
                }

                var currentCapacity = (voltage.Voltage - _minVoltage) / (_maxVoltage - _minVoltage) * 100;

                if (OnReceiveVoltage != null)
                {
                    if (voltage != null)
                    {
                        OnReceiveVoltage.Invoke(this, new VoltageEventArgs(currentCapacity));
                    }
                }
            });
        }

        /// <summary>
        /// Establish a live connection using SignalR
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> ConnectAsync()
        {
            try
            {
                await _hubConnection.StartAsync();

                return _hubConnection.ConnectionId != null;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<bool> CloseAsync()
        {
            try
            {
                await _hubConnection.StopAsync();

                return !(_hubConnection.ConnectionId == null);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Get All Meteorological information
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<MeteorologicalModel>> GetMeteorologicalDataAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                var response = await client.GetAsync(Url);

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<IEnumerable<MeteorologicalModel>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<MeteorologicalModel>> GetMeteorologicalDataByDateAsync(int dateId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient(Url);

                var response = await client.GetAsync(Url + $"/get-measurement-byDate/{dateId}");

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<IEnumerable<MeteorologicalModel>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///  Get All Measurement information
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<MeasurementModel>> GetMeasurementDataAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                var response = await client.GetAsync(Url + "/measurements");

                response.EnsureSuccessStatusCode();

                //return await response.Content.ReadFromJsonAsync<IEnumerable<MeasurementModel>>();
                return await client.GetFromJsonAsync<IEnumerable<MeasurementModel>>(Url + "/measurements");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<MeasurementModel?> GetMeasurementByDataAsync(DateTime date)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                return await client.GetFromJsonAsync<MeasurementModel>(Url + $"/get-measurementInfo-byDate/{date.ToString("yyyy-MM-dd HH:mm:ss")}");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}