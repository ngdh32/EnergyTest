using System;
using System.Reflection.Metadata;
using EnsekMeterReadingCore.Entities;
using EnsekMeterReadingCore.Helpers;
using EnsekMeterReadingCore.Helpers.Implementations;
using EnsekMeterReadingCore.Repositories;
using Moq;

namespace EnsekMeterReadingApiTest;

public class MeterReadingUploadCsvParserTest
{   
    private readonly IMeterReadingUploadCsvParser _parser;
    private readonly Mock<IAccountRepository> _accountRepositoryMock = new(MockBehavior.Strict);
    private readonly Mock<IMeterReadingRepository> _meterReadingRepositoryMock = new(MockBehavior.Strict);
    
    private static int[] AccountIds => [2344, 2000];

    public MeterReadingUploadCsvParserTest()
    {
        _accountRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsIn(AccountIds))).ReturnsAsync(() => new AccountEntity()
        {
            Id = 2344,
        });
        _accountRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsNotIn(AccountIds))).ReturnsAsync(() => null);
        
        _meterReadingRepositoryMock.Setup(repo => repo.GetAccountLastReadingTime(It.Is<int>(x => x == 2344)))
            .Returns(() => new MeterReadingEntity()
            {
                Id = 1,
                AccountId = 2344,
                Remark = String.Empty,
                ReadingTime = new DateTime(2018, 1, 1),
                ReadingValue = 1002
            });

        _parser = new MeterReadingUploadCsvParser(_accountRepositoryMock.Object, _meterReadingRepositoryMock.Object);
    }

    [Fact]
    public async Task GivenValidLine_WheRun_ThenReturnEntity()
    {
        // Arrange 
        const string lineText = "2344,22/04/2019 09:24,1002,";

        // Act
        var result = await _parser.GetMeterReadingFromLine(lineText);

        // Assert
        Assert.True(result.Successful);
        Assert.NotNull(result.MeterReadingEntity);
        Assert.Equal(2344, result.MeterReadingEntity.AccountId);
        Assert.Equal(new DateTime(2019,4,22,9,24,00), result.MeterReadingEntity.ReadingTime);
        Assert.Equal(1002, result.MeterReadingEntity.ReadingValue);
        Assert.Equal("", result.MeterReadingEntity.Remark);
    }

    [Fact]
    public async Task GivenInvalidAccountId_WheRun_ThenReturnNull()
    {
        // Arrange 
        const string lineText = "abc,22/04/2019 09:24,1002,";

        // Act
        var result = await _parser.GetMeterReadingFromLine(lineText);

        // Assert
        Assert.False(result.Successful);
    }
    
    [Fact]
    public async Task GivenValidNotFoundAccountId_WheRun_ThenReturnNull()
    {
        // Arrange 
        const string lineText = "999,22/04/2019 09:24,1002,";

        // Act
        var result = await _parser.GetMeterReadingFromLine(lineText);

        // Assert
        Assert.False(result.Successful);
    }

    [Fact]
    public async Task GivenInvalidRecordDate_WheRun_ThenReturnNull()
    {
        // Arrange 
        const string lineText = "2344,abc,1002,";

        // Act
        var result = await _parser.GetMeterReadingFromLine(lineText);

        // Assert
        Assert.False(result.Successful);
    }

    [Fact]
    public async Task GivenInvalidValue_WheRun_ThenReturnNull()
    {
        // Arrange 
        const string lineText = "2344,22/04/2019 09:24,abc,";

        // Act
        var result = await _parser.GetMeterReadingFromLine(lineText);

        // Assert
        Assert.False(result.Successful);
    }

    [Theory]
    [InlineData("-9", -9)]
    [InlineData("-99999", -99999)]
    [InlineData("99999", 99999)]
    public async Task GivenValidReadingValueText_WheRun_ThenReturnExpectedResult(string readingValueText, decimal expectedValue)
    {
        // Arrange 
        var lineText = $"2344,22/04/2019 09:24,{readingValueText},";

        // Act
        var result = await _parser.GetMeterReadingFromLine(lineText);

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
    public async Task GivenInvalidReadingValueText_WheRun_ThenReturnExpectedResult(string readingValueText)
    {
        // Arrange 
        var lineText = $"2344,22/04/2019 09:24,{readingValueText},";

        // Act
        var result = await _parser.GetMeterReadingFromLine(lineText);

        // Assert
        Assert.False(result.Successful);
    }
    
    [Fact]
    public async Task GivenNoLastReadingTime_WheRun_ThenReturnEntity()
    {
        // Arrange 
        const string lineText = "2000,22/04/2019 09:24,1002,";

        // Act
        var result = await _parser.GetMeterReadingFromLine(lineText);

        // Assert
        Assert.True(result.Successful);
        Assert.NotNull(result.MeterReadingEntity);
        Assert.Equal(2000, result.MeterReadingEntity.AccountId);
        Assert.Equal(new DateTime(2019,4,22,9,24,00), result.MeterReadingEntity.ReadingTime);
        Assert.Equal(1002, result.MeterReadingEntity.ReadingValue);
        Assert.Equal("", result.MeterReadingEntity.Remark);
    }
    
    
    [Fact]
    public async Task GivenLastReadingTimeIsLater_WheRun_ThenReturnEntity()
    {
        // Arrange 
        const string lineText = "2344,22/04/2017 09:24,1002,";


        // Act
        var result = await _parser.GetMeterReadingFromLine(lineText);

        // Assert
        Assert.False(result.Successful);
        Assert.Null(result.MeterReadingEntity);
    }
    
    [Fact]
    public async Task GivenDuplicateLastReadingTimeIsLater_WheRun_ThenReturnEntity()
    {
        // Arrange 
        const string lineText = "2344,01/01/2018 00:00,1002,";

        // Act
        var result = await _parser.GetMeterReadingFromLine(lineText);

        // Assert
        Assert.False(result.Successful);
        Assert.Null(result.MeterReadingEntity);
    }
}
