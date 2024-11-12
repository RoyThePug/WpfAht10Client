using Aht10.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Aht10.Domain.Services.Measurement;
using WpfAht10Client.ViewModels;

namespace WpfAht10Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider? ServiceProvider { get; private set; }

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddHttpClient();
            // services.AddHttpClient("signed").ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
            // {
            //     ClientCertificateOptions = ClientCertificateOption.Manual,
            //     ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            // });
            services.AddSingleton<IMeasurementService, MeasurementService>();
            services.AddTransient<IPlottingDataService, PlottingDataService>();
            services.AddSingleton<PlottingViewModel>();
            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<MainWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = ServiceProvider.GetService<MainWindow>();
            mainWindow?.Show();

            base.OnStartup(e);
        }
    }
}
