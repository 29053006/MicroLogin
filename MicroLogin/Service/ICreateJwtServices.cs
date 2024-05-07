using Facture.IdentityAccess.Contracts.Model;
using System.IdentityModel.Tokens.Jwt;

namespace MicroLogin.Service
{
    public interface ICreateJwtServices
    {
        JwtSecurityToken CreateJwtToken(String issuer, Guid tenantId, String username, DateTime expires, JwtIdentity identity, byte[] saltKey, string ip = null, bool isWeb = false, string rol = null);
    }
}
