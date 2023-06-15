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
        private static SupplierPortalDBContext _context;

        public static string FindLoginUserEmail(this HttpContext httpContext)
        {
            _httpContext = httpContext;
            var userEmail = _httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userEmail;
        }

        public static string FindLoginUserSupplierName(this SupplierPortalDBContext context, string emailId)
        {
            _context = context;
            var user = _context.UserEntities.Include(x => x.Role).FirstOrDefault(x => x.Email == emailId);

            if (user == null)
                throw new Exception("User not found !! Wrong EmailId");

            if (user.Role.Name == "External")
            {
                var contact = _context.ContactEntities.Where(x => x.UserId == user.Id)
                                                      .Include(x => x.Supplier)
                                                      .FirstOrDefault();

                if (contact == null)
                    throw new Exception("Contact not found !!");

                return contact.Supplier.Name;
            }
            else
                throw new Exception("Supplier can't found for Internal user !! Should be login with external user emailId");
        }
    
        public static string FindLoginUserRoleName(this HttpContext httpContext)
        {
            _httpContext = httpContext;
            var userRole = _httpContext.User.FindFirstValue(ClaimTypes.Role);
            return userRole;
        }
    
    }
}
