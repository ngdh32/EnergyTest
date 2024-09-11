using System;

namespace EnsekMeterReadingCore.Models;

public record MeterReadingUploadsActionResult(int successCount, int failedCount);