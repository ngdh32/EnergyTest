using System;

namespace EnsekMeterReadingCore.Entities;

public class AccountEntity
{
    public required int AccountId { get; set; }
    
    public required string LastName { get; set; }

    public required string FirstName { get; set; }
}
