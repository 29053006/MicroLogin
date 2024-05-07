using Dependences.Facture.Contracts;
using Facture.Core.Domain;
using Facture.IdentityAccess.Application;
using Facture.IdentityAccess.Application.Components.Entity;
using Facture.IdentityAccess.Application.Components.Services;
using Facture.IdentityAccess.Contracts;
using Facture.IdentityAccess.Contracts.Model;
using Facture.IdentityAccess.Contracts.Repositories;
using Facture.IdentityAccess.Domain;
using System;
using System.Linq;

namespace Facture.IdentityAccess.Infrastructure.AuthEnrichers
{
    public class SupplierEnrichToken : ITokenEnrichService
    {
        private readonly IUserRepository _userRepository;

        public SupplierEnrichToken(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void Enrich(Guid tenantId, String tenantName, ref JwtIdentity jwt)
        {
            Check.NotEmpty(() => tenantName, "La propiedad 'tenantName' no puede estar vacía");
            Check.NotNull(jwt, SN.TokenMustNotBeNull);

            var user = _userRepository.UserWithUsername(tenantId: tenantId, username: tenantName);
            Check.NotNull(() => user, "La propiedad 'user' no puede ser null");

            jwt.Identification.ThirdPartyIdentification = user.Username;
            jwt.Identification.ThirdPartyName = user.Person.Name.FirstName;

            jwt.Email = user.Person?.ContactInformation?.EmailAddress;

            jwt.Location.CountryCode = user.Person?.ContactInformation?.Address?.CountryCode;
            jwt.Location.RegionName = user.Person?.ContactInformation?.Address?.Region;
            jwt.Location.CityName = user.Person?.ContactInformation?.Address?.CityCode;
            jwt.Location.AddressLine = user.Person?.ContactInformation?.Address?.AddressLine;
            jwt.Location.PhoneNumber = user.Person?.ContactInformation?.PrimaryPhone?.Number;
            jwt.TaxCategory = user.Tenant?.Settings.Retrieve(key: IdentityConfig.TenantSettings.TaxCategory, fallbackToParentSettings: true).Value;

            jwt.Features = String.Join(",", user.Tenant.Features.ToArray());
        }
    }
}
