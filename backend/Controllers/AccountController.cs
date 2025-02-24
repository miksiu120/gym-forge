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
        public IActionResult Login([FromBody] LoginUserDto loginUserDto)
        {

            string resultToken = _accountService.Login(loginUserDto);

            return Ok(resultToken);
        }

        [HttpPut("register")]
        public IActionResult Register([FromBody] CreateUserDto createUserDto)
        {
            _accountService.Register(createUserDto);
            return Ok();
        }



    }
}