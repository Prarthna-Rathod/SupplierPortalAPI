using DataAccess.DataActionContext;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Extensions
{
    public static class FindLoggedUser
    {
        private static HttpContext _httpContext;

        public static string FindLoginUserEmail(this HttpContext httpContext)
        {
            _httpContext = httpContext;
            var userEmail = _httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userEmail;
        }

        public static string FindLoginUserRoleName(this HttpContext httpContext)
        {
            _httpContext = httpContext;
            var userRole = _httpContext.User.FindFirstValue(ClaimTypes.Role);
            return userRole;
        }
    
    }
}
