using Facture.Core.Domain.DomainModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace Facture.IdentityAccess.Domain
{
    public class ContactInformation : ValueObject
    {
        public ContactInformation(
                String emailAddress,
                Address address,
                Phone primaryPhone,
                Phone secondaryPhone)
        {
            this.EmailAddress = emailAddress;
            this.Address = address;
            this.PrimaryPhone = primaryPhone;
            this.SecondaryPhone = secondaryPhone;
        }

        protected ContactInformation()
        {
        }

        [StringLength(maximumLength: 100)]
        public virtual String EmailAddress { get; set; }

        public virtual Address Address { get; set; }

        public virtual Phone PrimaryPhone { get; set; }

        public virtual Phone SecondaryPhone { get; set; }

        public virtual String CopyEmailAddress { get; set; }
    }
}
