using Microsoft.Extensions.Logging;
using NetSimpleAuth.Backend.Domain.Entities;
using NetSimpleAuth.Backend.Domain.Interfaces.IRepositories;

namespace NetSimpleAuth.Backend.Infra.Repositories;

public class UserRepository(ILogger<UserRepository> logger,
        IUnitOfWork unitOfWork)
    : CrudRepository<UserEntity>(logger, unitOfWork), IUserRepository
{
    private readonly ILogger<UserRepository> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
}