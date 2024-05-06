using Facture.IdentityAccess.Domain;
using System;
using System.Runtime.Serialization;
using System.Security.Principal;

namespace Facture.IdentityAccess.Contracts.Model
{
    [DataContract]
    public class JwtIdentity : GenericIdentity
    {
        [DataMember]
        public Guid TenantId { get; set; }

        [DataMember]
        public String TenantName { get; set; }

        [DataMember]
        public String TenantDescription { get; set; }

        [DataMember]
        public Guid ParentTenantId { get; set; }

        [DataMember]
        public String ParentTenantName { get; set; }
       
        [DataMember]
        public Identification Identification { get; set; }

        [DataMember]
        public Location Location { get; set; }

        [DataMember]
        public String TaxCategory { get; set; }

        [DataMember]
        public String Features { get; set; }

        [DataMember]
        public String Email { get; set; }

        [DataMember]
        public Guid? ProviderIssuer { get; set; }

        // WARNING: This is no data member
        [IgnoreDataMember]
        public Guid ScopeTenantId { get; set; }

        [DataMember]
        public Boolean IsWeb { get; set; }

        [DataMember]
        public String Rol { get; set; }

        public JwtIdentity() : base(name: String.Empty) { }

        public JwtIdentity(String username, Guid tenantId, String tenantName, String tenantDescription, String authenticationType, Guid? providerIssuer = null, String rol=null)
            : base(name: username, type: authenticationType)
        {
            this.TenantId = tenantId;
            this.TenantName = tenantName;
            this.TenantDescription = tenantDescription;
            this.Identification = new Identification();
            this.Location = new Location();
            this.ProviderIssuer = providerIssuer;
            this.Rol = rol;
        }

        public JwtIdentity(Tenant tenant) : this(username: tenant.Name, tenantId: tenant.Id, tenantName: tenant.Name, tenantDescription: tenant.Description, authenticationType: "JWT") { }

        public JwtIdentity(Tenant tenant,string xrefSource = null) : this(username: tenant.Name, tenantId: tenant.Id, tenantName: tenant.Name, tenantDescription: tenant.Description, authenticationType: "JWT") { }

        public JwtIdentity(User user) : this(username: user.Username, tenantId: user.Tenant.Id, tenantName: user.Tenant.Name, tenantDescription: user.Tenant.Description, authenticationType: "JWT")
        {
            var taxCategory = user.Tenant?.Settings.Retrieve(key: "TaxCategory", fallbackToParentSettings: true).Value;

            Identification = new Identification
            {
                ThirdPartyIdentification = user.Username,
                ThirdPartyName = user.Person.Name.FirstName,
            };
            Location = new Location
            {
                CountryCode = user.Person?.ContactInformation?.Address?.CountryCode,
                RegionName = user.Person?.ContactInformation?.Address?.Region,
                CityName = user.Person?.ContactInformation?.Address?.CityCode,
                AddressLine = user.Person?.ContactInformation?.Address?.AddressLine,
                PhoneNumber = user.Person?.ContactInformation?.PrimaryPhone?.Number
            };
            Email = user.Person?.EmailAddress;
            TaxCategory = taxCategory;
        }

        public JwtIdentity(User user, string taxCategory = null) : this(username: user.Username, tenantId: user.Tenant.Id, tenantName: user.Tenant.Name, tenantDescription: user.Tenant.Description, authenticationType: "JWT")        {
           

            Identification = new Identification
            {
                ThirdPartyIdentification = user.Username,
                ThirdPartyName = user.Person.Name.FirstName,
            };
            Location = new Location
            {
                CountryCode = user.Person?.ContactInformation?.Address?.CountryCode,
                RegionName = user.Person?.ContactInformation?.Address?.Region,
                CityName = user.Person?.ContactInformation?.Address?.CityCode,
                AddressLine = user.Person?.ContactInformation?.Address?.AddressLine,
                PhoneNumber = user.Person?.ContactInformation?.PrimaryPhone?.Number
            };
            Email = user.Person?.EmailAddress;
            TaxCategory = taxCategory;
        }

    }
}
