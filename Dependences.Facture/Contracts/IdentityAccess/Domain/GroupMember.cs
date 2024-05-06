using Facture.Core.Domain.DomainModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace Facture.IdentityAccess.Domain
{
    public class GroupMember : Entity, IEquatable<GroupMember>
    {
        #region [ Internal Constructor ]

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupMember"/> class,
        /// restricted to internal access.
        /// </summary>
        /// <param name="tenant">
        /// Initial value of the <see cref="TenantId"/> property.
        /// </param>
        /// <param name="name">
        /// Initial value of the <see cref="Name"/> property.
        /// </param>
        /// <param name="type">
        /// Initial value of the <see cref="Type"/> property.
        /// </param>
        /// <remarks>
        /// This constructor is invoked by the <see cref="User.ToGroupMember"/>
        /// or <see cref="Group.ToGroupMember"/> factory methods of
        /// <see cref="User"/> or <see cref="Group"/>, respectively.
        /// </remarks>
        public GroupMember(Tenant tenant, String name, GroupMemberType type, Group group)
        {
            this.Name = name;
            this.Tenant = tenant;
            this.Type = type;
            this.Group = group;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupMember"/> class for a derived type,
        /// and otherwise blocks new instances from being created with an empty constructor.
        /// </summary>
        protected GroupMember()
        {
        }

        #endregion

        #region [ Public Properties ]

        public virtual Tenant Tenant { get; protected internal set; }
        [StringLength(maximumLength: 100)]

        public virtual String Name { get; protected internal set; }

        public virtual GroupMemberType Type { get; protected internal set; }

        public virtual Group Group { get; protected internal set; }

        public virtual Boolean IsGroup
        {
            get { return this.Type == GroupMemberType.Group; }
        }

        public virtual Boolean IsUser
        {
            get { return this.Type == GroupMemberType.User; }
        }

        public override Int32 GetHashCode()
        {
            int hashCodeValue =
                +(19563 * 181)
                + (this.Tenant != null ? 0 : this.Tenant.Id.GetHashCode())
                + (this.Name == null ? 0 : this.Name.GetHashCode())
                + this.Type.GetHashCode();

            return hashCodeValue;
        }

        public virtual bool Equals(GroupMember other)
        {
            return this.GetHashCode() == other.GetHashCode();
        }

        #endregion
    }
}
