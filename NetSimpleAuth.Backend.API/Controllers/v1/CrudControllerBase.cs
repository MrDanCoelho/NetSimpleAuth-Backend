using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetPOC.Backend.Domain.Interfaces.IServices;

namespace NetSimpleAuth.Backend.API.Controllers.v1
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Authorize]
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CrudControllerBase<T> : ControllerBase where T : class
    {
        private readonly ILogger<CrudControllerBase<T>> _logger;
        private readonly ICrudService<T> _crudService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="crudService"></param>
        public CrudControllerBase(ILogger<CrudControllerBase<T>> logger, ICrudService<T> crudService)
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
                _logger.LogInformation($"Begin - {nameof(GetAll)} ({nameof(T)})");

                var result = await _crudService.GetAll();
                
                _logger.LogInformation($"End - {nameof(GetAll)} ({nameof(T)})");

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(GetAll)} ({nameof(T)}): {e}");
                throw;
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
                _logger.LogInformation($"Begin - {nameof(GetById)} ({nameof(T)})");

                var result = await _crudService.GetById(id);
                
                _logger.LogInformation($"End - {nameof(GetById)} ({nameof(T)})");

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(GetById)} ({nameof(T)}): {e}");
                throw;
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
                _logger.LogInformation($"Begin - {nameof(Insert)} ({nameof(T)})");
                
                await _crudService.Insert(obj);
                
                _logger.LogInformation($"End - {nameof(Insert)} ({nameof(T)})");

                return Ok("Object inserted with success");
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(Insert)} ({nameof(T)}): {e}");
                throw;
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
                _logger.LogInformation($"Begin - {nameof(Update)} ({nameof(T)})");

                await _crudService.Update(obj);

                _logger.LogInformation($"End - {nameof(Update)} ({nameof(T)})");

                return Ok("Object updated with success");
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(Update)} ({nameof(T)}): {e}");
                throw;
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
                _logger.LogInformation($"Begin - {nameof(Delete)} ({nameof(T)})");
                
                await _crudService.Delete(id);
                
                _logger.LogInformation($"End - {nameof(Delete)} ({nameof(T)})");

                return Ok("Object deleted with success");
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(Delete)} ({nameof(T)}): {e}");
                throw;
            }
        }
    }
}