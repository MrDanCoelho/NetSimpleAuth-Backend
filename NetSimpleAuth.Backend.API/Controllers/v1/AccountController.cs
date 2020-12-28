using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetPOC.Backend.Domain.Dto;
using NetPOC.Backend.Domain.Interfaces.IServices;

namespace NetSimpleAuth.Backend.API.Controllers.v1
{
    /// <summary>
    /// 
    /// </summary>
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _accountService;

        public AccountController(
            ILogger<AccountController> logger,
            IAccountService accountService
            )
        {
            _logger = logger;
            _accountService = accountService;
        }
        
        // POST: api/v1/account/login
        [HttpPost("login")]
        public async Task<ActionResult<AuthUserResponse>> Login([Required] AuthUserDto user)
        {
            try
            {
                _logger.LogInformation($"Begin - {nameof(Login)}");

                var authUser = await _accountService.Authenticate(user);

                _logger.LogInformation($"End - {nameof(Login)}");

                return Ok(authUser);
            }
            catch (AuthenticationException e)
            {
                _logger.LogError($"{nameof(Login)}: {e}");
                
                return Unauthorized(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(Login)}: {e}");
                
                return BadRequest("Unable to authenticate user");
            }
        }
    }
}