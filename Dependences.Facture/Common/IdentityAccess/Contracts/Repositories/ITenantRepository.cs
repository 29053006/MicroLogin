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
        Tenant GetByTenantId(Guid tenantId, bool cacheable = true, bool readOnly = true);

        Tenant GetCurrent();
        Tenant GetCurrentRequired();
        Tenant GetByTenantName(string tenantName, bool cacheable = true, bool readOnly = true);
        Tenant Get(string tenantName, bool cacheable = true, bool readOnly = true);

        IEnumerable<Tenant> GetByParentId(Guid parentTenantId);

        string GetTenantSettingValueByName(Guid tenantId, string name);
        string GetRolSSOByTenantAndRol(string TenantName, string RolCode);
        Tenant GetFactureBox(bool cacheable = true, bool readOnly = true);
        bool TermsAndConditions_Get_Clause_2_0_Status(Guid parentId, string contractName);

        Tenant GetFacture(bool cacheable = true, bool readOnly = true);
        Tenant GetThirdPortal(bool cacheable = true, bool readOnly = true);

        bool HasBillforceService(string name);
        bool HasBillforceDocumentosService(string name);
        bool HasBillforceServiceIssue(string name);
        bool MostrarMenuBillforce();
        string MostrarOpcionesMenuBillforce();
        long getUnitsConsumedToday(string tokenContrato);
        Guid saveJtwExpired(string userName, string jwtContrato);
        bool genericUser(string userName, Guid tenantId);

        Task<IList<RazonSocialFactureModel>> ConsultarRazonSocialFacture(string contratos);
        void ActualizarRazonSocialFacture(string tenantName, string firstName, string middleName, string firstSurName, string secondSurName);
    }
}
