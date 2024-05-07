using Facture.Core.Base.Configurations;
using Facture.Core.Filters;
using Facture.IdentityAccess.Application.Components.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace Facture.IdentityAccess.AuthServices.Services
{
    [Authorize]
    //[UnhandledExceptionFilter]
    [ValidateModel]
    public class RegistrationController : ControllerBase
    {
        //[HttpPost]
        //[AllowAnonymous]
        //public IActionResult Login(LoginData loginData)
        //{
        //    JwtServices IJwtServices = new JwtServices();

        //    var result = IJwtServices.LoginUser(Request, loginData);
        //    return ReplyWith(result);
        //}
    }
}
