using Facture.Core.Domain.DomainModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace Facture.IdentityAccess.Domain
{
    public class FullName : ValueObject
    {
        #region Public Properties
        [StringLength(maximumLength: 100)]
        public virtual String FirstName { get; set; }

        [StringLength(maximumLength: 100)]
        public virtual String MiddleName { get; set; }

        [StringLength(maximumLength: 100)]
        public virtual String FirstSurName { get; set; }

        [StringLength(maximumLength: 100)]
        public virtual String SecondSurName { get; set; }

        [StringLength(maximumLength: 300)]
        public String LastName { get { return String.Format("{0} {1}", FirstSurName, SecondSurName); } }

        [StringLength(maximumLength: 100)]
        public virtual String Treatment { get; set; }

        #endregion

        protected FullName()
        {

        }

        public FullName(String firstName, String lastName) : this()
        {
            this.FirstName = firstName;
            this.FirstSurName = lastName;
        }

        public FullName(String firstName, String lastName, String middleName, String secondSurName) : this()
        {
            this.FirstName = firstName;
            this.FirstSurName = lastName;
            this.MiddleName = middleName;
            this.SecondSurName = secondSurName;
        }

    }
}
