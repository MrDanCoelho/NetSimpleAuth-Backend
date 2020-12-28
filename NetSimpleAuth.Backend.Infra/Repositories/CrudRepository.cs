using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dommel;
using Microsoft.Extensions.Logging;
using NetPOC.Backend.Domain.Interfaces.IRepositories;

namespace NetPOC.Backend.Infra.Repositories
{
    /// <inheritdoc/>
    public abstract class CrudRepository<T> : ICrudRepository<T> where T : class
    {
        private readonly ILogger<CrudRepository<T>> _logger;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Generic CRUD repository
        /// </summary>
        /// <param name="logger"><see>
        ///         <cref>ILogger{CrudRepository{T}}</cref>
        ///     </see>
        ///     logger</param>
        /// <param name="unitOfWork">Repository's Unit of Work</param>
        protected CrudRepository(ILogger<CrudRepository<T>> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        
        public async Task<IEnumerable<T>> GetAll()
        {
            try
            {
                _logger.LogInformation($"Begin - {nameof(GetAll)} ({nameof(T)})");
                
                var result = await _unitOfWork.DbConnection.GetAllAsync<T>();

                _logger.LogInformation($"End - {nameof(GetAll)} ({nameof(T)})");

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(GetAll)} ({nameof(T)}): {e}");
                throw;
            }
        }
        
        public async Task<T> GetById(object id)
        {
            try
            {
                _logger.LogInformation($"Begin - {nameof(GetById)} ({nameof(T)})");
                
                var result = await _unitOfWork.DbConnection.GetAsync<T>(id);
                
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
                
                var result = await _unitOfWork.DbConnection.SelectAsync(predicate);
                
                _logger.LogInformation($"End - {nameof(SelectFirst)} ({nameof(T)})");

                return result.FirstOrDefault();
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
                
                var result = await _unitOfWork.DbConnection.SelectAsync(predicate);
                
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
                
                _unitOfWork.Begin();
                await _unitOfWork.DbConnection.InsertAsync(obj, _unitOfWork.DbTransaction);
                
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
                
                _unitOfWork.Begin();
                await _unitOfWork.DbConnection.InsertAllAsync(objList, _unitOfWork.DbTransaction);
                
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
                
                _unitOfWork.Begin();
                await _unitOfWork.DbConnection.UpdateAsync(obj, _unitOfWork.DbTransaction);
                
                _logger.LogInformation($"End - {nameof(Update)} ({nameof(T)})");
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(Update)} ({nameof(T)}): {e}");
                throw;
            }
        }

        public async Task Delete(T obj)
        {
            try
            {
                _logger.LogInformation($"Begin - {nameof(Delete)} ({nameof(T)})");
                
                _unitOfWork.Begin();
                await _unitOfWork.DbConnection.DeleteAsync(obj, _unitOfWork.DbTransaction);
                
                _logger.LogInformation($"End - {nameof(Delete)} ({nameof(T)})");
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(Delete)} ({nameof(T)}): {e}");
                throw;
            }
        }

        public void Save()
        {
            try
            {
                _logger.LogInformation($"Begin - {nameof(Save)} ({nameof(T)})");
                
                _unitOfWork.Commit();
                
                _logger.LogInformation($"End - {nameof(Save)} ({nameof(T)})");
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(Save)} ({nameof(T)}): {e}");
                throw;
            }
        }
    }
}