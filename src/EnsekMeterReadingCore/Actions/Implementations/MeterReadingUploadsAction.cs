using System;
using System.Globalization;
using EnsekMeterReadingCore.Helpers;
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

    public async Task<int> RunAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        var successfulReadCount = 0;

        using var reader = new StreamReader(stream);
        var lineText = string.Empty;

        while((lineText = await reader.ReadLineAsync(cancellationToken)) != null)
        {
            var record = _csvParser.GetMeterReadingFromLine(lineText);
            if (record == null)
            {
                continue;
            }

            var account = await _accountRepository.GetByIdAsync(record.AccountId);
            if (account == null)
            {
                continue;
            }

            await _meterReadingRepository.AddAsync(record);
            successfulReadCount++;
        }

        return successfulReadCount;
    }
}
