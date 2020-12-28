using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper;
using Dommel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetPOC.Backend.Domain.Dto;
using NetPOC.Backend.Domain.Entities;
using NetPOC.Backend.Domain.Interfaces.IRepositories;

namespace NetPOC.Backend.Infra.Repositories
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
            try
            {
                _logger.LogInformation($"Begin - {nameof(SelectPaginated)}");
                var sql =
                    $@"SELECT *
	                FROM public.""Log""
	                WHERE ""Ip"" like '%{filter.Ip}%'
                    AND ""UserAgent"" like '%{filter.UserAgent}%'";
                
                if(filter.Hour != null)
                    sql += $@" AND extract(hour from ""Date"") = {filter.Hour}";

                if (filter.Order != null)
                    sql += $@" ORDER BY ""{filter.Order}"" {filter.Direction}";
                    
                sql += $@" OFFSET {(pageNumber -1) * pageSize} ROWS
	                FETCH NEXT {pageSize} ROWS ONLY";
                
                var sqlCount =
                    $@"SELECT count(*)
	                FROM public.""Log""
	                WHERE ""Ip"" like '%{filter.Ip}%'
                    AND ""UserAgent"" like '%{filter.UserAgent}%'";
                
                if(filter.Hour != null)
                    sqlCount += $@" AND extract(hour from ""Date"") = {filter.Hour}";


                var result = new SelectPaginatedResponse<LogEntity>
                {
                    obj = await _unitOfWork.DbConnection.QueryAsync<LogEntity>(sql),
                    count = (await _unitOfWork.DbConnection.QueryAsync<int>(sqlCount)).First()
                };

                _logger.LogInformation($"End - {nameof(SelectPaginated)}");

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(SelectPaginated)}: {e}");
                throw;
            }
        }
    }
}