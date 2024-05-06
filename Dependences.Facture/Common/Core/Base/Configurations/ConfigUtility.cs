using Dependences.Facture.Contracts;
using Facture.Core.Domain;
using Facture.IdentityAccess.Application;
using Facture.IdentityAccess.Application.Components.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;

namespace Facture.Core.Base.Configurations
{
    public static class ConfigUtility
    {
        private static readonly ConcurrentDictionary<string, object> _CacheDictionary = new ConcurrentDictionary<string, object>();
        private static IConfigurationRoot _config;


        public static int HeadersGetInt(string key, int defaultValue = 0)
        {
            var stringValue = HeadersGetString(key);
            if (int.TryParse(stringValue, out int value))
            { return value; }
            else
            { return defaultValue; }
        }

        public static string HeadersGetString(string key, string defaultValue = null)
        {
            var stringValue = IdentityService.HttpContextAccessor.HttpContext.Request.Headers[key].FirstOrDefault();
            return stringValue ?? defaultValue;
        }


        public static string ConnectionStringsGet(string cnnStringName, bool required = true)
        {
            var cnnString = GetConnectionString(cnnStringName);
            if (cnnString == null)
            {
                if (required) { throw new ArgumentNullException("ConnectionStrings", $"Missing ConnectionString: '{cnnStringName}'"); }
                else { return null; }
            }

            return cnnString;
        }

        public static string ConnectionStringsGet(string cnnStringName, string fallbackCnnStringName)
        {
            var cnnString = GetConnectionString(cnnStringName) ?? GetConnectionString(fallbackCnnStringName);

            if (cnnString == null)
            { throw new ArgumentNullException(nameof(cnnStringName), $"Missing ConnectionString '{cnnStringName}' or '{fallbackCnnStringName}'"); }

            return cnnString;
        }


        public static string CnnStringOrAppSettings(string cnnStringName, string appSettingKey)
        {
            var cacheKey = $"{cnnStringName}|{appSettingKey}";

            return (string)_CacheDictionary.GetOrAdd(cacheKey, _ =>
            {
                // search connectionStrings first using cnnStringName
                var value = ConnectionStringsGet(cnnStringName, required: false);
                // search appSettings using appSettingKey
                if (string.IsNullOrWhiteSpace(value)) { value = GetSetting(appSettingKey); }
                // raise error if not found on any
                if (string.IsNullOrWhiteSpace(value)) { throw new ArgumentNullException(cacheKey); }

                return value;
            });
        }

        public static string AppSettingsToStringWithFallback(string key, string fallbackKey, string defaultValue = null)
        {
            var value = AppSettingsToString(key, required: false);
            if (string.IsNullOrWhiteSpace(value)) { value = AppSettingsToString(fallbackKey, defaultValue, required: false); }
            // valor requerido
            if (string.IsNullOrWhiteSpace(value)) { throw new ArgumentNullException(key); }
            return value;
        }


        public static string AppSettingsToString(string key, string defaultValue = null, bool required = true)
        {
            return (string)_CacheDictionary.GetOrAdd(key, _ =>
            {
                string stringValue = GetSetting(key);
                string value;
                if (string.IsNullOrWhiteSpace(stringValue))
                {
                    if (required) { throw new ArgumentNullException(key); }
                    value = defaultValue;
                }
                else
                { value = stringValue; }

                return value;
            });
        }

        public static int AppSettingsToInt32(string key, int? defaultValue = null, int? minValue = null)
        {
            return (int)_CacheDictionary.GetOrAdd(key, _ =>
            {
                string stringValue = GetSetting(key);
                int value;
                if (string.IsNullOrWhiteSpace(stringValue))
                { value = defaultValue ?? throw new ArgumentNullException(key); }
                else
                { value = Convert.ToInt32(stringValue); }

                if (minValue.HasValue && value < minValue) { throw new ArgumentOutOfRangeException(key, $"AppSetting '{key}' value cannot be less than '{minValue.Value}'"); }

                return value;
            });
        }

        public static double AppSettingsToDouble(string key, double defaultValue = 0)
        {
            var valueString = GetSetting(key);

            if (string.IsNullOrEmpty(valueString))
            { return defaultValue; }
            else if (double.TryParse(valueString, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
            { return value; }
            else
            { throw new ArgumentOutOfRangeException(key, valueString, SN.CannotConvertToNumber); }
        }

        public static decimal AppSettingsToDecimal(string key, decimal? minValue = null, decimal? defaultValue = null)
        {
            return (decimal)_CacheDictionary.GetOrAdd(key, _ =>
            {
                string stringValue = GetSetting(key);
                decimal value;
                if (string.IsNullOrWhiteSpace(stringValue))
                { value = defaultValue ?? throw new ArgumentNullException(key); }
                else
                { value = Convert.ToDecimal(stringValue); }

                if (minValue.HasValue && value < minValue) { throw new ArgumentOutOfRangeException(key, $"AppSetting '{key}' value cannot be less than '{minValue.Value}'"); }

                return value;
            });
        }


        public static bool AppSettingsToBoolean(string key, string defaultValue = "0")
        {
            return (bool)_CacheDictionary.GetOrAdd(key, _ =>
            {
                string stringValue = (GetSetting(key) ?? defaultValue);
                bool value = stringValue == "1" || stringValue.ToUpper() == "TRUE";
                return value;
            });
        }

        public static bool AppSettingsToBoolean(string key, bool required)
        {
            var stringValue = string.Empty;

            return (bool)_CacheDictionary.GetOrAdd(key, _ =>
            {
                string stringValue = GetSetting(key);
                if (string.IsNullOrEmpty(stringValue) && required)
                {
                    throw new ArgumentNullException(key);
                }
                bool value = stringValue == "1" || stringValue?.ToUpper() == "TRUE";
                return value;
            });
        }


        public static string ResolveVariableWithAppSetting(string name, string replaceName)
        {
            var value = GetSetting(name);
            Check.NotNull(value, $"Cannot find AppSetting '{name}'");
            return ResolveVariableWithAppSettingKey(value, replaceName);
        }


        public static string ResolveVariableWithAppSettingKey(string value, string replaceName)
        {
            var replaceSetting = GetSetting(replaceName);
            Check.NotNull(replaceSetting, $"Cannot find AppSetting '{replaceName}'");
            return value.Replace($"{{{replaceName}}}", replaceSetting);
        }

        public static string GetSetting(string value)
        {
            if (_config == null)
                _config = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.json", optional: true)
                 .AddEnvironmentVariables()
                 .Build();

            return _config.GetSection($"AppSettings:{value}").Value;
        }

        public static string GetConnectionString(string value)
        {
            if (_config == null)
                _config = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.json", optional: true)
                 .AddEnvironmentVariables()
                 .Build();

            return _config.GetSection($"connectionStrings:{value}").Value;
        }
    }
}