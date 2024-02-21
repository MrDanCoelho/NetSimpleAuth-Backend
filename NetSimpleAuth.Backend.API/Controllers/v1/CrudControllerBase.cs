using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetSimpleAuth.Backend.Domain.Interfaces.IServices;


namespace NetSimpleAuth.Backend.API.Controllers.v1;

/// <summary>
/// Base abstract controller with CRUD operations
/// </summary>
/// <typeparam name="T">The type that this CRUD controller will use</typeparam>
[ApiController]
public abstract class CrudControllerBase<T> : ControllerBase where T : class
{
    private readonly ILogger<CrudControllerBase<T>> _logger;
    private readonly ICrudService<T> _crudService;

    /// <summary>
    /// Base abstract controller with CRUD operations
    /// </summary>
    /// <param name="logger">The app's logger</param>
    /// <param name="crudService">Service with CRUD operations</param>
    protected CrudControllerBase(ILogger<CrudControllerBase<T>> logger, ICrudService<T> crudService)
    {
        _logger = logger;
        _crudService = crudService;
    }
        
    /// <summary>
    /// Gets all objects in the database
    /// </summary>
    /// <returns>The list of objects</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<T>>> GetAll()
    {
        try
        {
            var result = await _crudService.GetAll();
                
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Getting all {T} request failed", typeof(T));
            return BadRequest("Unable to get object list. If problem persists, contact an administrator");
        }
    }
        
    /// <summary>
    /// Finds the object according to it's ID
    /// </summary>
    /// <param name="id">ID of the object to be searched</param>
    /// <returns>Found object</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<T>> GetById(int id)
    {
        try
        {
            var result = await _crudService.GetById(id);
                
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Getting {T} with id {$Id} request failed", typeof(T),id);
            return BadRequest("Unable to get object by id. If problem persists, contact an administrator");
        }
    }

    /// <summary>
    /// Inserts an object
    /// </summary>
    /// <param name="obj">Object to be inserted</param>
    /// <returns><see cref="ActionResult"/> of the operation</returns>
    [HttpPost]
    public async Task<ActionResult> Insert([FromBody] T obj)
    {
        try
        {
            await _crudService.Insert(obj);
                
            return Ok("Object inserted with success");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed inserting object of type {T}: {@Obj}", typeof(T), obj);
            return BadRequest("Unable to insert object. If problem persists, contact an administrator");
        }
    }
        
    /// <summary>
    /// Updates an object
    /// </summary>
    /// <param name="obj">Object to be updated</param>
    /// <returns><see cref="ActionResult"/> of the operation</returns>
    [HttpPut]
    public async Task<ActionResult> Update([FromBody] T obj)
    {
        try
        {
            await _crudService.Update(obj);

            return Ok("Object updated with success");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed updating object of type {T}: {@Obj}", typeof(T), obj);
            return BadRequest("Unable to update object. If problem persists, contact an administrator");
        }
    }
        
    /// <summary>
    /// Deletes an object according to it's ID
    /// </summary>
    /// <param name="id">ID of the object to be deleted</param>
    /// <returns><see cref="ActionResult"/> of the operation</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _crudService.Delete(id);
                
            return Ok("Object deleted with success");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed deleting object of type {T} and id = {$Id}", typeof(T), id);
            return BadRequest("Unable to delete object. If problem persists, contact an administrator");
        }
    }
}