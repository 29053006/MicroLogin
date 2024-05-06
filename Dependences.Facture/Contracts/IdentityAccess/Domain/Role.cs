using Facture.Core.Domain.DomainModel;
using System;
using System.Collections.Generic;

namespace Facture.IdentityAccess.Domain
{
    /// <summary>
    /// An entity representing an authentication role for a
    /// particular <see cref="Tenant"/>, which determines
    /// the types of access granted or denied to a
    /// <see cref="User"/> or <see cref="Domain.Group"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class borrows functionality from an internal
    /// instance of <see cref="Domain.Group"/> to be able to
    /// assign users and groups to this role.
    /// </para>
    /// <para>
    /// A role might also be called a "claim" in some
    /// authentication approaches.
    /// </para>
    /// </remarks>
    public class Role : EntityWithTypedId<Guid>
    {
        #region [ Fields and Constructor Overloads ]

        /// <summary>
        /// An internal instance of <see cref="Domain.Group"/>
        /// which provides functionality for assigning
        /// users and groups to this role.
        /// </summary>
        public virtual Group InternalGroup { get; set; }

        /// <summary>
        /// ExternalId is a reference to DNN Role Id
        /// </summary>
        public virtual Int32 ExternalId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Role"/> class.
        /// </summary>
        /// <param name="tenant">
        /// Initial value of the <see cref="Tenant"/> property.
        /// </param>
        /// <param name="name">
        /// Initial value of the <see cref="Name"/> property.
        /// </param>
        /// <param name="description">
        /// Initial value of the <see cref="Description"/> property.
        /// </param>
        /// <param name="supportsNesting">
        /// Initial value of the <see cref="SupportsNesting"/> property.
        /// </param>
        public Role(Tenant tenant, string name, string description, bool supportsNesting, bool ssoCompatible = false) : this()
        {
            // Defer validation to the property setters.
            this.Description = description;
            this.Name = name;
            this.SupportsNesting = supportsNesting;
            this.Tenant = tenant;
            this.SSOCompatible = ssoCompatible;
            this.InternalGroup = this.CreateInternalGroup();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Role"/> class for a derived type,
        /// and otherwise blocks new instances from being created with an empty constructor.
        /// </summary>
        protected Role()
        {
            Settings = new HashSet<RoleSetting>();
        }

        #endregion

        public virtual string Description { get; set; }

        [DomainSignature]
        public virtual string Name { get; set; }

        public virtual Boolean SupportsNesting { get; set; }

        public virtual Boolean SSOCompatible { get; set; }

        [DomainSignature]
        public virtual Tenant Tenant { get; set; }

        public virtual ISet<RoleSetting> Settings { get; set; }

        #region [ Command Methods which Publish Domain Events ]        

        /// <summary>
        /// Returns a string that represents the current entity.
        /// </summary>
        /// <returns>
        /// A unique string representation of an instance of this entity.
        /// </returns>
        public override string ToString()
        {
            const string Format = "Role [tenantId={0}, name={1}, description={2}, supportsNesting={3}, group={4}]";
            return string.Format(Format, this.Tenant.Id, this.Name, this.Description, this.SupportsNesting, this.InternalGroup);
        }

        private Group CreateInternalGroup()
        {
            string groupName = string.Concat(Group.RoleGroupPrefix, Guid.NewGuid());
            return new Group(this.Tenant, groupName, string.Concat("Role backing group for: ", this.Name));
        }

        #endregion
    }
}
