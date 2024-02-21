using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dommel;
using Microsoft.Extensions.Logging;
using NetSimpleAuth.Backend.Domain.Interfaces.IRepositories;

namespace NetSimpleAuth.Backend.Infra.Repositories;

/// <inheritdoc/>
public abstract class CrudRepository<T> : ICrudRepository<T> where T : class
{
    private readonly ILogger<CrudRepository<T>> _logger;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Generic CRUD repository
    /// </summary>
    /// <param name="logger">The logger for the repository</param>
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
            var result = await _unitOfWork.DbConnection.GetAllAsync<T>();

            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Method} failed for type {T}", nameof(GetAll), typeof(T));
            throw;
        }
    }
        
    public async Task<T> GetById(object id)
    {
        try
        {
            var result = await _unitOfWork.DbConnection.GetAsync<T>(id);
                
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Method} failed for type {T}", nameof(GetById), typeof(T));
            throw;
        }
    }
        
    public async Task<IEnumerable<T>> Select(Expression<Func<T, bool>> predicate)
    {
        try
        {
            var result = await _unitOfWork.DbConnection.SelectAsync(predicate);
                
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Method} failed for type {T} and predicate = {@Predicate}", nameof(Select), typeof(T), predicate);
            throw;
        }
    }
        
    public async Task Insert(T obj)
    {
        try
        {
            _unitOfWork.Begin();
            await _unitOfWork.DbConnection.InsertAsync(obj, _unitOfWork.DbTransaction);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Method} failed for type {T} and object = {@Obj}", nameof(Insert), typeof(T), obj);
            throw;
        }
    }
        
    public async Task InsertAll(IEnumerable<T> objList)
    {
        try
        {
            _unitOfWork.Begin();
            await _unitOfWork.DbConnection.InsertAllAsync(objList, _unitOfWork.DbTransaction);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Method} failed for type {T} and object list = {@ObjList}", nameof(InsertAll),
                typeof(T), objList);
            throw;
        }
    }
        
    public async Task Update(T obj)
    {
        try
        {
            _unitOfWork.Begin();
            await _unitOfWork.DbConnection.UpdateAsync(obj, _unitOfWork.DbTransaction);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Method} failed for type {T} and object = {@Obj}", nameof(Update), typeof(T), obj);
            throw;
        }
    }

    public async Task Delete(T obj)
    {
        try
        {
            _unitOfWork.Begin();
            await _unitOfWork.DbConnection.DeleteAsync(obj, _unitOfWork.DbTransaction);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Method} failed for type {T} and object = {@Obj}", nameof(Delete), typeof(T), obj);
            throw;
        }
    }

    public void Save()
    {
        try
        {
            _unitOfWork.Commit();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Method} failed for type {T}", nameof(Save), typeof(T));
            throw;
        }
    }
}