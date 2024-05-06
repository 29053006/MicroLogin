using System;

namespace Facture.IdentityAccess.Contracts.Model
{
    public class Location
    {
        public String RegionName { get; set; }
        public String CityName { get; set; }
        public String CountryCode { get; set; }
        public String AddressLine { get; set; }
        public String PhoneNumber { get; set; }
    }
}
