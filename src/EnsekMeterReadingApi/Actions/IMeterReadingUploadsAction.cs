using System;

namespace EnsekMeterReadingApi.Actions;

public interface IMeterReadingUploadsAction
{
    public Task RunAsync(Stream stream, CancellationToken cancellationToken = default);
}
