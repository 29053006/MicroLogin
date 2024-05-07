using Facture.Core.Domain.PersistenceSupport;
using Facture.IdentityAccess.Domain;
using System;

namespace Facture.IdentityAccess.Contracts.Repositories
{
    public interface IAuthorizedSoftwareRepository : IRepository<AuthorizedSoftware>
    {
        AuthorizedSoftware Get(Guid parentTenantId, String token);
        AuthorizedSoftware Get(Guid parentTenantId, Guid tenantId, String token);
    }
}
