using System.Threading.Tasks;
using NetSimpleAuth.Backend.Domain.Dto;
using NetSimpleAuth.Backend.Domain.Response;

namespace NetSimpleAuth.Backend.Domain.Interfaces.IServices
{
    /// <summary>
    /// Service with account methods
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Authenticates user
        /// </summary>
        /// <param name="authUserDto">DTO with user information</param>
        /// <returns>Authenticated user</returns>
        Task<AuthUserResponse> Authenticate(AuthUserDto authUserDto);
    }
}