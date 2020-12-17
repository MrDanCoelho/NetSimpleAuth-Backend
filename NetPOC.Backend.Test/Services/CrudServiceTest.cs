using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NetPOC.Backend.Application.Services;
using NetPOC.Backend.Domain.Interfaces.IRepositories;
using NetPOC.Backend.Domain.Models;
using NetPOC.Backend.Infra;
using NetPOC.Backend.Infra.Repositories;
using Xunit;

namespace NetPOC.Backend.Test.Services
{
    public class CrudServiceTest
    {
        private readonly Mock<ILogger<UsuarioService>> _logger;
        private readonly Mock<IUsuarioRepository> _crudRepository;
        
        public CrudServiceTest()
        {
            _logger = new Mock<ILogger<UsuarioService>>();
            _crudRepository = new Mock<IUsuarioRepository>();
        }
        
        [Fact]
        public async Task GetAll()
        {
            // Arrange
            var usuarios = new UsuarioModel[]
            {
                new UsuarioModel()
            };
            _crudRepository.Setup(x => x.GetAll())
                .Returns(Task.FromResult<IEnumerable<UsuarioModel>>(usuarios));
            
            // Act
            var service = new UsuarioService(_logger.Object, _crudRepository.Object);
            var result = await service.GetAll();
            
            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetById()
        {
            // Arrange
            _crudRepository.Setup(x => x.GetById(1))
                .Returns(Task.FromResult(new UsuarioModel()));
            
            // Act
            var service = new UsuarioService(_logger.Object, _crudRepository.Object);
            var result = await service.GetById(1);
            
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Insert()
        {
            // Arrange
            var usuario = new UsuarioModel();
            
            // Act
            var service = new UsuarioService(_logger.Object, _crudRepository.Object);
            var result = await Record.ExceptionAsync(async () => await service.Insert(usuario));
            
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Update()
        {
            // Arrange
            var usuario = new UsuarioModel();

            // Act
            var service = new UsuarioService(_logger.Object, _crudRepository.Object);
            var result = await Record.ExceptionAsync(() => service.Update(usuario));
            
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Delete()
        {
            // Arrange
            var usuario = new UsuarioModel();

            // Act
            var service = new UsuarioService(_logger.Object, _crudRepository.Object);
            var result = await Record.ExceptionAsync(() => service.Delete(usuario));
            
            // Assert
            Assert.Null(result);
        }
    }
}