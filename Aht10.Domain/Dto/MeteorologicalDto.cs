﻿using System;

namespace Aht10.Domain.Dto
{
    public class MeteorologicalDto
    {
        public int Id { get; set; }
        public float Temperature { get; set; }
        public float Humidity { get; set; }
        public DateTime MeteorologicalTime;
    }
}