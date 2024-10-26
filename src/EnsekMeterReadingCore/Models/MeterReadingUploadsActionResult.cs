using System;

namespace EnsekMeterReadingCore.Models;

public record MeterReadingUploadsActionResult(int SuccessCount, int FailedCount);