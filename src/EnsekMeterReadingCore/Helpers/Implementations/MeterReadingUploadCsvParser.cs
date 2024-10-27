using System.Globalization;
using EnsekMeterReadingCore.Entities;
using EnsekMeterReadingCore.Models;
using EnsekMeterReadingCore.Repositories;

namespace EnsekMeterReadingCore.Helpers.Implementations;

public class MeterReadingUploadCsvParser : IMeterReadingUploadCsvParser
{
    private readonly IAccountRepository _accountRepository;
    private readonly IMeterReadingRepository _readingRepository;

    public MeterReadingUploadCsvParser(IAccountRepository accountRepository, IMeterReadingRepository readingRepository)
    {
        _accountRepository = accountRepository;
        _readingRepository = readingRepository;
    }

    public async Task<MeterReadingUploadRowResult> GetMeterReadingFromLine(string lineText)
    {
        var columnCells = lineText.Split(",");
        if (columnCells.Count() != 4)
        {
            return new MeterReadingUploadRowResult("Row is not in the right format");
        }

        if (!int.TryParse(columnCells[0], out var accountId))
        {
            return new MeterReadingUploadRowResult("Account Id is not in the right format");
        }

        if (!DateTime.TryParseExact(columnCells[1], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var meterReadingDateTime))
        {
            return new MeterReadingUploadRowResult("Date and Time is not in the right format");
        }

        if (!int.TryParse(columnCells[2], out var meterReadValue) ||
            (meterReadValue < 0 && columnCells[2].Length > 6) ||
            (meterReadValue >= 0 && columnCells[2].Length > 5) 
        )
        {
            return new MeterReadingUploadRowResult("Meter Value is not in the right format");
        }
        
        var account = await _accountRepository.GetByIdAsync(accountId);
        if (account == null)
        {
            return new MeterReadingUploadRowResult("Account Not Found");
        }
        
        var accountLastReadingTime = _readingRepository.GetAccountLastReadingTime(account.Id);
        if (accountLastReadingTime != null && accountLastReadingTime > meterReadingDateTime)
        {
            return new MeterReadingUploadRowResult("Account Last Reading Time is larger than meter reading time");
        }
        
        return new MeterReadingUploadRowResult(string.Empty, true, new MeterReadingEntity
        {
            AccountId = accountId,
            ReadingTime = meterReadingDateTime,
            Remark = columnCells[3],
            ReadingValue = meterReadValue
        });
    }
}