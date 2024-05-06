using System;
using System.Collections.Generic;

namespace Facture.IdentityAccess.Application.Components.Services
{
    public interface ITokenService
    {
        String Generate(String branchCode, String processCode, String tenantName, String LDF, DateTime? expirationDate = null);
        String Generate(Guid tenantId, String branchCode, String processCode, String tenantName, String LDF, DateTime? expirationDate = null);

        String Generate(String issuer, IDictionary<String, Object> claims, DateTime issueAt, DateTime notAfter, DateTime? notBefore = null);
        String Generate(String issuer, IDictionary<String, Object> claims, DateTime notAfter, DateTime? notBefore = null);
        Boolean ValidateToken(String rawToken, String issuer, Boolean checkExpiry);
    }
}
