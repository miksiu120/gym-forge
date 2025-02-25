using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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

    }

    public class AccountService : IAccountService
    {

        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;

        public AccountService(IAccountRepository accountRepository,IMapper mapper,
            IPasswordHasher<User> passwordHasher, IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        public Task Register(CreateUserDto createUserDto)
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
            return Task.CompletedTask;
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
                RefreshToken = await GenerateRefreshToken(user),
                Token = await GenerateToken(user)
            };

            return loginResultDto;
        }

        public async Task<string> GenerateToken(User user)
        {

            var secretKey = _configuration["Authentication:JwtKey"];
            var issuer = _configuration["Authentication:JwtIssuer"];
            var tokenExpireHours = int.Parse(_configuration["Authentication:JwtExpireAccount"]);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email,$"{user.Email}"),
                new Claim(ClaimTypes.Role,$"{user.Role}"),
                new Claim(ClaimTypes.Name, $"{user.Nickname}")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.Now.AddHours(tokenExpireHours);

            var token = new JwtSecurityToken(
                issuer: issuer,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> GenerateRefreshToken(User user)
        {
            var secretKey = _configuration["Authentication:JwtKey"];
            var issuer = _configuration["Authentication:JwtIssuer"];
            var refreshTokenExpireDays = int.Parse(_configuration["Authentication:JwtRefreshTokenAccount"]);

            var claims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: issuer,
                expires: DateTime.Now.AddDays(refreshTokenExpireDays),
                signingCredentials: creds,
                claims:claims
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }

}
