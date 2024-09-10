using EnsekMeterReadingApi.Actions;

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

        // Act
        await _testee.RunAsync();

        // Assert

    }
}