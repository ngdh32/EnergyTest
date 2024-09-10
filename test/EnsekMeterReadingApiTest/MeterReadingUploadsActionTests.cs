using EnsekMeterReadingApi.Actions;
using Moq;

namespace EnsekMeterReadingApiTest;

public class MeterReadingUploadsActionTests
{
    private readonly IMeterReadingUploadsAction _testee;

    public MeterReadingUploadsActionTests(){
        _testee = new MeterReadingUploadsAction();
    }

    [Fact]
    public async Task GivenValidMeterDataUploaded_WhenRun_ThenRunSuccessfully()
    {
        // Arrange
        using var fileStream = new FileStream("TestData/Meter_Reading.csv", FileMode.Open, FileAccess.Read);
        using var reader = new StreamReader(fileStream);
        var text = await reader.ReadToEndAsync();

        // Act
        await _testee.RunAsync(fileStream);

        // Assert
        
    }
}