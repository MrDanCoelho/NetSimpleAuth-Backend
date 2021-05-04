using Microsoft.Extensions.Logging;
using NetSimpleAuth.Backend.Domain.Entities;
using NetSimpleAuth.Backend.Domain.Interfaces.IRepositories;

namespace NetSimpleAuth.Backend.Infra.Repositories
{
    public class RefreshTokenRepository : CrudRepository<RefreshTokenEntity>, IRefreshTokenRepository
    {
        private readonly ILogger<RefreshTokenRepository> _logger;
        private readonly IUnitOfWork _unitOfWork;
        
        public RefreshTokenRepository(
            ILogger<RefreshTokenRepository> logger, 
            IUnitOfWork unitOfWork
        ) : base(logger, unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
    }
}