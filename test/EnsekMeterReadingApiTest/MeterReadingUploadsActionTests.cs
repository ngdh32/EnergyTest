using EnsekMeterReadingCore.Actions;
using EnsekMeterReadingCore.Actions.Implementations;
using EnsekMeterReadingCore.Entities;
using EnsekMeterReadingCore.Helpers;
using EnsekMeterReadingCore.Helpers.Implementations;
using EnsekMeterReadingCore.Repositories;
using Moq;

namespace EnsekMeterReadingApiTest;

public class MeterReadingUploadsActionTests
{
    private readonly IMeterReadingUploadsAction _testee;
    private readonly Mock<IAccountRepository> _accountRepositoryMock = new (MockBehavior.Strict);
    private readonly Mock<IMeterReadingRepository> _meterReadingRepositoryMock = new (MockBehavior.Strict);

    public MeterReadingUploadsActionTests(){
        _meterReadingRepositoryMock.Setup(x => x.AddAsync(It.IsAny<MeterReadingEntity>())).Returns(Task.CompletedTask);
        _meterReadingRepositoryMock.Setup(x => x.Save()).Returns(Task.CompletedTask);

        _testee = new MeterReadingUploadsAction(new MeterReadingUploadCsvParser(_accountRepositoryMock.Object), _meterReadingRepositoryMock.Object);
    }

    [Fact]
    public async Task GivenValidMeterDataUploaded_WhenRun_ThenRunSuccessfully()
    {
        // Arrange
        using var fileStream = new FileStream("TestData/Meter_Reading.csv", FileMode.Open, FileAccess.Read);
        _accountRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new AccountEntity(){
            Id = 1,
            FirstName = "Test",
            LastName = "Test"
        });

        // Act
        var result = await _testee.RunAsync(fileStream);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(34, result.SuccessCount);
        Assert.Equal(1, result.FailedCount);
    }
    

    [Fact]
    public async Task GivenValidMeterDataUploadedOneAccountNotFound_WhenRun_ThenRunSuccessfully()
    {
        // Arrange
        using var fileStream = new FileStream("TestData/Meter_Reading.csv", FileMode.Open, FileAccess.Read);
        _accountRepositoryMock.Setup(x => x.GetByIdAsync(It.Is<int>(y => y != 2344))).ReturnsAsync(new AccountEntity(){
            Id = 1,
            FirstName = "Test",
            LastName = "Test"
        });

        _ = _accountRepositoryMock.Setup(x => x.GetByIdAsync(It.Is<int>(y => y == 2344))).ReturnsAsync((AccountEntity)null);


        // Act
        var result = await _testee.RunAsync(fileStream);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(31, result.SuccessCount);
        Assert.Equal(4, result.FailedCount);
    }
}