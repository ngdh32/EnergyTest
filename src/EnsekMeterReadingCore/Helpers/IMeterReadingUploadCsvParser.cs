using EnsekMeterReadingCore.Models;

namespace EnsekMeterReadingCore.Helpers;

public interface IMeterReadingUploadCsvParser 
{
    public MeterReadingUploadRowResult GetMeterReadingFromLine(string lineText);
}