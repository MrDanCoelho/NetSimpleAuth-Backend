using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NetPOC.Backend.Application.Helpers;
using NetPOC.Backend.Domain;
using NetPOC.Backend.Domain.Dto;
using NetPOC.Backend.Domain.Entities;
using NetPOC.Backend.Domain.Interfaces.IRepositories;
using NetPOC.Backend.Domain.Interfaces.IServices;

namespace NetPOC.Backend.Application.Services
{
    /// <inheritdoc/>
    public class AccountService : IAccountService
    {
        private readonly AppSettings _appSettings;
        private readonly ILogger<AccountService> _logger;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        
        public AccountService(
            AppSettings appSettings,
            ILogger<AccountService> logger,
            IRefreshTokenRepository refreshTokenRepository,
            IUserRepository userRepository
            )
        {
            _appSettings = appSettings;
            _logger = logger;
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
        }
        
        public async Task<AuthUserResponse> Authenticate(AuthUserDto authUserDto)
        {
            try
            {
                _logger.LogInformation($"Begin - {nameof(Authenticate)}");

                var userList =  await _userRepository.SelectAll(a => (a.UserName == authUserDto.Identity || a.Email == authUserDto.Identity));

                var user = userList.FirstOrDefault(a => CryptographyService.HashPassword(authUserDto.Password + a.PasswordSalt) == a.Password);

                if (user == null)
                    throw new AuthenticationException("Invalid user. Check your password and/or username");

                var jwtToken = GenerateJwtToken(user);
                var refreshToken = GenerateRefreshToken(authUserDto.IpAddress);

                refreshToken.UserId = user.Id;

                await _refreshTokenRepository.Insert(refreshToken);
                _refreshTokenRepository.Save();
                
                var authUserResponse = new AuthUserResponse()
                {
                    Username = user.UserName,
                    JwtToken = jwtToken,
                    RefreshToken = refreshToken.Token
                };

                _logger.LogInformation($"End - {nameof(Authenticate)}");
                
                return authUserResponse;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(Authenticate)}: {e}");
                throw;
            }
        }
        
        private string GenerateJwtToken(UserEntity user)
        {
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] 
                {
                    new Claim(ClaimTypes.Name, user.UserName)
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshTokenEntity GenerateRefreshToken(string ipAddress)
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return new RefreshTokenEntity
            {
                Token = Convert.ToBase64String(randomBytes),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }
    }
}