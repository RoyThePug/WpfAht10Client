using System;

namespace WpfAht10Client.Models;

public class PlotMeteorologicalModel
{
    public float Temperature { get; }
    public DateTime MeteorologicalTime { get; }

    public PlotMeteorologicalModel(float temperature, DateTime meteorologicalTime)
    {
        Temperature = temperature;
        MeteorologicalTime = meteorologicalTime;
    }
}