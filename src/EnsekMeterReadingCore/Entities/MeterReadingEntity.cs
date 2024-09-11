using System;

namespace EnsekMeterReadingCore.Entities;

public class MeterReadingEntity : BaseEntity
{
    public required int AccountId { get; set; }

    public required DateTime ReadingTime { get; set; }

    public required int ReadingValue { get; set; }

    public required string Remark { get; set;}
}
