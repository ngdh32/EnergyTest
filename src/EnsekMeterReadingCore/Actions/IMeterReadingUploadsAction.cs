using EnsekMeterReadingCore.Models;

namespace EnsekMeterReadingCore.Actions;

public interface IMeterReadingUploadsAction
{
    public Task<MeterReadingUploadsActionResult> RunAsync(Stream stream, CancellationToken cancellationToken = default);
}
