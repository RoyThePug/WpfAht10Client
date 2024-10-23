using System;

namespace Aht10.Domain.Models
{
    public class MeasurementModel
    {
        public int Id { get; set; }
        public DateTime MeasurementDate { get; set; }
        public int MetrologicalCount { get; set; }
        public byte[] MeasurementImage { get; set; }
    }
}