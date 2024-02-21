using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetSimpleAuth.Backend.Domain.Interfaces.IServices;

/// <summary>
/// Generic CRUD service
/// </summary>
public interface ICrudService<T> where T : class
{
    /// <summary>
    /// Service to collect all objects according to their indicated type
    /// </summary>
    /// <returns>List of the objects in the database</returns>
    Task<IEnumerable<T>> GetAll();
        
    /// <summary>
    /// Service to collect a object according to the indicated type and it's ID
    /// </summary>
    /// <param name="id">ID of the object to be searched</param>
    /// <returns>Object found or null if none</returns>
    Task<T> GetById(object id);

    /// <summary>
    /// Service to insert an object
    /// </summary>
    /// <param name="obj">Object to be inserted</param>
    Task Insert(T obj);
        
    /// <summary>
    /// Service to update an object
    /// </summary>
    /// <param name="obj">Object to be updated</param>
    Task Update(T obj);
        
    /// <summary>
    /// Service to delete an object accordingly to it's ID
    /// </summary>
    /// <param name="id">ID of the object to be deleted</param>
    Task Delete(object id);
}