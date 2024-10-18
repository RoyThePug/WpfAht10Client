
namespace WpfAht10Client.Common;

public class VoltageEventArgs
{
    public double Voltage { get; }

    public VoltageEventArgs(double voltage)
    {
        Voltage = voltage;
    }
}