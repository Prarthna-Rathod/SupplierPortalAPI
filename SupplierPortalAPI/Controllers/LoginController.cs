using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Interfaces;
using System.Security.Claims;

namespace SupplierPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private ILoginService _loginServices;
        public LoginController(ILoginService loginServices)
        {
            _loginServices = loginServices;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public string Login(string emailId, string password)
        {
            return _loginServices.Login(emailId, password);
        }

        [HttpGet("GetLoginUserEmail")]
        public string GetLoginUserEmail()
        {
            var currentUser = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return currentUser;
        }

        [HttpGet("GetSupplierName")]
        public string GetSupplierName()
        {
            var currentUser = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _loginServices.GetSupplierName(currentUser);
        }
    }
}
