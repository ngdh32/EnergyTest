using System;
using EnsekMeterReadingCore.Helpers;
using EnsekMeterReadingCore.Helpers.Implementations;

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
        Assert.True(result.Successful);
        Assert.NotNull(result.MeterReadingEntity);
        Assert.Equal(2344, result.MeterReadingEntity.AccountId);
        Assert.Equal(new DateTime(2019,4,22,9,24,00), result.MeterReadingEntity.ReadingTime);
        Assert.Equal(1002, result.MeterReadingEntity.ReadingValue);
        Assert.Equal("", result.MeterReadingEntity.Remark);
    }

    [Fact]
    public void GivenInvalidAccountId_WheRun_ThenReturnNull()
    {
        // Arrange 
        const string lineText = "abc,22/04/2019 09:24,1002,";

        // Act
        var result = _testee.GetMeterReadingFromLine(lineText);

        // Assert
        Assert.False(result.Successful);
    }

    [Fact]
    public void GivenInvalidRecordDate_WheRun_ThenReturnNull()
    {
        // Arrange 
        const string lineText = "2344,abc,1002,";

        // Act
        var result = _testee.GetMeterReadingFromLine(lineText);

        // Assert
        Assert.False(result.Successful);
    }

    [Fact]
    public void GivenInvalidValue_WheRun_ThenReturnNull()
    {
        // Arrange 
        const string lineText = "2344,22/04/2019 09:24,abc,";

        // Act
        var result = _testee.GetMeterReadingFromLine(lineText);

        // Assert
        Assert.False(result.Successful);
    }

    [Theory]
    [InlineData("-9", -9)]
    [InlineData("-99999", -99999)]
    [InlineData("99999", 99999)]
    public void GivenValidReadingValueText_WheRun_ThenReturnExpectedResult(string readingValueText, decimal expectedValue)
    {
        // Arrange 
        var lineText = $"2344,22/04/2019 09:24,{readingValueText},";

        // Act
        var result = _testee.GetMeterReadingFromLine(lineText);

        // Assert
        Assert.True(result.Successful);
        Assert.NotNull(result.MeterReadingEntity);
        Assert.Equal(expectedValue, result.MeterReadingEntity.ReadingValue);
    }
    
    
    [Theory]
    [InlineData("99999.0")]
    [InlineData("100000.0")]
    [InlineData("-99999.0")]
    [InlineData("-99999.01")]
    [InlineData("-100000")]
    public void GivenInvalidReadingValueText_WheRun_ThenReturnExpectedResult(string readingValueText)
    {
        // Arrange 
        var lineText = $"2344,22/04/2019 09:24,{readingValueText},";

        // Act
        var result = _testee.GetMeterReadingFromLine(lineText);

        // Assert
        Assert.False(result.Successful);
    }
}
