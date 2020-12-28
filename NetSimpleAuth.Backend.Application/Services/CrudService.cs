using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NetPOC.Backend.Domain.Entities;
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
        /// <param name="logger">Logger do tipo <see>
        ///         <cref>ILogger{CrudService{T}}</cref>
        ///     </see>
        /// </param>
        /// <param name="crudRepository">Repositório com as funções de CRUD que implemente <see cref="ICrudRepository{T}"/></param>
        protected CrudService(ILogger<CrudService<T>> logger, ICrudRepository<T> crudRepository)
        {
            _logger = logger;
            _crudRepository = crudRepository;
        }
        
        public async Task<IEnumerable<T>> GetAll()
        {
            try
            {
                _logger.LogInformation($"Begin - {nameof(CrudService<T>)}.{nameof(GetAll)} ");

                var result = await _crudRepository.GetAll();
                
                _logger.LogInformation($"End - {nameof(CrudService<T>)}.{nameof(GetAll)}");

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
                _logger.LogInformation($"Begin - {nameof(GetById)} ({nameof(T)})");
                
                var result = await _crudRepository.GetById(id);
                
                _logger.LogInformation($"End - {nameof(GetById)} ({nameof(T)})");

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(GetById)} ({nameof(T)}): {e}");
                throw;
            }
        }
        
        public async Task<T> SelectFirst(Expression<Func<T, bool>> predicate)
        {
            try
            {
                _logger.LogInformation($"Begin - {nameof(SelectFirst)} ({nameof(T)})");
                
                var result = await _crudRepository.SelectFirst(predicate);
                
                _logger.LogInformation($"End - {nameof(SelectFirst)} ({nameof(T)})");

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(SelectFirst)} ({nameof(T)}): {e}");
                throw;
            }
        }
        
        public async Task<IEnumerable<T>> SelectAll(Expression<Func<T, bool>> predicate)
        {
            try
            {
                _logger.LogInformation($"Begin - {nameof(SelectAll)} ({nameof(T)})");
                
                var result = await _crudRepository.SelectAll(predicate);
                
                _logger.LogInformation($"End - {nameof(SelectAll)} ({nameof(T)})");

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(SelectAll)} ({nameof(T)}): {e}");
                throw;
            }
        }
        
        public async Task Insert(T obj)
        {
            try
            {
                _logger.LogInformation($"Begin - {nameof(Insert)} ({nameof(T)})");
                
                await _crudRepository.Insert(obj);
                _crudRepository.Save();
                
                _logger.LogInformation($"End - {nameof(Insert)} ({nameof(T)})");
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(Insert)} ({nameof(T)}): {e}");
                throw;
            }
        }
        
        public async Task InsertAll(IEnumerable<T> objList)
        {
            try
            {
                _logger.LogInformation($"Begin - {nameof(Insert)} ({nameof(T)})");
                
                await _crudRepository.InsertAll(objList);
                
                _logger.LogInformation($"End - {nameof(Insert)} ({nameof(T)})");
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
                _logger.LogInformation($"Begin - {nameof(Update)} ({nameof(T)})");
                
                await _crudRepository.Update(obj);
                _crudRepository.Save();
                
                _logger.LogInformation($"End - {nameof(Update)} ({nameof(T)})");
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
                _logger.LogInformation($"Begin - {nameof(Delete)} ({nameof(T)})");

                var obj = await _crudRepository.GetById(id);
                await _crudRepository.Delete(obj);
                _crudRepository.Save();
                
                _logger.LogInformation($"End - {nameof(Delete)} ({nameof(T)})");
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(Delete)} ({nameof(T)}): {e}");
                throw;
            }
        }
    }
}