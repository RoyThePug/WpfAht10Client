using Aht10.Domain.Models;
using System;
using System.Collections.Generic;

namespace Aht10.Domain.Common
{
    public class MeteorologicalEventArgs : EventArgs
    {
        public IEnumerable<MeteorologicalModel> Data { get; }

        public MeteorologicalEventArgs(IEnumerable<MeteorologicalModel> data)
        {
            Data = data;
        }
    }
}