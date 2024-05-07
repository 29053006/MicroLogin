using Newtonsoft.Json;
using System;

namespace Facture.IdentityAccess.Application.Components.Entity
{
    [JsonObject]
    public class LoginResultData
    {       
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }
        
        [JsonProperty("tenantId")]
        public Guid TenantId { get; set; }

        [JsonIgnore]
        public string Error { get; set; }
    }
}
