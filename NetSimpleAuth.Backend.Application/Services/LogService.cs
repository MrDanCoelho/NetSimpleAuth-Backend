using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NetSimpleAuth.Backend.Domain.Dto;
using NetSimpleAuth.Backend.Domain.Entities;
using NetSimpleAuth.Backend.Domain.Interfaces.IRepositories;
using NetSimpleAuth.Backend.Domain.Interfaces.IServices;
using NetSimpleAuth.Backend.Domain.Response;

namespace NetSimpleAuth.Backend.Application.Services;

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
            var logList = new List<LogEntity>();
            while (await file.ReadLineAsync() is { } line)
            {
                _logger.LogDebug("Current line: {$Line}", line);
                    
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
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error during batch insertion");
            throw;
        }
    }
        
    public async Task<SelectPaginatedResponse<LogEntity>> SelectPaginated(LogFilterDto filter, int pageNumber, int pageSize)
    {
        try
        {
            var result = await _logRepository.SelectPaginated(filter, pageNumber, pageSize);
                
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                "Pagination failed for filter = {@Filter}, page number = {$PageNumber} and page size = {$PageSize}",
                filter, pageNumber, pageSize);
            throw;
        }
    }
}