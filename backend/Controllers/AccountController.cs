using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkPlanner.Application.Accounts;
using WorkPlanner.Models;

namespace WorkPlanner.Controllers;

[ApiController]
[Route("api/accounts")]
public sealed class AccountController(
    RegisterAccount registerAccount,
    LoginAccount loginAccount,
    GetAccountDetails getAccountDetails,
    UpdateAccount updateAccount) : ControllerBase
{
    [AllowAnonymous]
    [HttpPut("login")]
    public async Task<ActionResult<LoginResultDto>> Login(
        [FromBody] LoginUserDto request,
        CancellationToken cancellationToken) =>
        Ok(await loginAccount.ExecuteAsync(request, cancellationToken));

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] CreateUserDto request,
        CancellationToken cancellationToken)
    {
        await registerAccount.ExecuteAsync(request, cancellationToken);
        return Ok();
    }

    [Authorize]
    [HttpGet("details")]
    public async Task<ActionResult<AccountDetailsDto>> Get(
        CancellationToken cancellationToken) =>
        Ok(await getAccountDetails.ExecuteAsync(cancellationToken));

    [Authorize]
    [HttpPut("details")]
    public async Task<ActionResult<AccountDetailsDto>> Update(
        [FromBody] UpdateAccountDto request,
        CancellationToken cancellationToken) =>
        Ok(await updateAccount.ExecuteAsync(request, cancellationToken));
}
