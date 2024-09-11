using System;
using System.Globalization;
using EnsekMeterReadingCore.Helpers;

namespace EnsekMeterReadingCore.Actions;

public class MeterReadingUploadsAction : IMeterReadingUploadsAction
{
    private readonly IMeterReadingUploadCsvParser _csvParser;

    public MeterReadingUploadsAction(IMeterReadingUploadCsvParser csvParser)
    {
        _csvParser = csvParser;
    }

    public async Task<int> RunAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        var successfulReadCount = 0;

        using var reader = new StreamReader(stream);
        var lineText = string.Empty;

        while((lineText = await reader.ReadLineAsync(cancellationToken)) != null)
        {
            var record = _csvParser.GetAllMeterRecordingsFromUpload(lineText);
            if (record == null)
            {
                continue;
            }

            // TODO Check if the account exists and save it to the database

            successfulReadCount++;
        }

        return successfulReadCount;
    }
}
