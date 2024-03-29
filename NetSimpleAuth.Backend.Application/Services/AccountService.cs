﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NetSimpleAuth.Backend.Application.Helpers;
using NetSimpleAuth.Backend.Domain;
using NetSimpleAuth.Backend.Domain.Dto;
using NetSimpleAuth.Backend.Domain.Entities;
using NetSimpleAuth.Backend.Domain.Interfaces.IRepositories;
using NetSimpleAuth.Backend.Domain.Interfaces.IServices;
using NetSimpleAuth.Backend.Domain.Response;

namespace NetSimpleAuth.Backend.Application.Services;

/// <inheritdoc/>
public class AccountService(AppSettings appSettings,
        ILogger<AccountService> logger,
        IRefreshTokenRepository refreshTokenRepository,
        IUserRepository userRepository)
    : IAccountService
{
    public async Task<AuthUserResponse> Authenticate(AuthUserDto authUserDto)
    {
        try
        {
            logger.LogDebug("Beginning Authentication");

            var userList =  await userRepository.Select(a => a.UserName == authUserDto.Identity || a.Email == authUserDto.Identity);

            var user = userList.FirstOrDefault(a => CryptographyService.HashPassword(authUserDto.Password + a.PasswordSalt) == a.Password);

            if (user == null)
                throw new AuthenticationException("Invalid user. Check your password and/or username");

            logger.LogDebug("User found: {@User}", user);

            var jwtToken = GenerateJwtToken(user);
            logger.LogDebug("New token generated: {@JwtToken}", jwtToken);

            var refreshToken = GenerateRefreshToken(authUserDto.IpAddress);
            refreshToken.UserId = user.Id;

            await refreshTokenRepository.Insert(refreshToken);
            refreshTokenRepository.Save();
                
            logger.LogDebug("New refresh token generated: {@RefreshToken}", refreshToken);

            var authUserResponse = new AuthUserResponse()
            {
                Username = user.UserName,
                JwtToken = jwtToken,
                RefreshToken = refreshToken.Token
            };

            logger.LogDebug("Authentication successful");
                
            return authUserResponse;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error during authentication");
            throw;
        }
    }
        
    private string GenerateJwtToken(UserEntity user)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
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
        catch (Exception e)
        {
            logger.LogError(e, "Unable to generate JWT token for user={@User}", user);
            throw;
        }
    }

    private RefreshTokenEntity GenerateRefreshToken(string ipAddress)
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);
        
        return new RefreshTokenEntity
        {
            Token = Convert.ToBase64String(randomBytes),
            Expires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow,
            CreatedByIp = ipAddress
        };
    }
}