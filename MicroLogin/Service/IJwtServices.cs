using Facture.IdentityAccess.Application.Components.Entity;

namespace MicroLogin.Service
{
    public interface IJwtServices
    {
        String SchemeType { get; }
        LoginResultData LoginUser(HttpRequest request, LoginData loginData);
    }
}
