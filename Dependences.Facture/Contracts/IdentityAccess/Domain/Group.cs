using Facture.Core.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Facture.IdentityAccess.Domain
{
    public class Group : Entity
    {
        #region [ Fields and Constructor Overloads ]

        /// <summary>
        /// String constant with a prefix used by the private
        /// <see cref="IsInternalGroup"/> property to determine
        /// whether a <see cref="Group"/> is used only as an
        /// internal member of a <see cref="Domain.Model.Access.Role"/>.
        /// </summary>
        internal const string RoleGroupPrefix = "ROLE-INTERNAL-GROUP: ";

        /// <summary>
        /// Initializes a new instance of the <see cref="Group"/> class.
        /// </summary>
        /// <param name="tenantId">
        /// Initial value of the <see cref="TenantId"/> property.
        /// </param>
        /// <param name="name">
        /// Initial value of the <see cref="Name"/> property.
        /// </param>
        /// <param name="description">
        /// Initial value of the <see cref="Description"/> property.
        /// </param>
        public Group(Tenant tenant, string name, string description)
            : this()
        {
            // Defer validation to the property setters.
            this.Description = description;
            this.Name = name;
            this.Tenant = tenant;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Group"/> class for a derived type,
        /// and otherwise blocks new instances from being created with an empty constructor.
        /// </summary>
        protected Group()
        {
            this.GroupMembers = new HashSet<GroupMember>();
        }

        #endregion

        #region [ Public Properties and Private IsInternalGroup Property ]

        public virtual Tenant Tenant { get; protected internal set; }
        [StringLength(maximumLength: 100)]

        public virtual String Name { get; protected internal set; }
        [StringLength(maximumLength: 500)]

        public virtual String Description { get; protected internal set; }

        public virtual ISet<GroupMember> GroupMembers { get; protected internal set; }

        /// <summary>
        /// Gets a value indicating whether this group exists only
        /// to manage the membership in an authentication
        /// <see cref="Domain.Model.Access.Role"/>.
        /// If <c>true</c>, this group should not be
        /// shown on any lists of groups.
        /// </summary>
        public virtual bool IsInternalGroup
        {
            get { return this.Name.StartsWith(RoleGroupPrefix, StringComparison.Ordinal); }
        }

        #endregion

        /// <summary>
        /// Returns a string that represents the current entity.
        /// </summary>
        /// <returns>
        /// A unique string representation of an instance of this entity.
        /// </returns>
        public override String ToString()
        {
            const string Format = "Group [tenantId={0}, name={1}, description={2}]";
            return string.Format(Format, this.Tenant.Id, this.Name, this.Description);
        }

        /// <summary>
        /// Creates a <see cref="GroupMember"/> value of
        /// type <see cref="GroupMemberType.Group"/>
        /// based on this <see cref="Group"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="GroupMember"/> value of type
        /// <see cref="GroupMemberType.Group"/>
        /// based on this <see cref="Group"/>.
        /// </returns>
        public virtual GroupMember ToGroupMember()
        {
            return new GroupMember(this.Tenant, this.Name, GroupMemberType.Group, this);
        }
    }
}
