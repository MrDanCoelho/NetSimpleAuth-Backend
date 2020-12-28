using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NetPOC.Backend.Domain.Dto;
using NetPOC.Backend.Domain.Entities;
using NetPOC.Backend.Domain.Interfaces.IRepositories;
using NetPOC.Backend.Domain.Interfaces.IServices;

namespace NetPOC.Backend.Application.Services
{
    /// <inheritdoc cref="ILogService" />
    public class LogService : CrudService<LogEntity>, ILogService
    {
        private readonly ILogger<LogService> _logger;
        private readonly ILogRepository _logRepository;
        
        public LogService(ILogger<LogService> logger, ILogRepository logRepository)
            : base(logger, logRepository)
        {
            _logger = logger;
            _logRepository = logRepository;
        }

        public async Task InsertBatch(StreamReader file)
        {
            try
            {
                _logger.LogInformation($"Begin - {nameof(InsertBatch)}");

                string line;

                var logList = new List<LogEntity>();
                while ((line = await file.ReadLineAsync()) != null)
                {
                    _logger.LogInformation(line);
                    var values = line.Split(null).Select(a => a == "-"? null : a).ToArray();
                    
                    Enum.TryParse(values[8], out HttpStatusCode parsedStatusCode);
                    int.TryParse(values[9], out var parsedContentSize);

                    var logModel = new LogEntity
                    {
                        Ip = values[0],
                        App = values[1],
                        User = values[2],
                        Date = DateTime.ParseExact(values[3].TrimStart('[') + values[4].TrimEnd(']'), "dd/MMM/yyyy:HH:mm:sszzz", null),
                        RequestType = values[5].TrimStart('"'),
                        RequestUrl =  values[6],
                        RequestProtocol = values[7].TrimEnd('"'),
                        StatusCode = parsedStatusCode,
                        ContentSize = parsedContentSize,
                    };

                    if (values.Length > 10)
                    {
                        logModel.ResponseUrl = values[10];

                        logModel.UserAgent = values[11].TrimStart('"');
                        
                        var index = 11;
                        while (index ++ < values.Length - 1)
                        {
                            logModel.UserAgent += " ";
                            logModel.UserAgent += values[index];
                        }
                        
                        logModel.UserAgent = logModel.UserAgent.TrimEnd('"');
                    }

                    logList.Add(logModel);
                }
                
                await _logRepository.InsertAll(logList);
                _logRepository.Save();

                _logger.LogInformation($"End - {nameof(InsertBatch)}");
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(InsertBatch)}: {e}");
                throw;
            }
        }
        
        public async Task<SelectPaginatedResponse<LogEntity>> SelectPaginated(LogFilterDto filter, int pageNumber, int pageSize)
        {
            try
            {
                _logger.LogInformation($"Begin - {nameof(SelectPaginated)}");
                
                var result = await _logRepository.SelectPaginated(filter, pageNumber, pageSize);
                
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