
namespace Aht10.Domain.CustomEventArgs
{
    public class VoltageEventArgs
    {
        public double Voltage { get; }

        public VoltageEventArgs(double voltage)
        {
            Voltage = voltage;
        }
    }
}