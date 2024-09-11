using EnsekMeterReadingCore.Actions;
using EnsekMeterReadingCore.Helpers;
using Moq;

namespace EnsekMeterReadingApiTest;

public class MeterReadingUploadsActionTests
{
    private readonly IMeterReadingUploadsAction _testee;

    public MeterReadingUploadsActionTests(){
        _testee = new MeterReadingUploadsAction(new MeterReadingUploadCsvParser());
    }

    [Fact]
    public async Task GivenValidMeterDataUploaded_WhenRun_ThenRunSuccessfully()
    {
        // Arrange
        using var fileStream = new FileStream("TestData/Meter_Reading.csv", FileMode.Open, FileAccess.Read);

        // Act
        var result = await _testee.RunAsync(fileStream);

        // Assert
        Assert.Equal(35, result);
    }
}