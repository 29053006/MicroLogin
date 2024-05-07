using Dependences.Facture.Common.IdentityAccess.Contracts.Repositories;
using Dependences.Facture.Contracts;
using Facture.Core.Domain.PersistenceSupport;
using Facture.IdentityAccess.Application.Components.Services;
using Facture.IdentityAccess.Contracts.Repositories;
using Facture.IdentityAccess.Domain;


namespace Facture.IdentityAccess.Infrastructure.Repositories
{
    public class TenantRepository : ITenantRepository, IDatosEmisionServiceLocator
    {
        public IDbContext DbContext => throw new NotImplementedException();

        public Tenant GetCurrentRequired()
        {
            var tenantId = IdentityService.ScopeTenantId;
            var tenant = Get(tenantId);
            Check.NotNull(tenant, $"Tenant '{tenantId}' does not exist.");
            return tenant;
        }


        //public virtual Tenant Get(String tenantName, bool cacheable = true, bool readOnly = true)
        //{
        //    var tenant = Session.CreateCriteria<Tenant>("Tenant")
        //                         .Add(Restrictions.Eq("Tenant.Name", tenantName))
        //                         .SetCacheable(cacheable)
        //                         .SetFlushMode(NHibernate.FlushMode.Manual)
        //                         .SetReadOnly(readOnly)
        //                         .UniqueResult<Tenant>();

        //    return tenant;
        //}


        //public String GetTenantSettingValueByName(Guid tenantId, String name)
        //{
        //    //consulta creada con el objeto de obtener el value de un setting especifico cuando
        //    //se utiliza un usertype
        //    var value = Session.CreateSQLQuery(@"SELECT TOP(1) t.Value 
        //                                         FROM auth.TenantSettings t                                                             
        //                                         WHERE t.Name = :Name 
        //                                         AND t.TenantId = :tenantId")
        //                       .SetString("Name", name)
        //                       .SetGuid("tenantId", tenantId)
        //                       .UniqueResult<String>();
        //    return value;
        //}

        public Tenant Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public IList<Tenant> GetAll()
        {
            throw new NotImplementedException();
        }

        public Tenant SaveOrUpdate(Tenant entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Tenant entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public T FindSettingOnlyTenant<T>(string settingName, bool required = true)
        {
            throw new NotImplementedException();
        }

        public T FindSetting<T>(string settingName, bool required = true)
        {
            throw new NotImplementedException();
        }

        public void IsNotIssue()
        {
            throw new NotImplementedException();
        }

        public Tenant GetIsolated(string tenantName)
        {
            throw new NotImplementedException();
        }

        public T FindSettingIsolated<T>(Guid tenantId, Guid? parentTenantId, string settingName, bool required = true)
        {
            throw new NotImplementedException();
        }

        public Tenant Get(string tenantName)
        {
            throw new NotImplementedException();
        }

        public string FindSetting(Guid tenantId, string tenantName, string settingName, bool required = true, bool isGetCache = true)
        {
            throw new NotImplementedException();
        }

        public string GetTenantPeopleEmail(string tenantName)
        {
            throw new NotImplementedException();
        }

        public Tenant Get(string tenantName, bool cacheable = true, bool readOnly = true)
        {
            throw new NotImplementedException();
        }

        public string GetTenantSettingValueByName(Guid tenantId, string name)
        {
            throw new NotImplementedException();
        }
    }
}
