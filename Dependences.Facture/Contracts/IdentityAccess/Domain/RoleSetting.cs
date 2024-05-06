using Facture.Core.Domain.DomainModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace Facture.IdentityAccess.Domain
{
    public class RoleSetting : EntityWithTypedId<Guid>
    {
        public virtual Role Role { get; set; }

        [DomainSignature]
        [StringLength(maximumLength: 50)]
        public virtual String Name { get; set; }
        [StringLength(maximumLength: 100)]

        public virtual String Value { get; set; }

        public RoleSetting(Role role, String name, String value) : this()
        {
            this.Role = role;
            this.Name = name;
            this.Value = value;
        }

        protected RoleSetting() { }
    }
}
