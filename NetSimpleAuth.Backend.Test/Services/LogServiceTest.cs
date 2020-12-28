using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NetPOC.Backend.Application.Services;
using NetPOC.Backend.Domain.Dto;
using NetPOC.Backend.Domain.Entities;
using NetPOC.Backend.Domain.Interfaces.IRepositories;
using Xunit;

namespace NetPOC.Backend.Test.Services
{
    public class LogServiceTest
    {
        private readonly Mock<ILogger<LogService>> _logger;
        private readonly Mock<ILogRepository> _logRepository;
        
        public LogServiceTest()
        {
            _logger = new Mock<ILogger<LogService>>();
            _logRepository = new Mock<ILogRepository>();
        }

        [Fact]
        public async Task InsertBatch()
        {
            // Arrange
            const string content = "144.203.204.43 test - [04/Nov/2020:12:16:04 -1000] \"PUT http://test.com HTTP/1.0\" 422 1 \"http://test2.com\" \"Mozilla/5.0\"\n" +
            "22.52.125.200 - - [20/Aug/2019:20:24:09 -1200] \"POST http://teste3.com/ HTTP/1.1\" 200 884";
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(content));
            
            // Act
            var service = new LogService(_logger.Object, _logRepository.Object);
            var result = await Record.ExceptionAsync(async () => await service.InsertBatch(new StreamReader(ms)));
            
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task SelectPaginated()
        {
            // Arrange
            var logFilter = new LogFilterDto();
            _logRepository.Setup(a => a.SelectPaginated(logFilter, 0, 0))
                .ReturnsAsync(new SelectPaginatedResponse<LogEntity>());
            
            // Act
            var service = new LogService(_logger.Object, _logRepository.Object);
            var result = await service.SelectPaginated(logFilter, 0, 0);
            
            // Assert
            Assert.NotNull(result);
        }
    }
}