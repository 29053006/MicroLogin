using Facture.Core.Domain.DomainModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace Facture.IdentityAccess.Domain
{
    public class Address : Entity
    {
        [StringLength(maximumLength: 500)]
        public virtual String AddressLine { get; set; }

        [StringLength(maximumLength: 500)]

        public virtual String AddressLine2 { get; set; }

        //[Required]
        [StringLength(maximumLength: 100)]
        public virtual String CityCode { get; set; }

        //[Required]
        [StringLength(maximumLength: 100)]
        public virtual String CountryCode { get; set; }

        // [Required]
        [StringLength(maximumLength: 100)]
        public virtual String Region { get; set; }

        //[Required]
        //[StringLength(maximumLength: 2, MinimumLength = 6)]
        [StringLength(maximumLength: 30)]
        public virtual String PostalCode { get; set; }

        [StringLength(maximumLength: 100)]
        public virtual String Neighborhood { get; set; }

        [StringLength(maximumLength: 6)]
        public virtual string DistrictCode { get; set; }

        [StringLength(maximumLength: 6)]
        public virtual string UbietyCode { get; set; }

        protected Address() { }

        public Address(String addressLine, String cityCode, String countryCode, String region, String postalCode, String districtCode = null, String ubietyCode = null)
        {
            this.AddressLine = addressLine;
            this.CityCode = cityCode;
            this.CountryCode = countryCode;
            this.Region = region;
            this.PostalCode = postalCode;
            this.DistrictCode = districtCode;
            this.UbietyCode = ubietyCode;
        }

        public Address(String addressLine, String cityCode, String countryCode, String region, String postalCode, String neighborhood)
        {
            this.AddressLine = addressLine;
            this.CityCode = cityCode;
            this.CountryCode = countryCode;
            this.Region = region;
            this.PostalCode = postalCode;
            this.Neighborhood = neighborhood;
        }
    }
}
