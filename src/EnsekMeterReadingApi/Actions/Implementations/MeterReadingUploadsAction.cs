using System;

namespace EnsekMeterReadingApi.Actions;

public class MeterReadingUploadsAction : IMeterReadingUploadsAction
{
    public Task RunAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
