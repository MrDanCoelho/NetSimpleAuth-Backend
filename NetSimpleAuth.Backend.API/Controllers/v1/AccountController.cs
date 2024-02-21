using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetSimpleAuth.Backend.Domain.Dto;
using NetSimpleAuth.Backend.Domain.Interfaces.IServices;
using NetSimpleAuth.Backend.Domain.Response;


namespace NetSimpleAuth.Backend.API.Controllers.v1;

/// <summary>
/// Controller for Account operations
/// </summary>
[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly IAccountService _accountService;

    /// <summary>
    /// Controller for Account operations
    /// </summary>
    /// <param name="logger">The app's logger</param>
    /// <param name="accountService">Service with account operations</param>
    public AccountController(
        ILogger<AccountController> logger,
        IAccountService accountService
    )
    {
        _logger = logger;
        _accountService = accountService;
    }
        
    /// <summary>
    /// Authenticates user in the service
    /// </summary>
    /// <param name="user">The user to authenticate</param>
    /// <returns>The authenticated user</returns>
    // POST: api/v1/account/login
    [HttpPost("login")]
    public async Task<ActionResult<AuthUserResponse>> Login([Required] AuthUserDto user)
    {
        try
        {
            var authUser = await _accountService.Authenticate(user);
                    
            return Ok(authUser);
        }
        catch (AuthenticationException e)
        {
            _logger.LogError(e, "Wrong username and/or password");
                
            return Unauthorized(e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to authenticate due to an unknown error");
                
            return BadRequest("Unable to authenticate user");
        }
    }
}