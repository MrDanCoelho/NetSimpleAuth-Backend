using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NetSimpleAuth.Backend.Domain.Interfaces.IRepositories;
using NetSimpleAuth.Backend.Domain.Interfaces.IServices;

namespace NetSimpleAuth.Backend.Application.Services
{
    /// <inheritdoc/>
    public abstract class CrudService<T> : ICrudService<T> where T : class
    {
        private readonly ILogger<CrudService<T>> _logger;
        private readonly ICrudRepository<T> _crudRepository;

        /// <summary>
        /// Generic CRUD service
        /// </summary>
        /// <param name="logger">The CRUD logger</param>
        /// <param name="crudRepository">Repository that implements <see cref="ICrudRepository{T}"/></param>
        protected CrudService(ILogger<CrudService<T>> logger, ICrudRepository<T> crudRepository)
        {
            _logger = logger;
            _crudRepository = crudRepository;
        }
        
        public async Task<IEnumerable<T>> GetAll()
        {
            try
            {
                var result = await _crudRepository.GetAll();
                
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Method} failed for type {Type}", nameof(GetAll), typeof(T));
                throw;
            }
        }
        
        public async Task<T> GetById(object id)
        {
            try
            {
                var result = await _crudRepository.GetById(id);
                
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Method} failed for type {Type} and {$Id}", nameof(GetById), typeof(T), id);
                throw;
            }
        }
        
        public async Task<IEnumerable<T>> Select(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var result = await _crudRepository.Select(predicate);
                
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Method} failed for type {Type} and expression = {@Predicate}", nameof(Select),
                    typeof(T), predicate);
                throw;
            }
        }
        
        public async Task Insert(T obj)
        {
            try
            {
                await _crudRepository.Insert(obj);
                _crudRepository.Save();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Method} failed for type {Type} with object = {@Obj}", nameof(Insert), typeof(T), obj);
                throw;
            }
        }
        
        public async Task InsertAll(IEnumerable<T> objList)
        {
            try
            {
                await _crudRepository.InsertAll(objList);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Method} failed for type {Type} and object list = {@ObjList}", nameof(InsertAll),
                    typeof(T), objList);
                throw;
            }
        }
        
        public async Task Update(T obj)
        {
            try
            {
                await _crudRepository.Update(obj);
                _crudRepository.Save();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Method} failed for type {Type} and object = {@Obj}", nameof(Update), typeof(T), obj);
                throw;
            }
        }
        
        public async Task Delete(object id)
        {
            try
            {
                var obj = await _crudRepository.GetById(id);
                await _crudRepository.Delete(obj);
                _crudRepository.Save();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Method} failed for type {Type} and id = {$Id}", nameof(Delete), typeof(T), id);
                throw;
            }
        }
    }
}