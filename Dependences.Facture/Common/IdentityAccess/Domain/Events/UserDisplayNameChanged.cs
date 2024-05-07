using Facture.Core.Domain.Events;
using System;

namespace Facture.IdentityAccess.Domain.Events
{
    public class UserDisplayNameChanged : IDomainEvent
    {
        public UserDisplayNameChanged(
                Tenant tenant,
                String username,
                String displayName)
        {
            this.EventVersion = 1;
            this.OccurredOn = DateTimeOffset.Now;
            this.Tenant = tenant;
            this.Username = username;
            this.DisplayName = displayName;
        }

        public Enablement Enablement { get; private set; }

        public Int32 EventVersion { get; set; }

        public DateTimeOffset OccurredOn { get; set; }

        public Tenant Tenant { get; private set; }

        public String Username { get; private set; }

        public String DisplayName { get; set; }
    }
}
