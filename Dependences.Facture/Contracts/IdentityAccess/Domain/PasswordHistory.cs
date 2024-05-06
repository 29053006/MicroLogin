using Facture.Core.Domain.DomainModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace Facture.IdentityAccess.Domain
{
    public class PasswordHistory : EntityWithTypedId<Guid>
    {
        [DomainSignature]
        public virtual Tenant Tenant { get; protected internal set; }

        [DomainSignature]
        public virtual User User { get; protected internal set; }

        [DomainSignature]
        [StringLength(maximumLength: 100)]
        public virtual String Password { get; protected internal set; }

        public virtual DateTimeOffset Date { get; protected internal set; }

        public PasswordHistory(Tenant tenant, String password, DateTimeOffset date, User user) : this()
        {
            this.Tenant = tenant;
            this.Password = password;
            this.Date = date;
            this.User = user;
        }

        public PasswordHistory() { }
    }
}
