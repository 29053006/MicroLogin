using Dependences.Facture.Contracts;
using Facture.Core.Domain;
using Facture.Core.Domain.DomainModel;
using System;

namespace Facture.IdentityAccess.Domain
{
    public class Person : Entity
    {
        #region Constructor

        private FullName name;

        private ContactInformation contactInformation;

        /// <summary>
        /// Initializes a new instance of the <see cref="Person"/> class.
        /// </summary>
        /// <param name="tenantId">
        /// Initial value of the <see cref="TenantId"/> property.
        /// </param>
        /// <param name="name">
        /// Initial value of the <see cref="Name"/> property.
        /// </param>
        /// <param name="contactInformation">
        /// Initial value of the <see cref="ContactInformation"/> property.
        /// </param>
        public Person(Tenant tenantId, FullName name, ContactInformation contactInformation)
        {
            // Defer validation to the property setters.
            this.ContactInformation = contactInformation;
            this.Name = name;
            this.Tenant = tenantId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Person"/> class for a derived type,
        /// and otherwise blocks new instances from being created with an empty constructor.
        /// </summary>
        public Person() { }
        #endregion


        // ERROR: Lazy or no-proxy property Facture.IdentityAccess.Domain.Person.Tenant is not an auto property, which may result in uninitialized property access
        public virtual Tenant Tenant { get; set; }

        public virtual FullName Name
        {
            get
            {
                return this.name;
            }

            set
            {
                Check.NotNull(value, "The person name is required.");
                this.name = value;
            }
        }

        public virtual User User { get; set; }

        public virtual ContactInformation ContactInformation
        {
            get
            {
                return this.contactInformation;
            }

            set
            {
                Check.NotNull(value, "The person contact information is required.");
                this.contactInformation = value;
            }
        }

        public virtual String EmailAddress
        {
            get { return this.ContactInformation.EmailAddress; }
        }

        public virtual String LegalConstitution { get; set; }
    }
}
