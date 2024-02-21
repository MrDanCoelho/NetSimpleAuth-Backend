using Microsoft.Extensions.Logging;
using NetSimpleAuth.Backend.Domain.Entities;
using NetSimpleAuth.Backend.Domain.Interfaces.IRepositories;

namespace NetSimpleAuth.Backend.Infra.Repositories;

public class RefreshTokenRepository(ILogger<RefreshTokenRepository> logger,
        IUnitOfWork unitOfWork)
    : CrudRepository<RefreshTokenEntity>(logger, unitOfWork), IRefreshTokenRepository
{
    private readonly ILogger<RefreshTokenRepository> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
}