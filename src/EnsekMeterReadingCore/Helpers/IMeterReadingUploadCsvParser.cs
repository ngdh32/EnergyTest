using EnsekMeterReadingCore.Entities;

namespace EnsekMeterReadingCore.Helpers;

public interface IMeterReadingUploadCsvParser 
{
    public MeterReadingEntity? GetMeterReadingFromLine(string lineText);
}