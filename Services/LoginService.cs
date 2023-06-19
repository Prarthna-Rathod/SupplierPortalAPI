﻿using DataAccess.DataActionContext;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;
using SupplierPortalAPI.Infrastructure.Middleware.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services
{
    public class LoginService : ILoginService
    {
        private readonly IConfiguration _configuration;
        private readonly SupplierPortalDBContext _context;

        public LoginService(IConfiguration configuration, SupplierPortalDBContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public string Login(string emailId, string password)
        {
            if (password != "Admin@123")
                throw new BadRequestException("Password does not matched !!");

            var user = Authenticate(emailId);
            var token = GenerateToken(user);
            return token;

        }
        //To Generate token
        private string GenerateToken(UserEntity user)
        { 
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Email),
                new Claim(ClaimTypes.Role,user.Role.Name),
                new Claim(ClaimTypes.Name,user.Name)
            };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.Now.AddDays(1), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //To Authenticate user
        private UserEntity Authenticate(string emailId)
        {
            var currentUser = _context.UserEntities.Include(x=>x.Role).FirstOrDefault(x => x.Email == emailId);
            if (currentUser == null)
                throw new Exception("User not found!!");

            return currentUser;
        }
        public string GetSupplierName(string emailId)
        {
            var contact = _context.ContactEntities.Where(x => x.User.Email == emailId).Include(x => x.Supplier).FirstOrDefault();

            if (contact == null)
                throw new Exception("Contact not found !!");

            return contact.Supplier.Name;
        }
    }
}