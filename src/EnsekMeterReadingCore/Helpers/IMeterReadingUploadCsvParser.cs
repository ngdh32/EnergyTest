using EnsekMeterReadingCore.Entities;

namespace EnsekMeterReadingCore.Helpers;

public interface IMeterReadingUploadCsvParser 
{
    public MeterReading? GetMeterReadingFromLine(string lineText);
}