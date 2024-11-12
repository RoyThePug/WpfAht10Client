using Aht10.Domain.Models;
using Aht10.Domain.Services.Measurement;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework.Internal;
using RichardSzalay.MockHttp;
using Constants = Aht10.Domain.Constants;

namespace Aht10.Tests.Domain.Tests;

public class MeasurementServiceTests
{
    private Mock<IHttpClientFactory> _mockHttpClientFactory;
    private MockHttpMessageHandler _mockHttp;

    [SetUp]
    public void Setup()
    {
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        _mockHttp = new MockHttpMessageHandler();
    }

    [TearDown]
    public void TearDown()
    {
        _mockHttp.Dispose();
    }

    [Test]
    public async Task Get_meteorological_data_success()
    {
        var mockData = new List<MeteorologicalModel>
        {
            new() { Id = 1, MeasurementDataId = 1 },
            new() { Id = 2, MeasurementDataId = 1 },
            new() { Id = 2, MeasurementDataId = 2 }
        };
        
        var jsonData = JsonConvert.SerializeObject(mockData);
        _mockHttp.When(Constants.Url).Respond("application/json", jsonData);
        _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>()))
            .Returns(_mockHttp.ToHttpClient()).Verifiable();
        var sut = new MeasurementService(_mockHttpClientFactory.Object);
        
        
        var data = await sut.GetMeteorologicalDataAsync();
        

        Assert.NotNull(data);
    }
}