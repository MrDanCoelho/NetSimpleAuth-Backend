using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NetSimpleAuth.Backend.Application.Helpers;
using NetSimpleAuth.Backend.Application.Services;
using NetSimpleAuth.Backend.Domain;
using NetSimpleAuth.Backend.Domain.Dto;
using NetSimpleAuth.Backend.Domain.Entities;
using NetSimpleAuth.Backend.Domain.Interfaces.IRepositories;
using Xunit;

namespace NetSimpleAuth.Backend.Test.Services
{
    public class AccountServiceTest
    {
        private readonly AppSettings _appSettings = new();
        private readonly Mock<ILogger<AccountService>> _logger = new();
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepository = new();
        private readonly Mock<IUserRepository> _userRepository = new();

        [Fact]
        public async Task Authenticate()
        {
            // Arrange
            _appSettings.Secret = "at least 256 bit sized secret string";
            var authUserDto = new AuthUserDto() { Password = "" };
            var password = CryptographyService.HashPassword("");
            _userRepository.Setup(x => x.Select(It.IsAny<Expression<Func<UserEntity, bool>>>()))
                .ReturnsAsync(new List<UserEntity> { new() { UserName = "a", Password = password, PasswordSalt = "" } });
            
            // Act
            var service = new AccountService(_appSettings, _logger.Object, _refreshTokenRepository.Object,
                _userRepository.Object);
            var result = await service.Authenticate(authUserDto);
            
            // Assert
            Assert.NotNull(result);
        }
    }
}