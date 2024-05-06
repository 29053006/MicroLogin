using Facture.Core.Domain.DomainModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace Facture.IdentityAccess.Domain
{
    public class Phone : ValueObject
    {
        public virtual Nullable<Int32> CountryCode { get; set; }

        public virtual Nullable<Int32> AreaCode { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public virtual String Number { get; set; }

        public virtual Int32 Extension { get; set; }

        protected Phone() { }

        public Phone(Nullable<Int32> countryCode, Nullable<Int32> areaCode, String number)
        {
            this.CountryCode = countryCode;
            this.AreaCode = areaCode;
            this.Number = number;
        }

    }
}
