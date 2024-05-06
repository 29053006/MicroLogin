using Facture.Core.Base;
using Facture.Core.Domain.DomainModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace Facture.IdentityAccess.Domain
{
    public class LoginAudit : ValueObject
    {
        public virtual DateTimeOffset? LastLoginDate { get; set; }

        [StringLength(DefaultLength.UserName)]
        public virtual String LastLoginIP { get; set; }

        public virtual DateTimeOffset? LastActivityDate { get; set; }

        [StringLength(DefaultLength.IP)]
        public virtual string LastActivityIP { get; set; }

        public virtual DateTimeOffset? LastChangedPasswordDate { get; set; }

        [StringLength(DefaultLength.IP)]
        public virtual string LastChangedPasswordIP { get; set; }

        protected LoginAudit()
        {

        }

        public static LoginAudit WithLastActivity(DateTimeOffset date, String ip)
        {
            LoginAudit login = new LoginAudit
            {
                LastActivityDate = date,
                LastActivityIP = ip
            };

            return login;
        }

        public static LoginAudit WithLastLogin(DateTimeOffset date, String ip)
        {
            LoginAudit login = new LoginAudit
            {
                LastLoginDate = date,
                LastLoginIP = ip
            };

            return login;
        }

        public static LoginAudit WithLastChangedPassword(DateTimeOffset date, String ip)
        {
            LoginAudit login = new LoginAudit
            {
                LastChangedPasswordDate = date,
                LastChangedPasswordIP = ip
            };

            return login;
        }

    }
}
