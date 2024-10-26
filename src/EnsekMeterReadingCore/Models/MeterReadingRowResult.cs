using EnsekMeterReadingCore.Entities;

namespace EnsekMeterReadingCore.Models;

public record MeterReadingRowResult(bool Successful = false, MeterReadingEntity? MeterReadingEntity = null);