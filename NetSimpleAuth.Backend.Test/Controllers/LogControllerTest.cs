using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NetPOC.Backend.Domain.Interfaces.IServices;
using NetSimpleAuth.Backend.API.Controllers.v1;
using Xunit;

namespace NetPOC.Backend.Test.Controllers
{
    public class LogControllerTest
    {
        private readonly Mock<ILogger<LogController>> _logger;
        private readonly Mock<ILogService> _logService;


        public LogControllerTest()
        {
            _logger = new Mock<ILogger<LogController>>();
            _logService = new Mock<ILogService>();
        }

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
    }
}