using Facture.Core.Domain.DomainModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace Facture.IdentityAccess.Domain
{
    public class TenantSetting : EntityWithTypedId<Guid>
    {
        [DomainSignature]
        public virtual Tenant Tenant { get; set; }

        [DomainSignature]
        [StringLength(maximumLength: 100)]
        public virtual String Name { get; set; }

        public virtual String Value { get; set; }

        public TenantSetting(Tenant tenant, String name, String value) : this()
        {
            this.Tenant = tenant;
            this.Name = name;
            this.Value = value;
        }

        protected TenantSetting() { }
    }
}
