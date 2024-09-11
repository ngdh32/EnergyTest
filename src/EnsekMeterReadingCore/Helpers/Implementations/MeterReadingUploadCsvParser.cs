using System.Globalization;
using EnsekMeterReadingCore.Entities;

namespace EnsekMeterReadingCore.Helpers;

public class MeterReadingUploadCsvParser : IMeterReadingUploadCsvParser
{
    public MeterReading? GetMeterReadingFromLine(string lineText)
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

        if (!int.TryParse(columnCells[2], out var meterReadValue))
        {
            return null;
        }

        return new MeterReading {
            AccountId = accountId,
            ReadingTime = meterReadingDateTime,
            Remark = columnCells[3],
            Value = meterReadValue
        };
    }
}