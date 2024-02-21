using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NetSimpleAuth.Backend.API.Controllers.v1;
using NetSimpleAuth.Backend.Domain.Dto;
using NetSimpleAuth.Backend.Domain.Entities;
using NetSimpleAuth.Backend.Domain.Interfaces.IServices;
using NetSimpleAuth.Backend.Domain.Response;
using Xunit;

namespace NetSimpleAuth.Backend.Test.Controllers
{
    public class LogControllerTest
    {
        private readonly Mock<ILogger<LogController>> _logger = new();
        private readonly Mock<ILogService> _logService = new();


        [Fact]
        public async Task InsertBatch()
        {
            // Arrange
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(""));
            
            // Act
            var controller = new LogController(_logger.Object, _logService.Object);
            var result = await controller.InsertBatch(new FormFile(ms, 0, 0, "", ""));
            
            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }
        
        [Fact]
        public async Task SelectPaginated()
        {
            // Arrange
            var logFilterDto = new LogFilterDto();
            _logService.Setup(a => a.SelectPaginated(logFilterDto, 0, 0))
                .ReturnsAsync(new SelectPaginatedResponse<LogEntity>());
            
            // Act
            var controller = new LogController(_logger.Object, _logService.Object);
            var result = await controller.SelectPaginated(logFilterDto, 0, 0);
            
            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }
    }
}