using Dependences.Facture.Contracts;
using Facture.IdentityAccess.Contracts.Model;
using Facture.IdentityAccess.Domain;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MicroLogin.Service
{
    public class CreateJwtServices : ICreateJwtServices
    {
        public const string TenantIdClaimType = "sub";
        public const string TenantNameClaimType = "name";
        public const string TenantDescriptionClaimType = "desc";
        public const string ParentTenantIdClaimType = "ptid";
        public const string UsernameClaimType = "usr";
        public const string ThirdPartyIdentificationClaimType = "3id";
        public const string ThirdPartyNameClaimType = "3name";
        public const string CountryCodeClaimType = "ccod";
        public const string RegionNameClaimType = "reg";
        public const string CityNameClaimType = "city";
        public const string AddressLineClaimType = "addr";
        public const string PhoneClaimType = "phone";
        public const string TaxCategoryClaimType = "tax";
        public const string EmailClaimType = "mail";
        public const string ProviderIssuerClaimType = "pi";
        public const string IP = "ip";
        public const string Rol = "rol";
        public const string IsWeb = "isweb";
        public JwtSecurityToken CreateJwtToken(String issuer, Guid tenantId, String username, DateTime expires, JwtIdentity identity, byte[] saltKey, string ip = null, bool isWeb = false, string rol = null)
        {
            Check.Require(tenantId != Guid.Empty, "TenantId should not be null.");
            Check.NotEmpty(() => identity.TenantName, "La propiedad 'identity.TenantName' no puede estar vacía");
            Check.NotEmpty(() => username, "La propiedad 'username' no puede estar vacía");
            Check.NotEmpty(() => identity.Identification.ThirdPartyIdentification, "La propiedad 'identity.Identification.ThirdPartyIdentification' no puede estar vacía");
            Check.NotEmpty(() => identity.Identification.ThirdPartyName, "La propiedad 'identity.Identification.ThirdPartyName' no puede estar vacía");
            Check.NotEmpty(() => identity.Email, "La propiedad 'identity.Email' no puede estar vacía");
            Check.NotEmpty(() => identity.Location.CountryCode, "La propiedad 'identity.Location.CountryCode' no puede estar vacía");
            Check.NotEmpty(() => identity.Location.RegionName, "La propiedad 'identity.Location.RegionName' no puede estar vacía");
            Check.NotEmpty(() => identity.Location.CityName, "La propiedad 'identity.Location.CityName' no puede estar vacía");
            Check.NotEmpty(() => identity.Location.AddressLine, "La propiedad 'identity.Location.AddressLine' no puede estar vacía");

            Check.NotEmpty(() => identity.TaxCategory, "La propiedad 'identity.TaxCategory' no puede estar vacía");

            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(TenantIdClaimType, Convert.ToString(tenantId)));
            claimsIdentity.AddClaim(new Claim(TenantNameClaimType, identity.TenantName));
            claimsIdentity.AddClaim(new Claim(TenantDescriptionClaimType, identity.TenantDescription));
            claimsIdentity.AddClaim(new Claim(UsernameClaimType, username));
            claimsIdentity.AddClaim(new Claim(ThirdPartyIdentificationClaimType, identity.Identification.ThirdPartyIdentification));
            claimsIdentity.AddClaim(new Claim(ThirdPartyNameClaimType, identity.Identification.ThirdPartyName));
            claimsIdentity.AddClaim(new Claim(CountryCodeClaimType, identity.Location.CountryCode));
            claimsIdentity.AddClaim(new Claim(RegionNameClaimType, identity.Location.RegionName));
            claimsIdentity.AddClaim(new Claim(CityNameClaimType, identity.Location.CityName));
            claimsIdentity.AddClaim(new Claim(AddressLineClaimType, identity.Location.AddressLine));
            claimsIdentity.AddClaim(new Claim(TaxCategoryClaimType, identity.TaxCategory));
            claimsIdentity.AddClaim(new Claim(EmailClaimType, identity.Email));
            claimsIdentity.AddClaim(new Claim(IsWeb, Convert.ToString(isWeb)));

            if (!string.IsNullOrEmpty(ip))
            { claimsIdentity.AddClaim(new Claim(IP, ip)); }

            if (!string.IsNullOrEmpty(rol))
            {
                claimsIdentity.AddClaim(new Claim(Rol, rol));
            }

            if (identity.ProviderIssuer != Guid.Empty)
            { claimsIdentity.AddClaim(new Claim(ProviderIssuerClaimType, Convert.ToString(identity.ProviderIssuer))); }

            if (identity.ParentTenantId != Guid.Empty)
            { claimsIdentity.AddClaim(new Claim(ParentTenantIdClaimType, Convert.ToString(identity.ParentTenantId))); }

            if (!string.IsNullOrEmpty(identity.Location.PhoneNumber))
            {
                claimsIdentity.AddClaim(new Claim(PhoneClaimType, identity.Location.PhoneNumber));
            }

            var credentials = new SigningCredentials(new SymmetricSecurityKey(saltKey), SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(issuer, null, claimsIdentity, notBefore: null, expires, DateTime.UtcNow, credentials);
            return token;
        }
    }
}
