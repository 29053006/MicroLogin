using Dependences.Facture.Common.Core.Domain;
using Dependences.Facture.Contracts;
using Facture.IdentityAccess.Application;
using Facture.IdentityAccess.Application.Components.Entity;
using Facture.IdentityAccess.Contracts.Repositories;
using Facture.IdentityAccess.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Facture.IdentityAccess.Contracts
{
    public static class TenantSettingActions
    {
        private static TenantSetting Retrieve(this ISet<TenantSetting> collection, string key)
            => collection.FirstOrDefault(t => t.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase));

        private static TenantSetting RetrieveRequired(this ISet<TenantSetting> collection, string key)
        {
            var setting = collection.Retrieve(key);
            Check.NotNull(setting, string.Format(SN.RecordNotFoundWithKey, key));

            return setting;
        }

        public static (bool Found, TenantSetting Setting) TryToRetrieve(this ISet<TenantSetting> collection, string key)
        {
            var setting = collection.Retrieve(key);
            return (setting != null)
                    ? (true, setting)
                    : (false, null);
        }

        public static TenantSetting Retrieve(this ISet<TenantSetting> collection, string key, bool fallbackToParentSettings)
        {
            if (!fallbackToParentSettings) { return collection.RetrieveRequired(key: key); }

            // FALLBACK: 1-current tenant
            TenantSetting setting = collection.Retrieve(key);
            if (setting != null) { return setting; }

            // FALLBACK: 2-parent tenant
            var parent = collection.FirstOrDefault()?.Tenant?.Parent;
            if ((setting = parent?.Settings?.Retrieve(key)) != null) { return setting; }

            // FALLBACK: 3-system default tenant
            var tenantRepository = SafeServiceLocator<ITenantRepository>.GetService();
            var defaultTenant = tenantRepository.Get(IdentityConfig.AppSettings.Platform.DefaultTenant);
            return defaultTenant.Settings.RetrieveRequired(key);
        }

        private static Boolean ContainsKey(this ISet<TenantSetting> collection, String key)
        {
            return collection.Any(t => t.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase));
        }

        public static Boolean ContainsKey(this ISet<TenantSetting> collection, String key, Boolean fallbackToParentSettings)
        {
            if (collection.ContainsKey(key: key))
            {
                return true;
            }
            else if (fallbackToParentSettings)
            {
                var parent = collection.FirstOrDefault()?.Tenant?.Parent;
                return (parent == null) ? false : parent.Settings.ContainsKey(key: key);
            }
            return false;
        }

        public static void RemoveWithKey(this ISet<TenantSetting> collection, String key)
        {
            var setting = collection.FirstOrDefault(t => t.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase));
            Check.NotNull(setting, string.Format(SN.RecordNotFoundWithKey, key));

            collection.Remove(item: setting);
        }
    }
}
