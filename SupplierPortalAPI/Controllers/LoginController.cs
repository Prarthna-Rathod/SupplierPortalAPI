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
        private readonly IJwtTokenService _jwtTokenService;
        private readonly SupplierPortalDBContext _context;

        public LoginController(IJwtTokenService jwtTokenService, SupplierPortalDBContext context)
        {
            _jwtTokenService = jwtTokenService;
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost("LoginAndGenerateToken")]
        public string LoginAndGenerateToken(string email, string password)
        {
            return _jwtTokenService.LoginAndTokenGeneration(email, password);
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
            var userId = HttpContext.FindLoginUserEmail();
            return _context.FindLoginUserSupplierName(userId);
        }

    }
}
