using System.Globalization;
using EnsekMeterReadingCore.Entities;
using EnsekMeterReadingCore.Models;

namespace EnsekMeterReadingCore.Helpers.Implementations;

public class MeterReadingUploadCsvParser : IMeterReadingUploadCsvParser
{
    public MeterReadingUploadRowResult GetMeterReadingFromLine(string lineText)
    {
        var columnCells = lineText.Split(",");
        if (columnCells.Count() != 4)
        {
            return new MeterReadingUploadRowResult();
        }

        if (!int.TryParse(columnCells[0], out var accountId))
        {
            return new MeterReadingUploadRowResult();
        }

        if (!DateTime.TryParseExact(columnCells[1], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var meterReadingDateTime))
        {
            return new MeterReadingUploadRowResult();
        }

        if (!int.TryParse(columnCells[2], out var meterReadValue) ||
            (meterReadValue < 0 && columnCells[2].Length > 6) ||
            (meterReadValue >= 0 && columnCells[2].Length > 5) 
        )
        {
            return new MeterReadingUploadRowResult();
        }


        return new MeterReadingUploadRowResult(true, new MeterReadingEntity
        {
            AccountId = accountId,
            ReadingTime = meterReadingDateTime,
            Remark = columnCells[3],
            ReadingValue = meterReadValue
        });
    }
}