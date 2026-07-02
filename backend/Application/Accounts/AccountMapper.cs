using WorkPlanner.Entities;
using WorkPlanner.Models;

namespace WorkPlanner.Application.Accounts;

public static class AccountMapper
{
    public static AccountDetailsDto ToDetails(User user) => new()
    {
        Nickname = user.Nickname,
        Name = user.Name,
        Surname = user.Surname,
        Email = user.Email,
        Weight = user.Weight,
        Height = user.Height,
        Role = user.Role,
        CreatedAt = user.CreatedAt ?? DateTime.MinValue,
        BirthDay = user.BirthDay,
        Description = user.Description,
        MeasurementSystem = user.MeasurementSystem
    };
}
