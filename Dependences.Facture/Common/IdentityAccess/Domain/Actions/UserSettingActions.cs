using Dependences.Facture.Contracts;
using Facture.Core.Domain;
using Facture.IdentityAccess.Application;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Facture.IdentityAccess.Domain
{
    public static class UserSettingActions
    {
        [Obsolete]
        public static String WithKey(this ISet<UserSetting> collection, String key)
        {
            var setting = collection.FirstOrDefault(t => t.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase));
            Check.NotNull(setting, string.Format(SN.RecordNotFoundWithKey, key));

            return setting.Value;
        }

        public static UserSetting Retrieve(this ISet<UserSetting> collection, String key)
        {
            var setting = collection.FirstOrDefault(t => t.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase));
            Check.NotNull(setting, string.Format(SN.RecordNotFoundWithKey, key));

            return setting;
        }

        public static Boolean ContainsKey(this ISet<UserSetting> collection, String key)
        {
            return collection.Any(t => t.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase));
        }

        public static void RemoveWithKey(this ISet<UserSetting> collection, String key)
        {
            var setting = collection.FirstOrDefault(t => t.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase));
            Check.NotNull(setting, string.Format(SN.RecordNotFoundWithKey, key));

            collection.Remove(item: setting);
        }

        public static UserSetting WithValue(this UserSetting setting, String value)
        {
            setting.Value = value;
            return setting;
        }
    }
}
