using System.Text;
using EnsekMeterReadingCore.Actions;
using EnsekMeterReadingCore.Actions.Implementations;
using EnsekMeterReadingCore.Entities;
using EnsekMeterReadingCore.Helpers;
using EnsekMeterReadingCore.Models;
using EnsekMeterReadingCore.Repositories;
using Moq;

namespace EnsekMeterReadingApiTest;

public class MeterReadingUploadsActionTests
{
    private readonly IMeterReadingUploadsAction _action;
    private readonly Mock<IMeterReadingRepository> _meterReadingRepositoryMock = new (MockBehavior.Strict);
    private readonly Mock<IMeterReadingUploadCsvParser> _readingCsvParserMock = new (MockBehavior.Strict);

    public MeterReadingUploadsActionTests(){
        _meterReadingRepositoryMock.Setup(x => x.AddAsync(It.IsAny<MeterReadingEntity>())).Returns(Task.CompletedTask);
        _meterReadingRepositoryMock.Setup(x => x.Save()).Returns(Task.CompletedTask);

        _action = new MeterReadingUploadsAction(_readingCsvParserMock.Object, _meterReadingRepositoryMock.Object);
    }

    [Fact]
    public async Task GivenValidMeterDataUploaded_WhenRun_ThenRunSuccessfully()
    {
        // Arrange
        var textFile =
            "AccountId,MeterReadingDateTime,MeterReadValue,\n2344,22/04/2019 09:24,1002,\n2233,22/04/2019 12:25,323,\n8766,22/04/2019 12:25,3440,";
        var byteArray = Encoding.UTF8.GetBytes(textFile);
        using var memoryStream = new MemoryStream(byteArray);
        
        _readingCsvParserMock.Setup(x => x.GetMeterReadingFromLine(It.Is<string>(y => y != "2344,22/04/2019 09:24,1002,"))).ReturnsAsync(() =>
            new MeterReadingUploadRowResult(true, new MeterReadingEntity()
            {
                Id = 1,
                AccountId = 1,
                Remark = string.Empty,
                ReadingTime = new DateTime(2024, 3, 18),
                ReadingValue = 100
            }));

        // Act
        var result = await _action.RunAsync(memoryStream);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.SuccessCount);
        Assert.Equal(1, result.FailedCount);
    }
}