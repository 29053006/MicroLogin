using Facture.Core.Domain.DomainModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace Facture.IdentityAccess.Domain
{
    public class UserSetting : EntityWithTypedId<Guid>
    {
        [DomainSignature]
        public virtual Tenant Tenant { get; set; }

        [DomainSignature]
        public virtual User User { get; set; }

        [DomainSignature]
        [StringLength(maximumLength: 100)]
        public virtual String Name { get; set; }

        public virtual String Value { get; set; }

        public UserSetting(Tenant tenant, String name, String value, User user) : this()
        {
            this.Tenant = tenant;
            this.Name = name;
            this.Value = value;
            this.User = user;
        }

        protected UserSetting() { }
    }
}
