using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Data;
using WorkPlanner.Entities;
using WorkPlanner.Enums;
using WorkPlanner.Models;
using WorkPlanner.Repositories;

namespace WorkPlanner.Services
{
    public interface IAccountService
    {
        void Register(CreateUserDto createUserDto);
        string Login(LoginUserDto loginUserDto);
    }

    public class AccountService : IAccountService
    {

        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountService(IAccountRepository accountRepository,IMapper mapper,
            IPasswordHasher<User> passwordHasher)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;

        }

        public void Register(CreateUserDto createUserDto)
        {
            if (_accountRepository.GetAccountByEmailAsync(createUserDto.Email).Result is not null)
            {
                throw new ArgumentException("An account with this email already exists.");
            }
            else if (_accountRepository.GetAccountByNicknameAsync(createUserDto.Email).Result is not null)
            {
                throw new ArgumentException("An account with this email already exists.");
            }
            else if (_accountRepository.GetAccountByNicknameAsync(createUserDto.Nickname).Result is not null)
            {
                throw new ArgumentException("An account with this email already exists.");
            }

            if (createUserDto.Password != createUserDto.ConfirmPassword)
            {
                throw new ArgumentException("Passwords are not the same.");
            }

            

            User newUser = _mapper.Map<User>(createUserDto);

            if (Enum.TryParse(typeof(MeasurementSystem), createUserDto.MeasurementValue, true, out var result))
            {
                newUser.MeasurementSystem = (MeasurementSystem)result;
            }
            else
            {
                throw new ArgumentException("Invalid measurement system value.");
            }
            var passwordHash = _passwordHasher.HashPassword(newUser,createUserDto.Password);
            newUser.HashedPassword = passwordHash;

            _accountRepository.CreateAsync(newUser);

        }

        public string Login(LoginUserDto loginUserDto)
        {
            return String.Empty;
        }

    }

}
