using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetPOC.Backend.Domain.Interfaces.IRepositories;
using NetPOC.Backend.Domain.Models;

namespace NetPOC.Backend.Infra.Repositories
{
    /// <inheritdoc cref="IUsuarioRepository" />
    public class UsuarioRepository : CrudRepository<UsuarioModel>, IUsuarioRepository
    {
        /// <summary>
        /// Repositório do <see cref="UsuarioModel"/>
        /// </summary>
        /// <param name="logger">Logger do tipo <see cref="ILogger{UsuarioRepository}"/></param>
        /// <param name="context"><see cref="DataContext"/> da aplicação</param>
        public UsuarioRepository(ILogger<UsuarioRepository> logger, DataContext context) 
            : base(logger, context)
        {
            
        }
    }
}