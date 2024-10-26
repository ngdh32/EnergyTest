using EnsekMeterReadingCore.Entities;

namespace EnsekMeterReadingCore.Models;

public record MeterReadingUploadRowResult(bool Successful = false, MeterReadingEntity? MeterReadingEntity = null);