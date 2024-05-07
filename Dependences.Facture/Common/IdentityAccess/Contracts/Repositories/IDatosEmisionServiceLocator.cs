using FactureIndentityDomain = Facture.IdentityAccess.Domain;

namespace Facture.IdentityAccess.Contracts.Repositories
{
    public  interface IDatosEmisionServiceLocator
    {
        /// <summary>
        /// WARNING: Este metodo solo aplica para:
        /// Facture.IdentityAccess.Infrastructure.Repositories => TenantRepository
        /// </summary>
        /// <returns></returns>
        FactureIndentityDomain.Tenant GetCurrentRequired();

        /// <summary>
        /// WARNING: Este metodo solo aplica para:
        /// PLColab.Core.Infrastructure.Repositories => TenantRepository
        /// </summary>
        /// <returns></returns>
        T FindSettingOnlyTenant<T>(string settingName, bool required = true);

        /// <summary>
        /// WARNING: Este metodo solo aplica para:
        /// PLColab.Core.Infrastructure.Repositories => TenantRepository
        /// </summary>
        /// <returns></returns>
        T FindSetting<T>(string settingName, bool required = true);


        /// <summary>
        /// WARNING: Este metodo solo aplica para:
        /// PLColab.Core.Issue.Application.Services => CanNotIssueService
        /// </summary>
        /// <returns></returns>
        void IsNotIssue();

        /// <summary>
        /// WARNING: Este metodo solo aplica para:
        /// PLColab.Core.Infrastructure.Repositories => TenantRepository
        /// </summary>
        /// <returns></returns>
        FactureIndentityDomain.Tenant GetIsolated(string tenantName);


        /// <summary>
        /// WARNING: Este metodo solo aplica para:
        /// PLColab.Core.Infrastructure.Repositories => TenantRepository
        /// </summary>
        /// <returns></returns>
        T FindSettingIsolated<T>(Guid tenantId, Guid? parentTenantId, string settingName, bool required = true);


        /// <summary>
        /// WARNING: Este metodo solo aplica para:
        /// PLColab.Core.Infrastructure.Repositories => TenantRepository
        /// </summary>
        /// <returns></returns>
        FactureIndentityDomain.Tenant Get(string tenantName);

        /// <summary>
        /// WARNING: Este metodo solo aplica para:
        /// Facture.IdentityAccess.Infrastructure.Repositories => TenantRepository
        /// </summary>
        /// <returns></returns>
        FactureIndentityDomain.Tenant Get(String tenantName, bool cacheable = true, bool readOnly = true);


        String FindSetting(Guid tenantId, String tenantName, String settingName, Boolean required = true, Boolean isGetCache = true);

        string GetTenantPeopleEmail(string tenantName);
    }
}
