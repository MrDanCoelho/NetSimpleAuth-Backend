using System.Threading.Tasks;
using NetPOC.Backend.Domain.Dto;

namespace NetPOC.Backend.Domain.Interfaces.IServices
{
    public interface IAccountService
    {
        Task<AuthUserResponse> Authenticate(AuthUserDto authUserDto);
    }
}