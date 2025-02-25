using Microsoft.AspNetCore.Mvc;
using WorkPlanner.Models;
using WorkPlanner.Services;

namespace WorkPlanner.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountController : ControllerBase
    {

        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _accountService;

        public AccountController(ILogger<AccountController> logger, IAccountService service)
        {
            _logger = logger;
            _accountService = service;
        }


        [HttpPut("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
        {

            LoginResultDto loginResultDto= await _accountService.Login(loginUserDto);

            return Ok(loginResultDto);
        }

        [HttpPut("register")]
        public IActionResult Register([FromBody] CreateUserDto createUserDto)
        {
            _accountService.Register(createUserDto);
            return Ok();
        }



    }
}