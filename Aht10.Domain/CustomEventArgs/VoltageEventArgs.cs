
namespace Aht10.Domain.Common
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