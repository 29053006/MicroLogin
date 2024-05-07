using Facture.IdentityAccess.Contracts.Model;
using System;

namespace Facture.IdentityAccess.Application.Components.Services
{
    public interface ITokenEnrichService
    {
        void Enrich(Guid tenantId, String tenantName, ref JwtIdentity jwt);
    }
}
