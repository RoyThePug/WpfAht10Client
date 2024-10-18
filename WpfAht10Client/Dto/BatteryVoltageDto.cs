namespace WpfAht10Client.Dto;

public class BatteryVoltageDto
{
    private double _maxVoltage = 3.05;
    public float Voltage { get; set; }
    public double VoltageCapacity
    {
        get
        {
            var value = Voltage / _maxVoltage * 100;
            
            if (value > 100)
            {
                return 100;
            }
            
            return value;
        }
    }
}