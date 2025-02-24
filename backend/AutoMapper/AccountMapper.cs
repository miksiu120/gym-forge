using AutoMapper;
using WorkPlanner.Entities;
using WorkPlanner.Models;

namespace WorkPlanner.AutoMapper
{
    public class AccountMapper:Profile
    {

        public AccountMapper()
        {
            CreateMap<CreateUserDto, User>();

        }
    }
}
