using System.Threading.Tasks;
using NetSimpleAuth.Backend.Domain.Dto;
using NetSimpleAuth.Backend.Domain.Entities;
using NetSimpleAuth.Backend.Domain.Response;

namespace NetSimpleAuth.Backend.Domain.Interfaces.IRepositories;

public interface ILogRepository : ICrudRepository<LogEntity>
{
    /// <summary>
    /// Repository service to search for a list of objects paginated according to an expression
    /// </summary>
    /// <param name="filter">The predicate of the objects to be searched</param>
    /// <param name="pageNumber">The page number</param>
    /// <param name="pageSize">The size of the pages</param>
    /// <returns></returns>
    Task<SelectPaginatedResponse<LogEntity>> SelectPaginated(LogFilterDto filter, int pageNumber, int pageSize);
}