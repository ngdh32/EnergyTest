using EnsekMeterReadingCore.Models;

namespace EnsekMeterReadingCore.Helpers;

public interface IMeterReadingUploadCsvParser 
{
    public Task<MeterReadingUploadRowResult> GetMeterReadingFromLine(string lineText);
}