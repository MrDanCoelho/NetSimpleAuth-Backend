using NetSimpleAuth.Backend.Domain.Entities;

namespace NetSimpleAuth.Backend.Domain.Interfaces.IRepositories
{
    /// <summary>
    /// Repository with methods for token refreshing
    /// </summary>
    public interface IRefreshTokenRepository : ICrudRepository<RefreshTokenEntity>
    {
        
    }
}