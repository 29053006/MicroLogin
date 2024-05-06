using Dependences.Facture.Contracts;
using Facture.Core.Domain;
using Facture.Core.Domain.DomainModel;
using Facture.IdentityAccess.Application;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Facture.IdentityAccess.Domain
{
    public class Tenant : EntityWithTypedId<Guid>
    {
        #region Public Properties

        [DomainSignature]
        [StringLength(maximumLength: 100)]
        public virtual String Name { get; set; }

        public virtual String ParentNameNE { get; set; }

        [StringLength(maximumLength: 500)]
        public virtual String Description { get; set; }

        public virtual Tenant Parent { get; set; }

        public virtual Enablement Enablement { get; set; }

        [Required]
        public virtual String Platform { get; set; }

        public virtual ISet<TenantSetting> Settings { get; set; }

        public virtual ISet<Role> Role { get; set; }


        protected string _features;
        [Required]
        public virtual IEnumerable<String> Features
        {
            get
            {
                if (string.IsNullOrEmpty(_features))
                {
                    return new List<String>();
                }

                return _features.Split(new[] { "|" }, StringSplitOptions.None).ToList();
            }
            set
            {
                _features = string.Join("|", value);
            }
        }

        public virtual Boolean HasIssued { get; set; }

        public virtual Boolean IsMassive { get; set; }
        public virtual Boolean IsCorporate { get; set; }
        public virtual Boolean IsAlly { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Tenant"/> class.
        /// </summary>
        /// <param name="name">
        /// Initial value of the <see cref="Name"/> property.
        /// </param>
        /// <param name="description">
        /// Initial value of the <see cref="Description"/> property.
        /// </param>
        /// <param name="active">
        /// Initial value of the <see cref="Active"/> property.
        /// </param>
        public Tenant(string name, string description, string platform) : this()
        {
            Check.NotEmpty(() => name, "La propiedad 'name' no puede estar vacía");
            Check.NotEmpty(() => description, "La propiedad 'description' no puede estar vacía");
            this.Name = name;
            this.Description = description;
            this.Platform = platform;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tenant"/> class.
        /// </summary>
        /// <param name="name">
		/// Initial value of the <see cref="Name"/> property.
		/// </param>
        /// <param name="description">
		/// Initial value of the <see cref="Description"/> property.
		/// </param>
        /// <param name="id">
		/// Initial value of the <see cref="Id"/> property.
		/// </param>
        public Tenant(string name, string description, Guid id, String platform) : this(name: name, description: description, platform: platform)
        {
            Check.Assert(id != Guid.Empty, SN.TenantIdRequired);

            this.Id = id;
        }

        /// <summary>
		/// Initializes a new instance of the <see cref="Tenant"/> class.
		/// </summary>
		/// <param name="name">
		/// Initial value of the <see cref="Name"/> property.
		/// </param>
		/// <param name="description">
		/// Initial value of the <see cref="Description"/> property.
		/// </param>
		/// <param name="active">
		/// Initial value of the <see cref="Active"/> property.
		/// </param>
		public Tenant(string name, string description, Tenant parent, String platform) : this(name: name, description: description, platform: platform)
        {
            this.Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tenant"/> class.
        /// </summary>
        /// <param name="name">
		/// Initial value of the <see cref="Name"/> property.
		/// </param>
        /// <param name="description">
		/// Initial value of the <see cref="Description"/> property.
		/// </param>
        /// <param name="parent">
		/// Initial value of the <see cref="Parent"/> property.
		/// </param>
        /// <param name="id">
		/// Initial value of the <see cref="Id"/> property.
		/// </param>
        public Tenant(string name, string description, Tenant parent, Guid id, String platform) : this(name: name, description: description, id: id, platform: platform)
        {
            this.Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tenant"/> class for a derived type,
        /// and otherwise blocks new instances from being created with an empty constructor.
        /// </summary>
        protected Tenant()
        {
            Settings = new HashSet<TenantSetting>();
            Features = new List<String>();
        }

        /// <summary>
        /// Returns a string that represents the current entity.
        /// </summary>
        /// <returns>
        /// A unique string representation of an instance of this entity.
        /// </returns>
        //public override string ToString()
        //{
        //    const string Format = "Tenant [tenantId={0}, name={1}, description={2}, active={3}]";
        //    return string.Format(Format, this.Id, this.Name, this.Description, this.Enablement?.Enabled);
        //}
    }
}
