using System;

namespace WpfAht10Client.Models;

public class MeteorologicalModel
{
    public int Id { get; set; }
    public int MeasurementDataId { get; set; }
    public float Temperature { get; set; }
    public float Humidity { get; set; }
    public DateTime MeteorologicalTime { get; set; }
}