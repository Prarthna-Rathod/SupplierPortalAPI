using DataAccess.DataActionContext;
using DataAccess.DataActions.Interfaces;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services
{
    public class LoginAndTokenService : ILoginAndTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly ISupplierDataActions _supplierDataActions;

        public LoginAndTokenService(IConfiguration configuration, ISupplierDataActions supplierDataActions)
        {
            _configuration = configuration;
            _supplierDataActions = supplierDataActions;
        }

        public string LoginAndTokenGeneration(string emailId, string password)
        {
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

        public string FindLoginUserSupplierName(string emailId)
        {
            var user = _supplierDataActions.GetUserByEmailId(emailId);

            if (user == null)
                throw new Exception("User not found !! Wrong EmailId");

            if (user.Role.Name == "External")
            {
                var allContacts = _supplierDataActions.GetAllContacts();
                var contact = allContacts.Where(x => x.UserId == user.Id).FirstOrDefault();

                if (contact == null)
                    throw new Exception("Contact not found !!");

                return contact.Supplier.Name;
            }
            else
                throw new Exception("Supplier can't found for Internal user !! Should be login with external user emailId");
        }



    }
}
