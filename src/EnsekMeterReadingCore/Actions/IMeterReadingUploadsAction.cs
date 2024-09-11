using System;

namespace EnsekMeterReadingCore.Actions;

public interface IMeterReadingUploadsAction
{
    public Task<int> RunAsync(Stream stream, CancellationToken cancellationToken = default);
}
