using System.Globalization;
using EnsekMeterReadingCore.Entities;

namespace EnsekMeterReadingCore.Helpers;

public class MeterReadingUploadCsvParser : IMeterReadingUploadCsvParser
{
    public MeterReadingEntity? GetMeterReadingFromLine(string lineText)
    {
        var columnCells = lineText.Split(",");
        if (columnCells.Count() != 4)
        {
            return null;
        }

        if (!int.TryParse(columnCells[0], out var accountId))
        {
            return null;
        }

        if (!DateTime.TryParseExact(columnCells[1], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var meterReadingDateTime))
        {
            return null;
        }

        if (!int.TryParse(columnCells[2], out var meterReadValue) ||
            (meterReadValue < 0 && columnCells[2].Length > 6) ||
            (meterReadValue >= 0 && columnCells[2].Length > 5) 
        )
        {
            return null;
        }

        return new MeterReadingEntity {
            AccountId = accountId,
            ReadingTime = meterReadingDateTime,
            Remark = columnCells[3],
            Value = meterReadValue
        };
    }
}