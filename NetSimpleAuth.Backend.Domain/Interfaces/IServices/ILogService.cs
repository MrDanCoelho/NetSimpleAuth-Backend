using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NetPOC.Backend.Domain.Dto;
using NetPOC.Backend.Domain.Entities;

namespace NetPOC.Backend.Domain.Interfaces.IServices
{
    public interface ILogService : ICrudService<LogEntity>
    {
        Task InsertBatch(StreamReader file);
        
        /// <summary>
        /// Service to collect all objects paginated according to their indicated type and predicate
        /// </summary>
        /// <param name="predicate">Predicate to tell which objects to return</param>
        /// <param name="pageNumber">The current page number</param>
        /// <param name="pageSize">The page size</param>
        /// <returns>List of the objects found and paginated</returns>
        Task<SelectPaginatedResponse<LogEntity>> SelectPaginated(LogFilterDto filter, int pageNumber, int pageSize);
    }
}