using Aht10.Domain.Enums;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Data;
using WpfAht10Client.Common;
using WpfAht10Client.Models;
using WpfAht10Client.Services.Measurement;

namespace WpfAht10Client.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        #region Fields

        private readonly IMeasurementService _measurementService;
        private readonly object _itemsLock;

        #endregion

        #region Properties

        public ObservableCollection<LogModel> LogSource { get; }

        #endregion

        public LoginViewModel(IMeasurementService measurementService)
        {
            _measurementService = measurementService ?? throw new ArgumentNullException(nameof(measurementService));

            _measurementService.OnReceiveData += MetereologicalReceiveDataHandler;

            _itemsLock = new object();       

            LogSource = new ObservableCollection<LogModel>();

            BindingOperations.EnableCollectionSynchronization(LogSource, _itemsLock);
        }

        #region Event Handler

        private void MetereologicalReceiveDataHandler(object? sender, MeteorologicalEventArgs e)
        {
            if (e.Data is IEnumerable<MeteorologicalModel> metrologicalSource)
            {
                lock (_itemsLock)
                {
                    LogSource.Add(new LogModel(LogLevel.Success, "metrological data received"));
                }
            }
        }

        #endregion
    }
}
