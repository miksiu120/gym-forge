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

    }

}
