using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NetPOC.Backend.Domain.Models;
using NetPOC.Backend.Infra;
using NetPOC.Backend.Infra.Repositories;
using Xunit;

namespace NetPOC.Backend.Test.Repositories
{
    public class CrudRepositoryTest
    {
        private readonly Mock<ILogger<UsuarioRepository>> _logger;
        private DbContextOptions<DataContext> _options;
        
        public CrudRepositoryTest()
        {
            _logger = new Mock<ILogger<UsuarioRepository>>();
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "FakeDB")
                .Options;
        }

        [Fact]
        public async Task GetAll()
        {
            var context = new DataContext(_options);
            await context.Usuario.AddAsync(new UsuarioModel() {Id = 1});
            await context.SaveChangesAsync();
            
            // Act
            var repository = new UsuarioRepository(_logger.Object, context);
            var result = await repository.GetAll();
            
            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetById()
        {
            var context = new DataContext(_options);
            await context.Usuario.AddAsync(new UsuarioModel());
            await context.SaveChangesAsync();
            
            // Act
            var repository = new UsuarioRepository(_logger.Object, context);
            var result = await repository.GetById(1);
            
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Insert()
        {
            // Arrange
            var usuario = new UsuarioModel();
            
            var context = new DataContext(_options);
            
            // Act
            var repository = new UsuarioRepository(_logger.Object, context);
            var result = await Record.ExceptionAsync(async () => await repository.Insert(usuario));
            
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Update()
        {
            // Arrange
            var usuario = new UsuarioModel();
            
            var context = new DataContext(_options);
            await context.Usuario.AddAsync(usuario);
            await context.SaveChangesAsync();

            // Act
            var repository = new UsuarioRepository(_logger.Object, context);
            var result = Record.Exception(() => repository.Update(usuario));
            
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Delete()
        {
            // Arrange
            var usuario = new UsuarioModel();
            
            var context = new DataContext(_options);
            await context.Usuario.AddAsync(usuario);
            await context.SaveChangesAsync();
            
            // Act
            var repository = new UsuarioRepository(_logger.Object, context);
            var result = Record.Exception(() => repository.Delete(usuario));
            
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Save()
        {
            var context = new DataContext(_options);
            
            // Act
            var repository = new UsuarioRepository(_logger.Object, context);
            var result = await Record.ExceptionAsync(async () => await repository.Save());
            
            // Assert
            Assert.Null(result);
        }
    }
}