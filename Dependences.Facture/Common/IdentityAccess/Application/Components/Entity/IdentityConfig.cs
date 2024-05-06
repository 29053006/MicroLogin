using Facture.Core.Base.Configurations;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Facture.IdentityAccess.Application.Components.Entity
{
    public static class IdentityConfig
    {
        public static readonly ReadOnlyCollection<string> BlackListRoles = new ReadOnlyCollection<string>(new List<string> { Roles.Subscribers, Roles.TranslatorUS, Roles.UnverifiedUsers });

        public static class LocalStorage
        {
            public static readonly string ScopeTenantId = "SCOPE_TENANTID_STORAGE";
        }

        public static class Roles
        {
            public static readonly string Administrator = "Administrator";
            public static readonly string Subscribers = "Subscribers";
            public static readonly string TranslatorUS = "Translator (en-US)";
            public static readonly string UnverifiedUsers = "Unverified Users";
        }

        public static class UserSettings
        {
            public static readonly string Branches = "Branches";
            public static readonly string Processes = "Processes";
            public static readonly string Roles = "Roles";
            public static readonly string ProfilePicture = "profilepicture";
        }

        public static class TenantSettings
        {
            public static readonly string TaxCategory = "TaxCategory";
        }

        public static class AppSettings
        {
            public static class Url
            {
                public static string ApiRoot { get => ConfigUtility.AppSettingsToString("Url.ApiRoot", required: false); }
            }

            public static class Platform
            {
                public static string DefaultTenant { get => ConfigUtility.AppSettingsToString("Platform.DefaultTenant"); }
            }

            public static class IdentityAccess
            {
                public static int ParentSessionTokenTTLInHours { get => ConfigUtility.AppSettingsToInt32("IdentityAccess.ParentSessionTokenTTLInHours", defaultValue: 1); }
                public static int SessionTokenTTLInHours { get => ConfigUtility.AppSettingsToInt32("IdentityAccess.SessionTokenTTLInHours", defaultValue: 1); }
                public static int ParentSessionTokenTTLInMonths { get => ConfigUtility.AppSettingsToInt32("IdentityAccess.ParentSessionTokenTTLInMonths", defaultValue: 12); }
                public static int SessionTokenTTLInMonths { get => ConfigUtility.AppSettingsToInt32("IdentityAccess.SessionTokenTTLInMonths", defaultValue: 3); }
                public static int PasswordHistoryEntries { get => ConfigUtility.AppSettingsToInt32("IdentityAccess.PasswordHistoryEntries", defaultValue: 10); }
                public static double PasswordTTLInDays { get => ConfigUtility.AppSettingsToDouble("IdentityAccess.PasswordTTLInDays", defaultValue: 4 * 30); }
                public static string SSOType { get => "SSO"; }
            }
        }

        public static class ConnectionStrings
        {
            public static string FileStorage { get => ConfigUtility.ConnectionStringsGet("Files.Storage.ConnectionString"); }
        }

        public static class Storage
        {
            public static readonly string DefaultContainer = "files";
        }
    }
}
