using Facture.Core.Domain.DomainModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace Facture.IdentityAccess.Domain
{
    public class AuthorizedSoftware : Entity
    {
        [Required]
        public virtual Tenant ParentTenant { get; set; }

        [Required]
        public virtual Tenant Tenant { get; set; }

        [Required]
        [StringLength(maximumLength: 40, MinimumLength = 4)]
        public virtual String Token { get; set; }

        public AuthorizedSoftware(Tenant parentTenant, Tenant tenant, String token) : this()
        {
            this.ParentTenant = parentTenant;
            this.Tenant = tenant;
            this.Token = token;
        }

        protected AuthorizedSoftware()
        {
        }
    }
}
