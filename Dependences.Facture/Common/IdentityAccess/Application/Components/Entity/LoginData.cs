using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Facture.IdentityAccess.Application.Components.Entity
{
    /// <summary>
    /// Class used for the Login to obtain a Json Web Token (JWT).
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class LoginData
    {
        [Required]
        [RegularExpression(pattern: "[a-zA-Z_-]")]
        [JsonProperty("u")]
        public String Username;

        [Required]
        [JsonProperty("p")]
        public String Password;

        [JsonProperty("t")]
        public Guid? TenantId;

        [JsonProperty("pi")]
        public Guid? ProviderIssuerId;

        [JsonProperty("i")]
        public Guid? InNamedOfTenantId;

        [JsonProperty("in")]
        public String InNamedOfTenantNumber;

        [StringLength(maximumLength: 36)]
        [JsonProperty("s")]
        public String Token;

        [JsonProperty("ip")]
        public String IP;
    }
}
