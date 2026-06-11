using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PartyGame.Services;
using WorkPlanner.Entities;
using WorkPlanner.Enums;
using WorkPlanner.Models;
using WorkPlanner.Repositories;

namespace WorkPlanner.Services
{
    public interface IAccountService
    {
       Task Register(CreateUserDto createUserDto);
       Task<LoginResultDto> Login(LoginUserDto loginUserDto);
       Task<AccountDetailsDto> UpdateAccountAsync(UpdateAccountDto request);

       AccountDetailsDto GetAccountDetails();

    }

    public class AccountService : IAccountService
    {

        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessorService _httpContextAccessorService;

        public AccountService(IAccountRepository accountRepository,IMapper mapper,
            IPasswordHasher<User> passwordHasher, IConfiguration configuration, ITokenService tokenService,
            IHttpContextAccessorService httpContextAccessorService)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _httpContextAccessorService = httpContextAccessorService;
        }

        public async Task Register(CreateUserDto createUserDto)
        {
            if (( _accountRepository.GetAccountByEmailAsync(createUserDto.Email).Result) is not null)
            {
                throw new ArgumentException("An account with this email already exists.");
            }
            else if (( _accountRepository.GetAccountByNicknameAsync(createUserDto.Email).Result) is not null)
            {
                throw new ArgumentException("An account with this email already exists.");
            }
            else if (( _accountRepository.GetAccountByNicknameAsync(createUserDto.Nickname).Result) is not null)
            {
                throw new ArgumentException("An account with this email already exists.");
            }

            if (createUserDto.Password != createUserDto.ConfirmPassword)
            {
                throw new ArgumentException("Passwords are not the same.");
            }

            User newUser = _mapper.Map<User>(createUserDto);

           
            var passwordHash = _passwordHasher.HashPassword(newUser,createUserDto.Password);
            newUser.HashedPassword = passwordHash;
            newUser.CreatedAt = DateTime.UtcNow;

            await _accountRepository.CreateAsync(newUser);
            
        }

        public async Task<LoginResultDto> Login(LoginUserDto loginUserDto)
        {

            var user = await  _accountRepository.GetAccountByNicknameAsync(loginUserDto.Nickname);

            if (user is null)
            {
                throw new Exception("Invalid nickname or password");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.HashedPassword, loginUserDto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new Exception("Invalid nickname or password");
            }

            LoginResultDto loginResultDto = new LoginResultDto()
            {
                RefreshToken =  _tokenService.GenerateRefreshToken(user),
                Token =  _tokenService.GenerateToken(user),
                Nickname = user.Nickname
            };

            return loginResultDto;
        }

        public  AccountDetailsDto GetAccountDetails()
        {
            var userId = _httpContextAccessorService.GetUserIdFromToken();

            if (userId is null)
            {
                throw new Exception("ID does not exist in token");
            }

            User user =  _accountRepository.GetAsync((int)userId).Result;


            AccountDetailsDto accountDetailsDto = _mapper.Map<AccountDetailsDto>(user);

            return accountDetailsDto;
        }

        public async Task<AccountDetailsDto> UpdateAccountAsync(UpdateAccountDto request)
        {
            var userId = _httpContextAccessorService.GetUserIdFromToken()
                ?? throw new UnauthorizedAccessException("ID does not exist in token");
            var user = await _accountRepository.GetAsync(userId)
                ?? throw new KeyNotFoundException("Account was not found.");

            var normalizedEmail = request.Email.Trim();
            var existingEmailUser = await _accountRepository.GetAccountByEmailAsync(normalizedEmail);
            if (existingEmailUser is not null && existingEmailUser.Id != user.Id)
                throw new ArgumentException("An account with this email already exists.");

            user.Email = normalizedEmail;
            user.Name = string.IsNullOrWhiteSpace(request.Name) ? null : request.Name.Trim();
            user.Surname = string.IsNullOrWhiteSpace(request.Surname) ? null : request.Surname.Trim();
            user.Weight = request.Weight;
            user.Height = request.Height;
            user.BirthDay = request.BirthDay.HasValue
                ? DateTime.SpecifyKind(request.BirthDay.Value, DateTimeKind.Utc)
                : null;
            user.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
            user.MeasurementSystem = request.MeasurementSystem;

            await _accountRepository.UpdateAsync(user);
            return _mapper.Map<AccountDetailsDto>(user);
        }

    }

}
