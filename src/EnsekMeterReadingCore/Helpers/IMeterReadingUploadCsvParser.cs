using EnsekMeterReadingCore.Models;

namespace EnsekMeterReadingCore.Helpers;

public interface IMeterReadingUploadCsvParser 
{
    public MeterReadingRowResult GetMeterReadingFromLine(string lineText);
}