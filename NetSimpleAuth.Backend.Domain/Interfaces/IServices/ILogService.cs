using System.IO;
using System.Threading.Tasks;
using NetSimpleAuth.Backend.Domain.Dto;
using NetSimpleAuth.Backend.Domain.Entities;
using NetSimpleAuth.Backend.Domain.Response;

namespace NetSimpleAuth.Backend.Domain.Interfaces.IServices
{
    public interface ILogService : ICrudService<LogEntity>
    {
        /// <summary>
        /// Service to insert a batch of logs from a file
        /// </summary>
        /// <param name="file">The file with the logs to be inserted</param>
        Task InsertBatch(StreamReader file);
        
        /// <summary>
        /// Service to collect all objects paginated according to their indicated type and predicate
        /// </summary>
        /// <param name="filter">The filter to be applied</param>
        /// <param name="pageNumber">The current page number</param>
        /// <param name="pageSize">The page size</param>
        /// <returns>List of the objects found and paginated</returns>
        Task<SelectPaginatedResponse<LogEntity>> SelectPaginated(LogFilterDto filter, int pageNumber, int pageSize);
    }
}