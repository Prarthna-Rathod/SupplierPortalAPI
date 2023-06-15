using DataAccess.DataActions.Interfaces;
using DataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly ISupplierDataActions _supplierDataActions;

        public JwtTokenService(IConfiguration configuration, ISupplierDataActions supplierDataActions)
        {
            _configuration = configuration;
            _supplierDataActions = supplierDataActions;
        }

        public string LoginAndTokenGeneration(string emailId, string password)
        {
            //if (password != "1234")
            //    throw new Exception("Password not matched !!");

            var userEntity = _supplierDataActions.GetUserByEmailId(emailId.ToLower());

            var token = GenerateToken(userEntity);

            return token;
        }

        private string GenerateToken(UserEntity userEntity)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userEntity.Email),
                    new Claim(ClaimTypes.Role, userEntity.Role.Name)
                };

                var token = new JwtSecurityToken(_configuration["JWT:Issuer"], _configuration["JWT:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token); ;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured during token generation !! " + ex.Message);
            }
        }

    }
}
