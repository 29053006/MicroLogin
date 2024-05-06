using Facture.Core.Domain.DomainModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace Facture.IdentityAccess.Domain
{
    public class UserDescriptor : ValueObject
    {
        public static UserDescriptor NullDescriptorInstance()
        {
            return new UserDescriptor();
        }

        public UserDescriptor(Tenant tenants, Int32 userId, String username, String displayName, String emailAddress, Boolean isEnabled, DateTimeOffset? passwordExpiration)
        {
            this.Tenants = tenants;
            this.UserId = userId;
            this.Username = username;
            this.DisplayName = displayName;
            this.EmailAddress = emailAddress;
            this.IsEnabled = isEnabled;
            this.PasswordExpiration = passwordExpiration;
        }

        protected UserDescriptor() { }

        [StringLength(maximumLength: 250)]
        public virtual String EmailAddress { get; protected internal set; }

        public virtual Tenant Tenants { get; protected internal set; }

        [StringLength(maximumLength: 100)]
        public virtual String Username { get; protected internal set; }

        [StringLength(maximumLength: 100)]
        public virtual String DisplayName { get; protected internal set; }

        public virtual Int32 UserId { get; protected internal set; }

        public virtual Boolean IsEnabled { get; protected internal set; }

        public virtual DateTimeOffset? PasswordExpiration { get; protected internal set; }

        public virtual Guid? DefaultTenant { get; set; }

        public override string ToString()
        {
            return $"{nameof(UserDescriptor)} [{nameof(EmailAddress)}: '{EmailAddress}', TenantId: '{Tenants}', {nameof(Username)}: '{Username}']";
        }
    }
}
