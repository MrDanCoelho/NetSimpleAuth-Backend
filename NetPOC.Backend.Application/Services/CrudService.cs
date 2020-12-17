using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NetPOC.Backend.Domain.Interfaces.IRepositories;
using NetPOC.Backend.Domain.Interfaces.IServices;

namespace NetPOC.Backend.Application.Services
{
    /// <inheritdoc/>
    public abstract class CrudService<T> : ICrudService<T> where T : class
    {
        private readonly ILogger<CrudService<T>> _logger;
        private readonly ICrudRepository<T> _crudRepository;

        /// <summary>
        /// Serviço abstrato de operações CRUD
        /// </summary>
        /// <param name="logger">Logger do tipo <see cref="ILogger{CrudService{T}}"/></param>
        /// <param name="crudRepository">Repositório com as funções de CRUD que implemente <see cref="ICrudRepository{T}"/></param>
        public CrudService(ILogger<CrudService<T>> logger, ICrudRepository<T> crudRepository)
        {
            _logger = logger;
            _crudRepository = crudRepository;
        }
        
        public async Task<IEnumerable<T>> GetAll()
        {
            try
            {
                _logger.LogInformation($"Inicio - {nameof(CrudService<T>)}.{nameof(GetAll)} ");

                var result = await _crudRepository.GetAll();
                
                _logger.LogInformation($"Fim - {nameof(CrudService<T>)}.{nameof(GetAll)}");

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(CrudService<T>)}.{nameof(GetAll)}: {e}");
                throw;
            }
        }
        
        public async Task<T> GetById(object id)
        {
            try
            {
                _logger.LogInformation($"Inicio - {nameof(GetById)} ({nameof(T)})");
                
                var result = await _crudRepository.GetById(id);
                
                _logger.LogInformation($"Fim - {nameof(GetById)} ({nameof(T)})");

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(GetById)} ({nameof(T)}): {e}");
                throw;
            }
        }
        
        public async Task Insert(T obj)
        {
            try
            {
                _logger.LogInformation($"Inicio - {nameof(Insert)} ({nameof(T)})");
                
                await _crudRepository.Insert(obj);
                await _crudRepository.Save();
                
                _logger.LogInformation($"Fim - {nameof(Insert)} ({nameof(T)})");
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(Insert)} ({nameof(T)}): {e}");
                throw;
            }
        }
        
        public async Task Update(T obj)
        {
            try
            {
                _logger.LogInformation($"Inicio - {nameof(Update)} ({nameof(T)})");
                
                _crudRepository.Update(obj);
                await _crudRepository.Save();
                
                _logger.LogInformation($"Fim - {nameof(Update)} ({nameof(T)})");
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(Update)} ({nameof(T)}): {e}");
                throw;
            }
        }
        
        public async Task Delete(object id)
        {
            try
            {
                _logger.LogInformation($"Inicio - {nameof(Delete)} ({nameof(T)})");

                var obj = await _crudRepository.GetById(id);
                _crudRepository.Delete(obj);
                await _crudRepository.Save();
                
                _logger.LogInformation($"Fim - {nameof(Delete)} ({nameof(T)})");
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(Delete)} ({nameof(T)}): {e}");
                throw;
            }
        }
    }
}