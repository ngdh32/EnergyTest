using EnsekMeterReadingCore.Helpers;
using EnsekMeterReadingCore.Models;
using EnsekMeterReadingCore.Repositories;

namespace EnsekMeterReadingCore.Actions.Implementations;

public class MeterReadingUploadsAction : IMeterReadingUploadsAction
{
    private readonly IMeterReadingUploadCsvParser _csvParser;
    private readonly IMeterReadingRepository _meterReadingRepository;

    public MeterReadingUploadsAction(IMeterReadingUploadCsvParser csvParser, IMeterReadingRepository meterReadingRepository)
    {
        _csvParser = csvParser;
        _meterReadingRepository = meterReadingRepository;
    }

    public async Task<MeterReadingUploadsActionResult> RunAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        var successCount = 0;
        var failedCount = 0;

        using var reader = new StreamReader(stream);
        var lineText = string.Empty;

        var index = 0;
        while((lineText = await reader.ReadLineAsync(cancellationToken)) != null)
        {
            try 
            {
                if (index == 0){
                    continue;
                }

                var result = await _csvParser.GetMeterReadingFromLine(lineText);
                if (!result.Successful)
                {
                    failedCount++;
                    continue;
                }

                var record = result.MeterReadingEntity;

                await _meterReadingRepository.AddAsync(record);
                successCount++;
            } 
            catch(Exception)
            {
                failedCount++;
            }
            finally
            {
                index++;
            }
        }

        await _meterReadingRepository.Save();
        return new MeterReadingUploadsActionResult(successCount, failedCount);
    }
}
