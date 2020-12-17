using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetPOC.Backend.Domain.Interfaces.IServices;

namespace NetPOC.Backend.API.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CrudControllerBase<T> : ControllerBase where T : class
    {
        private readonly ILogger<CrudControllerBase<T>> _logger;
        private readonly ICrudService<T> _crudService;

        public CrudControllerBase(ILogger<CrudControllerBase<T>> logger, ICrudService<T> crudService)
        {
            _logger = logger;
            _crudService = crudService;
        }
        
        /// <summary>
        /// Busca todos os objetos
        /// </summary>
        /// <returns>Lista de objetos/></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<T>>> GetAll()
        {
            try
            {
                _logger.LogInformation($"Inicio - {nameof(GetAll)} ({nameof(T)})");

                var result = await _crudService.GetAll();
                
                _logger.LogInformation($"Fim - {nameof(GetAll)} ({nameof(T)})");

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(GetAll)} ({nameof(T)}): {e}");
                throw;
            }
        }
        
        /// <summary>
        /// Busca um objeto de acordo com o ID fornecido
        /// </summary>
        /// <param name="id">ID do objeto a ser buscado</param>
        /// <returns>Objeto achado</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<T>> GetById(int id)
        {
            try
            {
                _logger.LogInformation($"Inicio - {nameof(GetById)} ({nameof(T)})");

                var result = await _crudService.GetById(id);
                
                _logger.LogInformation($"Fim - {nameof(GetById)} ({nameof(T)})");

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(GetById)} ({nameof(T)}): {e}");
                throw;
            }
        }
        
        /// <summary>
        /// Insere um objeto
        /// </summary>
        /// <param name="obj">Objeto a ser inserido</param>
        /// <returns><see cref="ActionResult"/> da operação</returns>
        [HttpPost]
        public async Task<ActionResult> Insert([FromBody] T obj)
        {
            try
            {
                _logger.LogInformation($"Inicio - {nameof(Insert)} ({nameof(T)})");
                
                await _crudService.Insert(obj);
                
                _logger.LogInformation($"Fim - {nameof(Insert)} ({nameof(T)})");

                return Ok("Objeto inserido com sucesso");
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(Insert)} ({nameof(T)}): {e}");
                throw;
            }
        }
        
        /// <summary>
        /// Atualiza objeto
        /// </summary>
        /// <param name="obj">Objeto a ser atualizado</param>
        /// <returns><see cref="ActionResult"/> da operação</returns>
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] T obj)
        {
            try
            {
                _logger.LogInformation($"Inicio - {nameof(Update)} ({nameof(T)})");

                await _crudService.Update(obj);

                _logger.LogInformation($"Fim - {nameof(Update)} ({nameof(T)})");

                return Ok("Objeto atualizado com sucesso");
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(Update)} ({nameof(T)}): {e}");
                throw;
            }
        }
        
        /// <summary>
        /// Apaga objeto de acordo com o ID fornecido
        /// </summary>
        /// <param name="id">ID do objeto a ser apagado</param>
        /// <returns><see cref="ActionResult"/> da operação</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation($"Inicio - {nameof(Delete)} ({nameof(T)})");
                
                await _crudService.Delete(id);
                
                _logger.LogInformation($"Fim - {nameof(Delete)} ({nameof(T)})");

                return Ok("Objeto apagado com sucesso");
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(Delete)} ({nameof(T)}): {e}");
                throw;
            }
        }
    }
}