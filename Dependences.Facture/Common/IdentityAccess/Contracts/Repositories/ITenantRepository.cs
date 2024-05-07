using Facture.Core.Domain.PersistenceSupport;
using Facture.IdentityAccess.Domain;
using PLColab.Contracts.Internal.Base.Contracts.Model.RazonSocialFactureDian;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dependences.Facture.Common.IdentityAccess.Contracts.Repositories
{
    public interface ITenantRepository : IRepositoryWithTypedId<Tenant, Guid>
    {
        Tenant Get(string tenantName, bool cacheable = true, bool readOnly = true);
        string GetTenantSettingValueByName(Guid tenantId, string name);
    }
}
