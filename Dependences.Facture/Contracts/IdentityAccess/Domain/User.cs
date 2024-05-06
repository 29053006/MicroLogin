using Dependences.Facture.Contracts;
using Facture.Core.Base;
using Facture.Core.Domain;
using Facture.Core.Domain.DomainModel;
using Facture.IdentityAccess.Application;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Facture.IdentityAccess.Domain
{
    public class User : EntityWithTypedId<Int32>
    {
        #region [ Fields and Constructor Overloads ]

        private Enablement userEnablement;

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class
        /// and publishes a <see cref="UserRegistered"/> event.
        /// </summary>
        /// <param name="username">
        /// Initial value of the <see cref="Username"/> property.
        /// </param>
        /// <param name="password">
        /// Initial value of the <see cref="Password"/> property.
        /// </param>
        /// <param name="enablement">
        /// Initial value of the <see cref="Enablement"/> property.
        /// </param>
        /// <param name="person">
        /// Initial value of the <see cref="Person"/> property.
        /// </param>
        public User(
            Tenant tenant,
            string username,
            string password,
            Enablement enablement,
            Person person)
        {
            Check.NotNull(() => tenant, "La propiedad 'tenant' no puede ser null");
            Check.NotNull(() => person, "La propiedad 'person' no puede ser null");
            Check.NotEmpty(() => username, "La propiedad 'username' no puede estar vacía");
            Check.NotEmpty(() => password, "La propiedad 'password' no puede estar vacía");

            //Defer validation to the property setters.
            this.Enablement = enablement;
            this.Person = person;
            this.Tenant = tenant;
            this.Username = username;

            this.Password = password;
            this.CreatedOnDate = DateTimeOffset.Now;

            person.User = this;
            Settings = new HashSet<UserSetting>();
            History = new HashSet<PasswordHistory>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class for a derived type,
        /// and otherwise blocks new instances from being created with an empty constructor.
        /// </summary>
        protected User()
        {
            Settings = new HashSet<UserSetting>();
            History = new HashSet<PasswordHistory>();
        }

        #endregion

        public virtual Tenant Tenant { get; set; }

        public virtual DateTimeOffset? CreatedOnDate { get; set; }

        public virtual DateTimeOffset? PasswordExpiresOnDate { get; set; }

        public virtual Boolean IsEnabled
        {
            get { return this.Enablement.IsEnablementEnabled(); }
        }

        public virtual Enablement Enablement
        {
            get
            {
                return this.userEnablement;
            }

            set
            {
                Check.NotNull(value, SN.EnablementObjectIsRequired);
                this.userEnablement = value;
            }
        }

        public virtual Boolean Enablement_AuthorizeThird { get; set; }

        [StringLength(maximumLength: 100)]
        public virtual String Password { get; set; }

        public virtual Person Person { get; set; }
        public virtual Boolean IsCustomerUser { get; set; }
        public virtual Boolean IsPortalThird { get; set; }
        public virtual String UrlPortalThird { get; set; }

        public virtual String DisplayName { get; set; }

        public virtual ISet<UserSetting> Settings { get; set; }

        public virtual ISet<PasswordHistory> History { get; set; }

        public virtual UserDescriptor UserDescriptor
        {
            get
            {
                var displayName = (String.IsNullOrEmpty(DisplayName) ? $"{this.Person.Name.FirstName} {this.Person.Name.FirstSurName}" : DisplayName);

                return new UserDescriptor(
                    this.Tenant,
                    this.Id,
                    this.Username,
                    displayName,
                    this.Person.EmailAddress,
                    this.IsEnabled,
                    this.PasswordExpiresOnDate);
            }
        }

        [StringLength(DefaultLength.UserName)]
        [DomainSignature]
        public virtual string Username { get; set; }

        public virtual LoginAudit LoginAudit { get; set; }

        public virtual Boolean IsSSO { get; set; }

        public virtual Guid? DefaultTenant { get; set; }

        public virtual Boolean? IsConfidentialUser { get; set; }

        public virtual Boolean? Enablement_SecurityCode { get; set; }

        /// <summary>
        /// Returns a string that represents the current entity.
        /// </summary>
        /// <returns>
        /// A unique string representation of an instance of this entity.
        /// </returns>
        public override string ToString()
        {
            const string Format = "User [tenantId={0}, username={1}, person={2}, enablement={3}]";
            return string.Format(Format, this.Tenant, this.Username, this.Person, this.Enablement);
        }
    }
}
