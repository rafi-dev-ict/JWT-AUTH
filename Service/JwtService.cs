using JWT_AUTH.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JWT_AUTH.Service
{
    public class JwtService
    {
        private readonly AppDbContext appDbContext;
        private readonly IConfiguration configuration;

        public JwtService(AppDbContext appDbContext, IConfiguration configuration)
        {
            this.appDbContext = appDbContext;
            this.configuration = configuration;
        }

        public async Task<LoginResponse> Authenticate(LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return null;
            }

            //var userAcc = await appDbContext.LoginRequest.FirstOrDefaultAsync(x => x.Username == request.Username);
            //if (userAcc is null)
            //{
            //    return null;
            //}

            var issuer = configuration["JwtConfig:Issuer"];
            var audience = configuration["JwtConfig:Audience"];
            var key = configuration["JwtConfig:Key"];
            var tokenValidityMin = configuration.GetValue<int>("JwtConfig:TokenValidityMins");
            var tokenExpiryTimestamp = DateTime.UtcNow.AddMinutes(tokenValidityMin);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Name, request.Username)
                }),
                Expires = tokenExpiryTimestamp,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                SecurityAlgorithms.HmacSha512Signature),
            };

            var tokenHandler=new JwtSecurityTokenHandler();
            var newkey = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)); // 64 bytes = 512 bits
            Console.WriteLine(newkey);

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken=tokenHandler.WriteToken(securityToken);

            return new LoginResponse
            {
                AccessToken = accessToken,
                UserName = request.Username,
                ExpiresIn = (int)tokenExpiryTimestamp.Subtract(DateTime.UtcNow).TotalSeconds,
            };
        }
    }
}
