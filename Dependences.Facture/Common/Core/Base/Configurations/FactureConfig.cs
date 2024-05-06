namespace Facture.Core.Base.Configurations
{
    public static class FactureConfig
    {
        public const string TOKEN_FILE_STORAGE_URI = "Files.Storage.Uri";

        public static class Headers
        {
            public static readonly string Key_XTraceMinElapsedMsec = "X-TRACE-MIN-ELAPSED-MSEC";
            public static int XTraceMinElapsedMsec { get => ConfigUtility.HeadersGetInt(Key_XTraceMinElapsedMsec); }
        }

        public static class AppSettings
        {
            public static class App
            {
                public static string SaltKey { get => ConfigUtility.AppSettingsToString("App.SaltKey", required: false); }
            }

            public static class NHibernate
            {
                public static bool ProfilerEnabled { get => ConfigUtility.AppSettingsToBoolean("NHibernate.ProfilerEnabled"); }
            }

            public static class Telerik
            {
                public static class AsyncUpload
                {
                    public static string ConfigurationEncryptionKey { get => ConfigUtility.AppSettingsToString("Telerik.AsyncUpload.ConfigurationEncryptionKey", required: false); }
                }
            }
        }
    }
}
