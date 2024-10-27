using EnsekMeterReadingCore.Entities;

namespace EnsekMeterReadingCore.Models;

public record MeterReadingUploadRowResult(string Message, bool Successful = false, MeterReadingEntity? MeterReadingEntity = null)
{
    public MeterReadingUploadRowResult(bool successful, MeterReadingEntity? meterReadingEntity) : this (string.Empty, successful, meterReadingEntity) {}
}