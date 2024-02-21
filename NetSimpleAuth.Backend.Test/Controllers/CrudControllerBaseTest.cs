using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NetSimpleAuth.Backend.API.Controllers.v1;
using NetSimpleAuth.Backend.Domain.Entities;
using NetSimpleAuth.Backend.Domain.Interfaces.IServices;
using Xunit;

namespace NetSimpleAuth.Backend.Test.Controllers
{
    public class CrudControllerBaseTest
    {
        private readonly Mock<ILogger<LogController>> _logger = new();
        private readonly Mock<ILogService> _crudService = new();


        [Fact]
        public async Task GetAll()
        {
            // Arrange
            var logs = new[]
            {
                new LogEntity()
            };
            _crudService.Setup(x => x.GetAll())
                .Returns(Task.FromResult<IEnumerable<LogEntity>>(logs));
            
            // Act
            var controller = new LogController(_logger.Object, _crudService.Object);
            var result = await controller.GetAll();
            
            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetById()
        {
            // Arrange
            _crudService.Setup(x => x.GetById(1))
                .Returns(Task.FromResult(new LogEntity()));
            
            // Act
            var controller = new LogController(_logger.Object, _crudService.Object);
            var result = await controller.GetById(1);
            
            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task Insert()
        {
            // Arrange
            var log = new LogEntity();
            
            // Act
            var controller = new LogController(_logger.Object, _crudService.Object);
            var result = await Record.ExceptionAsync(async () => await controller.Insert(log));
            
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Update()
        {
            // Arrange
            var log = new LogEntity();

            // Act
            var controller = new LogController(_logger.Object, _crudService.Object);
            var result = await Record.ExceptionAsync(() => controller.Update(log));
            
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Delete()
        {
            // Act
            var controller = new LogController(_logger.Object, _crudService.Object);
            var result = await Record.ExceptionAsync(() => controller.Delete(1));
            
            // Assert
            Assert.Null(result);
        }
    }
}