using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NetPOC.Backend.Domain.Dto;
using NetPOC.Backend.Domain.Entities;

namespace NetPOC.Backend.Domain.Interfaces.IRepositories
{
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
}