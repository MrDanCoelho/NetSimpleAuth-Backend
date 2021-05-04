using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using NetSimpleAuth.Backend.Domain.Dto;
using NetSimpleAuth.Backend.Domain.Entities;
using NetSimpleAuth.Backend.Domain.Interfaces.IRepositories;
using NetSimpleAuth.Backend.Domain.Response;

namespace NetSimpleAuth.Backend.Infra.Repositories
{
    /// <inheritdoc cref="ILogRepository" />
    public class LogRepository : CrudRepository<LogEntity>, ILogRepository
    {
        private readonly ILogger<LogRepository> _logger;
        private readonly IUnitOfWork _unitOfWork;
        
        /// <summary>
        /// <see cref="LogEntity"/> repository
        /// </summary>
        /// <param name="logger"><see cref="ILogger{LogRepository}"/> logger</param>
        /// <param name="unitOfWork"><see cref="UnitOfWork"/> for the repository</param>
        public LogRepository(ILogger<LogRepository> logger, IUnitOfWork unitOfWork) 
            : base(logger, unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        
        public async Task<SelectPaginatedResponse<LogEntity>> SelectPaginated(LogFilterDto filter, int pageNumber, int pageSize)
        {
            var query = "";
            
            try
            {
                query =
                    $@"SELECT *
	                FROM public.""Log""
	                WHERE ""Ip"" like '%{filter.Ip}%'
                    AND ""UserAgent"" like '%{filter.UserAgent}%'";
                
                if(filter.Hour != null)
                    query += $@" AND extract(hour from ""Date"") = {filter.Hour}";

                if (filter.Order != null)
                    query += $@" ORDER BY ""{filter.Order}"" {filter.Direction}";
                    
                query += $@" OFFSET {(pageNumber -1) * pageSize} ROWS
	                FETCH NEXT {pageSize} ROWS ONLY";
                
                var countQuery =
                    $@"SELECT count(*)
	                FROM public.""Log""
	                WHERE ""Ip"" like '%{filter.Ip}%'
                    AND ""UserAgent"" like '%{filter.UserAgent}%'";
                
                if(filter.Hour != null)
                    countQuery += $@" AND extract(hour from ""Date"") = {filter.Hour}";


                var result = new SelectPaginatedResponse<LogEntity>
                {
                    Obj = await _unitOfWork.DbConnection.QueryAsync<LogEntity>(query),
                    Count = (await _unitOfWork.DbConnection.QueryAsync<int>(countQuery)).First()
                };

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Pagination failed for query = {$Query}", query);
                throw;
            }
        }
    }
}