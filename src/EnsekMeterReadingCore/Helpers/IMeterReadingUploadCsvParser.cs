using EnsekMeterReadingCore.Entities;

namespace EnsekMeterReadingCore.Helpers;

public interface IMeterReadingUploadCsvParser 
{
    public MeterReading? GetAllMeterRecordingsFromUpload(string lineText);
}