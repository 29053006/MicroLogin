
using Facture.Core.Domain.PersistenceSupport;
using Facture.IdentityAccess.Contracts.Repositories;
using Facture.IdentityAccess.Domain;
using System;

namespace Facture.IdentityAccess.Infrastructure.Repositories
{
    public class AuthorizedSoftwareRepository : IAuthorizedSoftwareRepository
    {
        public IDbContext DbContext => throw new NotImplementedException();

        public void Delete(AuthorizedSoftware entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        //cambiar ORM
        //public AuthorizedSoftware Get(Guid parentTenantId, string token)
        //{
        //    var software = Session.CreateCriteria<AuthorizedSoftware>("AuthorizedSoftware")
        //                             .Add(Restrictions.Eq("AuthorizedSoftware.ParentTenant.Id", parentTenantId))
        //                             .Add(Restrictions.Eq("AuthorizedSoftware.Token", token))
        //                             .SetCacheable(true)
        //                             .SetFlushMode(NHibernate.FlushMode.Manual)
        //                             .SetReadOnly(true)
        //                             .UniqueResult<AuthorizedSoftware>();
        //    return software;
        //}
        ////cambiar ORM
        //public AuthorizedSoftware Get(Guid parentTenantId, Guid tenantId, String token)
        //{
        //    var software = Session.CreateCriteria<AuthorizedSoftware>("AuthorizedSoftware")
        //                             .Add(Restrictions.Eq("AuthorizedSoftware.ParentTenant.Id", parentTenantId))
        //                             .Add(Restrictions.Eq("AuthorizedSoftware.Tenant.Id", tenantId))
        //                             .Add(Restrictions.Eq("AuthorizedSoftware.Token", token))
        //                             .UniqueResult<AuthorizedSoftware>();
        //    return software;
        //}

        public AuthorizedSoftware Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public AuthorizedSoftware Get(Guid parentTenantId, string token)
        {
            throw new NotImplementedException();
        }

        public AuthorizedSoftware Get(Guid parentTenantId, Guid tenantId, string token)
        {
            throw new NotImplementedException();
        }

        public IList<AuthorizedSoftware> GetAll()
        {
            throw new NotImplementedException();
        }

        public AuthorizedSoftware SaveOrUpdate(AuthorizedSoftware entity)
        {
            throw new NotImplementedException();
        }
    }
}
