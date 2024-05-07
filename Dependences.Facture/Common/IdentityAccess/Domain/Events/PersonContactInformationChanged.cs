using Facture.Core.Domain.Events;
using System;

namespace Facture.IdentityAccess.Domain.Events
{
    public class PersonContactInformationChanged : IDomainEvent
    {
        public PersonContactInformationChanged(
               Tenant tenant,
               String username,
               ContactInformation contactInformation)
        {
            this.ContactInformation = contactInformation;
            this.EventVersion = 1;
            this.OccurredOn = DateTimeOffset.Now;
            this.Tenant = tenant;
            this.Username = username;
        }

        public ContactInformation ContactInformation { get; private set; }

        public Int32 EventVersion { get; set; }

        public DateTimeOffset OccurredOn { get; set; }

        public Tenant Tenant { get; private set; }

        public String Username { get; private set; }
    }
}
