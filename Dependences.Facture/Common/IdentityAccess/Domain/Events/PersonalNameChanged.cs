using Facture.Core.Domain.Events;
using System;

namespace Facture.IdentityAccess.Domain.Events
{
    public class PersonalNameChanged : IDomainEvent
    {
        public PersonalNameChanged(
                Tenant tenant,
                String username,
                FullName name)
        {
            this.EventVersion = 1;
            this.Name = name;
            this.OccurredOn = DateTimeOffset.Now;
            this.Tenant = tenant;
            this.Username = username;
        }

        public Int32 EventVersion { get; set; }

        public FullName Name { get; private set; }

        public DateTimeOffset OccurredOn { get; set; }

        public Tenant Tenant { get; private set; }

        public String Username { get; private set; }
    }
}
