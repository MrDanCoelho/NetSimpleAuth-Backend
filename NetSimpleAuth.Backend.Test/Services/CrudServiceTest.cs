﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NetPOC.Backend.Application.Services;
using NetPOC.Backend.Domain.Entities;
using NetPOC.Backend.Domain.Interfaces.IRepositories;
using Xunit;

namespace NetPOC.Backend.Test.Services
{
    public class CrudServiceTest
    {
        private readonly Mock<ILogger<LogService>> _logger;
        private readonly Mock<ILogRepository> _crudRepository;
        
        public CrudServiceTest()
        {
            _logger = new Mock<ILogger<LogService>>();
            _crudRepository = new Mock<ILogRepository>();
        }
        
        [Fact]
        public async Task GetAll()
        {
            // Arrange
            _crudRepository.Setup(x => x.GetAll())
                .Returns(Task.FromResult<IEnumerable<LogEntity>>(new List<LogEntity>()));
            
            // Act
            var service = new LogService(_logger.Object, _crudRepository.Object);
            var result = await service.GetAll();
            
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetById()
        {
            // Arrange
            _crudRepository.Setup(x => x.GetById(1))
                .Returns(Task.FromResult(new LogEntity()));
            
            // Act
            var service = new LogService(_logger.Object, _crudRepository.Object);
            var result = await service.GetById(1);
            
            // Assert
            Assert.NotNull(result);
        }
        
        [Fact]
        public async Task SelectFirst()
        {
            // Arrange
            _crudRepository.Setup(x => x.SelectFirst(a => a.Id == 1))
                .Returns(Task.FromResult(new LogEntity()));
            
            // Act
            var service = new LogService(_logger.Object, _crudRepository.Object);
            var result = await service.SelectFirst(a => a.Id == 1);
            
            // Assert
            Assert.NotNull(result);
        }
        
        [Fact]
        public async Task SelectAll()
        {
            // Arrange
            _crudRepository.Setup(x => x.SelectAll(a => a.Id == 1))
                .Returns(Task.FromResult<IEnumerable<LogEntity>>(new List<LogEntity>()));
            
            // Act
            var service = new LogService(_logger.Object, _crudRepository.Object);
            var result = await service.SelectAll(a => a.Id == 1);
            
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Insert()
        {
            // Arrange
            var log = new LogEntity();
            
            // Act
            var service = new LogService(_logger.Object, _crudRepository.Object);
            var result = await Record.ExceptionAsync(async () => await service.Insert(log));
            
            // Assert
            Assert.Null(result);
        }
        
        [Fact]
        public async Task InsertAll()
        {
            // Act
            var service = new LogService(_logger.Object, _crudRepository.Object);
            var result = await Record.ExceptionAsync(async () => await service.InsertAll(new List<LogEntity>()));
            
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Update()
        {
            // Arrange
            var log = new LogEntity();

            // Act
            var service = new LogService(_logger.Object, _crudRepository.Object);
            var result = await Record.ExceptionAsync(() => service.Update(log));
            
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Delete()
        {
            // Arrange
            var log = new LogEntity();

            // Act
            var service = new LogService(_logger.Object, _crudRepository.Object);
            var result = await Record.ExceptionAsync(() => service.Delete(log));
            
            // Assert
            Assert.Null(result);
        }
    }
}