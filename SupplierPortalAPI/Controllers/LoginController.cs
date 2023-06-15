using DataAccess.DataActionContext;
using DataAccess.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Security.Claims;

namespace SupplierPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginAndTokenService _loginAndTokenService;

        public LoginController(ILoginAndTokenService loginAndTokenService)
        {
            _loginAndTokenService = loginAndTokenService;
        }

        [AllowAnonymous]
        [HttpPost("LoginAndGenerateToken")]
        public string LoginAndGenerateToken(string email, string password)
        {
            return _loginAndTokenService.LoginAndTokenGeneration(email, password);
        }

        [Authorize]
        [HttpGet("GetAuthorizeLoggedUserEmail")]
        public ActionResult GetAuthorizeLoggedUserEmail() 
        {
            var userId = HttpContext.FindLoginUserEmail();
            return Ok(userId);
        }

        [Authorize]
        [HttpGet("GetSupplierName")]
        public string GetSupplierName()
        {
            var userEmailId = HttpContext.FindLoginUserEmail();
            return _loginAndTokenService.FindLoginUserSupplierName(userEmailId);
        }

    }
}
