using System;
using System.Globalization;
using EnsekMeterReadingCore.Helpers;
using EnsekMeterReadingCore.Models;
using EnsekMeterReadingCore.Repositories;

namespace EnsekMeterReadingCore.Actions;

public class MeterReadingUploadsAction : IMeterReadingUploadsAction
{
    private readonly IMeterReadingUploadCsvParser _csvParser;
    private readonly IAccountRepository _accountRepository;
    private readonly IMeterReadingRepository _meterReadingRepository;

    public MeterReadingUploadsAction(IMeterReadingUploadCsvParser csvParser, IAccountRepository accountRepository, IMeterReadingRepository meterReadingRepository)
    {
        _csvParser = csvParser;
        _accountRepository = accountRepository;
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

                var result = _csvParser.GetMeterReadingFromLine(lineText);
                if (!result.Successful)
                {
                    failedCount++;
                    continue;
                }

                var record = result.MeterReadingEntity;
                var account = await _accountRepository.GetByIdAsync(record.AccountId);
                if (account == null)
                {
                    failedCount++;
                    continue;
                }

                await _meterReadingRepository.AddAsync(record);
                await _meterReadingRepository.Save();
                successCount++;
            } 
            catch(Exception ex)
            {
                failedCount++;
            }
            finally
            {
                
                index++;
            }
        }

        return new MeterReadingUploadsActionResult(successCount, failedCount);
    }
}
