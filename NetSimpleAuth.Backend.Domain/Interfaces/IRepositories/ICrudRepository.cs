using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NetSimpleAuth.Backend.Domain.Interfaces.IRepositories;

public interface ICrudRepository<T> where T : class
{
    /// <summary>
    /// Repository service to get all repository data
    /// </summary>
    /// <returns>List of all the objects of the repository</returns>
    Task<IEnumerable<T>> GetAll();
        
    /// <summary>
    /// Repository service to get a single object by it's ID
    /// </summary>
    /// <param name="id">ID of the object to be searched</param>
    /// <returns>The object with the ID supplied or null if not found</returns>
    Task<T> GetById(object id);
        
    /// <summary>
    /// Repository service to search for a list of objects according to an expression
    /// </summary>
    /// <param name="predicate">The predicate of the objects to be searched</param>
    /// <returns>A IEnumerable that matches the predicate supplied or null if not found</returns>
    Task<IEnumerable<T>> Select(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Repository service to insert an object in the repository
    /// </summary>
    /// <param name="obj">Object to be inserted</param>
    Task Insert(T obj);
        
    /// <summary>
    /// Repository service to insert a list of objects in the repository
    /// </summary>
    /// <param name="objList">List of objects to be inserted</param>
    Task InsertAll(IEnumerable<T> objList);
        
    /// <summary>
    /// Repository service to update an object in the repository
    /// </summary>
    /// <param name="obj">Object to be updated</param>
    Task Update(T obj);
        
    /// <summary>
    /// Repository service to delete a single object
    /// </summary>
    /// <param name="obj">Object to be deleted</param>
    Task Delete(T obj);
        
    /// <summary>
    /// Repository service to commit changes made
    /// </summary>
    void Save();
}