using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NetPOC.Backend.API.Controllers.v1;
using NetPOC.Backend.Domain.Interfaces.IServices;
using NetPOC.Backend.Domain.Models;
using Xunit;

namespace NetPOC.Backend.Test.Controllers
{
    public class CrudControllerBaseTest
    {
        private readonly Mock<ILogger<UsuarioController>> _logger;
        private readonly Mock<IUsuarioService> _crudService;


        public CrudControllerBaseTest()
        {
            _logger = new Mock<ILogger<UsuarioController>>();
            _crudService = new Mock<IUsuarioService>();
        }
        
        [Fact]
        public async Task GetAll()
        {
            // Arrange
            var usuarios = new UsuarioModel[]
            {
                new UsuarioModel()
            };
            _crudService.Setup(x => x.GetAll())
                .Returns(Task.FromResult<IEnumerable<UsuarioModel>>(usuarios));
            
            // Act
            var controller = new UsuarioController(_logger.Object, _crudService.Object);
            var result = await controller.GetAll();
            
            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetById()
        {
            // Arrange
            _crudService.Setup(x => x.GetById(1))
                .Returns(Task.FromResult(new UsuarioModel()));
            
            // Act
            var controller = new UsuarioController(_logger.Object, _crudService.Object);
            var result = await controller.GetById(1);
            
            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task Insert()
        {
            // Arrange
            var usuario = new UsuarioModel();
            
            // Act
            var controller = new UsuarioController(_logger.Object, _crudService.Object);
            var result = await Record.ExceptionAsync(async () => await controller.Insert(usuario));
            
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Update()
        {
            // Arrange
            var usuario = new UsuarioModel();

            // Act
            var controller = new UsuarioController(_logger.Object, _crudService.Object);
            var result = await Record.ExceptionAsync(() => controller.Update(usuario));
            
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Delete()
        {
            // Act
            var controller = new UsuarioController(_logger.Object, _crudService.Object);
            var result = await Record.ExceptionAsync(() => controller.Delete(1));
            
            // Assert
            Assert.Null(result);
        }
    }
}