using System;
using EnsekMeterReadingCore.Helpers;

namespace EnsekMeterReadingApiTest;

public class MeterReadingUploadCsvParserTest
{
    private readonly IMeterReadingUploadCsvParser _testee;

    public MeterReadingUploadCsvParserTest()
    {
        _testee = new MeterReadingUploadCsvParser();
    }

    [Fact]
    public void GivenValidLine_WheRun_ThenReturnEntity()
    {
        // Arrange 
        const string lineText = "2344,22/04/2019 09:24,1002,";

        // Act
        var result = _testee.GetMeterReadingFromLine(lineText);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2344, result.AccountId);
        Assert.Equal(new DateTime(2019,4,22,9,24,00), result.ReadingTime);
        Assert.Equal(1002, result.Value);
        Assert.Equal("", result.Remark);
    }

    [Fact]
    public void GivenInvalidAccountId_WheRun_ThenReturnNull()
    {
        // Arrange 
        const string lineText = "abc,22/04/2019 09:24,1002,";

        // Act
        var result = _testee.GetMeterReadingFromLine(lineText);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GivenInvalidRecordDate_WheRun_ThenReturnNull()
    {
        // Arrange 
        const string lineText = "2344,abc,1002,";

        // Act
        var result = _testee.GetMeterReadingFromLine(lineText);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GivenInvalidValue_WheRun_ThenReturnNull()
    {
        // Arrange 
        const string lineText = "2344,22/04/2019 09:24,abc,";

        // Act
        var result = _testee.GetMeterReadingFromLine(lineText);

        // Assert
        Assert.Null(result);
    }
}
