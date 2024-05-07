using System;
using System.Collections.Generic;

namespace Facture.IdentityAccess.Domain.Services
{
    public interface IAuthenticationService
    {
        UserDescriptor Authenticate(string username, string password, out IEnumerable<Tenant> tenants, Guid? tenantId = null, string portal = null);
    }
}