using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetSimpleAuth.Backend.API.CustomValidation;
using NetSimpleAuth.Backend.Domain.Dto;
using NetSimpleAuth.Backend.Domain.Entities;
using NetSimpleAuth.Backend.Domain.Interfaces.IServices;


namespace NetSimpleAuth.Backend.API.Controllers.v1;

/// <summary>
/// Controller for Log requests
/// </summary>
[Authorize]
[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class LogController : CrudControllerBase<LogEntity>
{
    private readonly ILogger<LogController> _logger;
    private readonly ILogService _logService;
        
    /// <summary>
    /// Controller for Log requests
    /// </summary>
    /// <param name="logger">The app's logger</param>
    /// <param name="logService">Service with Log methods</param>
    public LogController(ILogger<LogController> logger, ILogService logService)
        : base(logger, logService)
    {
        _logger = logger;
        _logService = logService;
    }

        
    /// <summary>
    /// Insert a batch of logs contained in a file
    /// </summary>
    /// <param name="logFile">A file containing a batch of logs</param>
    /// <returns>Result of the operation</returns>
    [HttpPost("batch")]
    public async Task<ActionResult<string>> InsertBatch(
        [Required(ErrorMessage = "Choose a file to be uploaded"), 
         AllowedExtensions(new[] { ".log", ".txt" }, ErrorMessage = "Wrong file format")]
        IFormFile logFile)
    {
        try
        {
            using (var file = new StreamReader(logFile.OpenReadStream()))
            {
                await _logService.InsertBatch(file);
            }

            return Ok("Batch inserted successfully");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Batch insertion failed");
                
            return BadRequest("Unable to read data. Check if file has the right format. If the problem persists, contact the administrator");
        }
    }
        
    /// <summary>
    /// Gets all objects in the database paginated
    /// </summary>
    /// <param name="filter">The filter to be used</param>
    /// <param name="page">The current page</param>
    /// <param name="pageSize">The page size</param>
    /// <returns>The list of objects paginated</returns>
    [HttpPost("{page}/{pageSize}")]
    public async Task<ActionResult<IEnumerable<LogEntity>>> SelectPaginated([FromBody]LogFilterDto filter, int page, int pageSize)
    {
        try
        {
            var result = await _logService.SelectPaginated(filter, page, pageSize);
                
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                "Log pagination failed for filters={@Filter}, page={$Page} and page size={$PageSize}", filter, page,
                pageSize);
            return BadRequest("Unable to get data. If problem persists, contact an administrator");
        }
    }
}