using Microsoft.Extensions.Logging;
using NetPOC.Backend.Domain.Interfaces.IRepositories;
using NetPOC.Backend.Domain.Interfaces.IServices;
using NetPOC.Backend.Domain.Models;

namespace NetPOC.Backend.Application.Services
{
    /// <inheritdoc cref="IUsuarioService" />
    public class UsuarioService : CrudService<UsuarioModel>, IUsuarioService
    {
        private readonly ILogger<UsuarioService> _logger;
        private readonly IUsuarioRepository _usuarioRepository;
        
        public UsuarioService(ILogger<UsuarioService> logger, IUsuarioRepository usuarioRepository)
            : base(logger, usuarioRepository)
        {
            _logger = logger;
            _usuarioRepository = usuarioRepository;
        }
    }
}