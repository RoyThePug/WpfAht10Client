using Aht10.Domain.Enums;
using System;

namespace Aht10.Domain.Models
{
    public class LogModel
    {
        public LogLevel LogLevel { get; }
        public string Message { get; }
        public DateTime Time { get; }

        public LogModel(LogLevel logLevel, string message)
        {
            LogLevel = logLevel;

            Message = message;

            Time = DateTime.Now;

        }
    }
}