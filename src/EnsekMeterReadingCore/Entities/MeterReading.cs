using System;

namespace EnsekMeterReadingCore.Entities;

public class MeterReading
{
    public required int AccountId { get; set; }

    public required DateTime ReadingTime { get; set; }

    public required int Value { get; set; }
}
