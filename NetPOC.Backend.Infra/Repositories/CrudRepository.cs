using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetPOC.Backend.Domain.Interfaces;
using NetPOC.Backend.Domain.Interfaces.IRepositories;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace NetPOC.Backend.Infra.Repositories
{
    /// <inheritdoc/>
    public abstract class CrudRepository<T> : ICrudRepository<T> where T : class
    {
        private readonly ILogger<CrudRepository<T>> _logger;
        public DataContext _context;
        public DbSet<T> _table;
        
        /// <summary>
        /// Repositório abstrato de operações CRUD
        /// </summary>
        /// <param name="logger">Logger do tipo <see cref="ILogger{CrudRepository{T}}"/></param>
        /// <param name="context"><see cref="DataContext"/> da aplicação</param>
        public CrudRepository(ILogger<CrudRepository<T>> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
            _table = _context.Set<T>();
        }
        
        public async Task<IEnumerable<T>> GetAll()
        {
            try
            {
                _logger.LogInformation($"Inicio - {nameof(GetAll)} ({nameof(T)})");
                
                var result = await _table.ToListAsync();
                
                _logger.LogInformation($"Fim - {nameof(GetAll)} ({nameof(T)})");

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
                _logger.LogInformation($"Inicio - {nameof(GetById)} ({nameof(T)})");
                
                var result = await _table.FindAsync(id);
                
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
                
                await _table.AddAsync(obj);
                
                _logger.LogInformation($"Fim - {nameof(Insert)} ({nameof(T)})");
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(Insert)} ({nameof(T)}): {e}");
                throw;
            }
        }
        
        public void Update(T obj)
        {
            try
            {
                _logger.LogInformation($"Inicio - {nameof(Update)} ({nameof(T)})");
                
                _table.Update(obj);
                
                _logger.LogInformation($"Fim - {nameof(Update)} ({nameof(T)})");
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(Update)} ({nameof(T)}): {e}");
                throw;
            }
        }
        
        public void Delete(T obj)
        {
            try
            {
                _logger.LogInformation($"Inicio - {nameof(Delete)} ({nameof(T)})");
                
                _table.Remove(obj);
                
                _logger.LogInformation($"Fim - {nameof(Delete)} ({nameof(T)})");
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(Delete)} ({nameof(T)}): {e}");
                throw;
            }
        }
        
        public async Task Save()
        {
            try
            {
                _logger.LogInformation($"Inicio - {nameof(Save)} ({nameof(T)})");
                
                await _context.SaveChangesAsync();
                
                _logger.LogInformation($"Fim - {nameof(Save)} ({nameof(T)})");
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(Save)} ({nameof(T)}): {e}");
                throw;
            }
        }
    }
}