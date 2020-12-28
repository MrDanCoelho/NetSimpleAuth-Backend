using Microsoft.Extensions.Logging;
using NetPOC.Backend.Domain.Entities;
using NetPOC.Backend.Domain.Interfaces.IRepositories;

namespace NetPOC.Backend.Infra.Repositories
{
    public class UserRepository : CrudRepository<UserEntity>, IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;
        private readonly IUnitOfWork _unitOfWork;
        
        public UserRepository(
            ILogger<UserRepository> logger, 
            IUnitOfWork unitOfWork
            ) : base(logger, unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
    }
}