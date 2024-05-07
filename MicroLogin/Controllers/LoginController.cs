using Facture.IdentityAccess.Application.Components.Entity;
using MicroLogin.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace MicroLogin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController(IJwtServices _iJwtServices) : ControllerBase
    {
        [HttpPost]
        public IActionResult Login(string userName ,string pass)
        {
            var loginData = new LoginData();
            loginData.Username = userName;
            loginData.Password = pass;
            var result = _iJwtServices.LoginUser(Request, loginData);
            return ReplyWith(result);
        }

        private IActionResult ReplyWith(LoginResultData result)
        {
            if (result == null)
            {
                return Unauthorized();
            }

            if (!string.IsNullOrEmpty(result.Error))
            {
                //HACK: this will return the scheme with the error message as a challenge; non-standard method
                return Unauthorized(new AuthenticationHeaderValue(JwtServices.AuthScheme, result.Error));
            }

            return Ok(result);
        }
    }
}
