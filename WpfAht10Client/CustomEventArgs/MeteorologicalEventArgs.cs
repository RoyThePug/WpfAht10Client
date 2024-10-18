using WpfAht10Client.Models;

namespace WpfAht10Client.Common;

public class MeteorologicalEventArgs : EventArgs
{
    public IEnumerable<MeteorologicalModel> Data { get; }

    public MeteorologicalEventArgs(IEnumerable<MeteorologicalModel> data)
    {
        Data = data;
    }
}