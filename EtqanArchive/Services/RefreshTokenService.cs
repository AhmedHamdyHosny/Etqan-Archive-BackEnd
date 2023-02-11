using System;
using System.Security.Cryptography;

namespace EtqanArchive.BackEnd.Services
{
    public interface IRefreshTokenService
    {
        string GenerateRefreshToken();
    }

    public class RefreshTokenService : IRefreshTokenService
    {
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using(var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
