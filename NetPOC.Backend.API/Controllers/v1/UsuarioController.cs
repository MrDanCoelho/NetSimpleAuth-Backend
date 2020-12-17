using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetPOC.Backend.Domain.Interfaces.IServices;
using NetPOC.Backend.Domain.Models;

namespace NetPOC.Backend.API.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UsuarioController : CrudControllerBase<UsuarioModel>
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly IUsuarioService _usuarioService;
        
        public UsuarioController(ILogger<UsuarioController> logger, IUsuarioService usuarioService)
            : base(logger, usuarioService)
        {
            _logger = logger;
            _usuarioService = usuarioService;
        }
    }
}