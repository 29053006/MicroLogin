using CommonServiceLocator;
using Facture.Core.Domain;
using Facture.Core.Domain.Events;
using Facture.IdentityAccess.Application;
using Facture.IdentityAccess.Application.Components.Entity;
using Facture.IdentityAccess.Domain.Events;
using Facture.IdentityAccess.Domain.Services;
using CommonServiceLocator;
using System;
using System.Collections.Generic;
using System.Linq;
using Dependences.Facture.Contracts;
using Dependences.Facture.Common.Core.Domain;

namespace Facture.IdentityAccess.Domain
{
    public static class UserActions
    {
        public static Boolean IsPasswordExpired(this User user)
        {
            return (user.PasswordExpiresOnDate.HasValue && user.PasswordExpiresOnDate < DateTimeOffset.Now);
        }

        public static Boolean IsInPasswordHistory(this User user, String plainTextPassword)
        {
            var encryptionService = SafeServiceLocator<IEncryptionService>.GetService();
            Check.NotNull(() => encryptionService, "La propiedad 'encryptionService' no puede ser null");

            var inHistory = user.History.Any(t => encryptionService.VerifyPassword(plainTextPassword: plainTextPassword, saltedHash: t.Password));
            return inHistory;
        }

        public static (PasswordHistory recent, List<PasswordHistory> oldest) ArchivePassword(this User user)
        {
            var entry = new PasswordHistory(tenant: user.Tenant, password: user.Password, date: DateTimeOffset.Now, user: user);
            user.History.Add(entry);

            // WARNING Make sure to keep the lastest N changed passwords
            var oldest = user.History.OrderByDescending(t => t.Date)
                                      .Skip(IdentityConfig.AppSettings.IdentityAccess.PasswordHistoryEntries)
                                      .ToList();
            foreach (var o in oldest)
            {
                user.History.Remove(o);
            }

            return (entry, oldest);
        }

        public static void WithPasswordExpiration(this User user, DateTimeOffset? expiresAt = null)
        {
            user.PasswordExpiresOnDate = expiresAt ?? BuildPasswordExpiration();
        }

        public static void ChangePassword(this User user, String currentPassword, String changedPassword)
        {
            Check.NotEmpty(() => currentPassword, "La propiedad 'currentPassword' no puede estar vacía");

            var encryptionService = SafeServiceLocator<IEncryptionService>.GetService();
            Check.NotNull(() => encryptionService, "La propiedad 'encryptionService' no puede ser null");
            Check.Require(encryptionService.VerifyPassword(plainTextPassword: currentPassword, saltedHash: user.Password), SN.MustConfirmPassword);

            user.ProtectPassword(currentPassword, changedPassword);
            user.PasswordExpiresOnDate = BuildPasswordExpiration();

            DomainEvents.Raise(
                new UserPasswordChanged(
                        user.Tenant,
                        user.Username)
                        );
        }

        internal static DateTimeOffset BuildPasswordExpiration()
            => DateTimeOffset.Now.AddDays(IdentityConfig.AppSettings.IdentityAccess.PasswordTTLInDays);

        public static User EstablishSaltedPassword(this User self, String saltedPassword)
        {
            var passwordService = ServiceLocator.Current.GetInstance<IPasswordService>();
            Check.NotNull(() => passwordService, "La propiedad 'passwordService' no puede ser null");
            Check.NotEmpty(() => saltedPassword, "La propiedad 'saltedPassword' no puede estar vacía");
            Check.Require(passwordService.LookLikeASaltedPassword(saltedPassword), SN.InvalidPassword);

            self.Password = saltedPassword;
            self.PasswordExpiresOnDate = BuildPasswordExpiration();

            return self;
        }

        public static void EstablishPassword(this User user, String plainTextPassword)
        {
            Check.NotEmpty(() => plainTextPassword, "La propiedad 'plainTextPassword' no puede estar vacía");

            var passwordService = ServiceLocator.Current.GetInstance<IPasswordService>();
            Check.NotNull(() => passwordService, "La propiedad 'passwordService' no puede ser null");
            Check.Require(!passwordService.IsWeak(plainTextPassword), SN.WeakPassword);
            Check.NotEquals(user.Username, plainTextPassword, SN.UserAndPasswordMustBeDifferent);

            user.Password = AsEncryptedValue(plainTextPassword);
            user.PasswordExpiresOnDate = BuildPasswordExpiration();

            DomainEvents.Raise(
                new UserPasswordEstablished(
                    user.Tenant,
                    user.Username)
                );
        }

        public static User ChangeEmail(this User user, String emailAddress)
        {
            user.Person.ContactInformation.EmailAddress = emailAddress;

            return user;
        }

        public static void ChangeContactInformation(this User user,
                String emailAddress,
                Address address,
                Phone primaryPhone,
                Phone secondaryPhone)
        {
            user.Person.ContactInformation.EmailAddress = emailAddress;
            user.Person.ContactInformation.Address = address;
            user.Person.ContactInformation.PrimaryPhone = primaryPhone;
            user.Person.ContactInformation.SecondaryPhone = secondaryPhone;

            DomainEvents.Raise(new PersonContactInformationChanged(
                        user.Tenant,
                        user.Username,
                        user.Person.ContactInformation));
        }

        public static void ChangePersonalContactInformation(this User user,
                String emailAddress,
                Address address,
                Phone primaryPhone,
                Phone secondaryPhone)
        {
            Check.NotEmpty(() => emailAddress, "La propiedad 'emailAddress' no puede estar vacía");
            Check.NotNull(() => address, "La propiedad 'address' no puede ser null");

            user.Person.ContactInformation.EmailAddress = emailAddress;
            user.Person.ContactInformation.Address = address;
            user.Person.ContactInformation.PrimaryPhone = primaryPhone;
            user.Person.ContactInformation.SecondaryPhone = secondaryPhone;

            DomainEvents.Raise(new PersonContactInformationChanged(
                        user.Tenant,
                        user.Username,
                        user.Person.ContactInformation));
        }

        public static void ChangePersonalName(this User user,
                FullName fullName)
        {
            Check.NotNull(() => fullName, "La propiedad 'fullName' no puede ser null");

            user.Person.Name = fullName;

            DomainEvents.Raise(new PersonalNameChanged(
                        user.Tenant,
                        user.Username,
                        user.Person.Name));
        }

        public static void ChangeDisplayName(this User user, String displayName)
        {
            user.DisplayName = (String.IsNullOrEmpty(displayName) ? $"{user.Person.Name.FirstName} {user.Person.Name.FirstSurName}" : displayName);

            DomainEvents.Raise(new UserDisplayNameChanged(
                        user.Tenant,
                        user.Username,
                        user.DisplayName));
        }

        public static void DefineEnablement(this User user, Enablement enablement)
        {
            user.Enablement = enablement;

            DomainEvents.Raise(new UserEnablementChanged(
                        user.Tenant,
                        user.Username,
                        user.Enablement));
        }

        /// <summary>
        /// Creates a <see cref="GroupMember"/> value of
        /// type <see cref="GroupMemberType.User"/>
        /// based on this <see cref="User"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="GroupMember"/> value of type
        /// <see cref="GroupMemberType.User"/>
        /// based on this <see cref="User"/>.
        /// </returns>
        public static GroupMember ToGroupMember(this User user, Group group)
        {
            return new GroupMember(user.Tenant, user.Username, GroupMemberType.User, group);
        }

        private static string AsEncryptedValue(string plainTextPassword)
        {
            var encryptionService = ServiceLocator.Current.GetInstance<IEncryptionService>();
            Check.NotNull(() => encryptionService, "La propiedad 'encryptionService' no puede ser null");

            return encryptionService.CreateSaltedHash(plainTextPassword);
        }

        public static void ProtectPassword(this User user, string currentPassword, string changedPassword)
        {
            Check.NotEquals(currentPassword, changedPassword, SN.NewPassswordMustBeDifferent);
            //TODO: Change this!
            //Check.Require(!passwordService.IsWeak(changedPassword), SN.WeakPassword);
            //Check.NotEquals(user.Username, changedPassword, SN.UserAndPasswordMustBeDifferent);

            ProtectPassword(user, changedPassword);
        }

        public static void ProtectPassword(this User user, string changedPassword)
        {
            var passwordService = ServiceLocator.Current.GetInstance<IPasswordService>();

            user.Password = AsEncryptedValue(changedPassword);
        }

        public static void WithSetting(this User user, String name, String value, Boolean @override)
        {
            if (!@override)
                Check.Require(!user.Settings.Any(t => t.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)), string.Format(SN.RecordAlreadyFoundWithName, name));

            if (!user.Settings.Any(t => t.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)))
                user.Settings.Add(new UserSetting(tenant: user.Tenant, name: name, value: value, user: user));
            else
            {
                var setting = user.Settings.SingleOrDefault(t => t.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
                setting.Value = value;
            }
        }

        public static User EstablishLastLoginDate(this User user, String ip)
        {
            if (user.LoginAudit == null)
            {
                user.LoginAudit = LoginAudit.WithLastLogin(DateTimeOffset.Now, ip);
            }
            else
            {
                user.LoginAudit.LastLoginDate = DateTimeOffset.Now;
                user.LoginAudit.LastLoginIP = ip;
            }

            return user;
        }

        public static User EstablishLastActivity(this User user, String ip)
        {
            if (user.LoginAudit == null)
            {
                user.LoginAudit = LoginAudit.WithLastActivity(DateTimeOffset.Now, ip);
            }
            else
            {
                user.LoginAudit.LastActivityDate = DateTimeOffset.Now;
                user.LoginAudit.LastActivityIP = ip;
            }

            return user;
        }

        public static User EstablishLastChangedPassword(this User user, String ip)
        {
            if (user.LoginAudit == null)
            {
                user.LoginAudit = LoginAudit.WithLastChangedPassword(DateTimeOffset.Now, ip);

            }
            else
            {
                user.LoginAudit.LastChangedPasswordDate = DateTimeOffset.Now;
                user.LoginAudit.LastChangedPasswordIP = ip;
            }

            return user;
        }

        public static User ChangeIsConfidentialUser(this User user, Boolean? isConfidentialUser)
        {
            user.IsConfidentialUser = isConfidentialUser;
            return user;
        }

        public static User ChangeEnablement_SecurityCode(this User user, Boolean? enablement_SecurityCode)
        {
            user.Enablement_SecurityCode = enablement_SecurityCode;
            return user;
        }

        public static Byte[] GetProfilePicture(this User user)
        {
            if (!user.Settings.ContainsKey(key: IdentityConfig.UserSettings.ProfilePicture))
            { return null; }
            else
            {
                var userSetting = user.Settings.Retrieve(key: IdentityConfig.UserSettings.ProfilePicture);
                var image = Convert.FromBase64String(userSetting.Value);
                return image;
            }
        }

        public static string GetPictureBase64(this User user)
        {
            byte[] rawLogo = null;
            try
            {
                rawLogo = GetProfilePicture(user);
            }
            catch
            {
                //Error 404 resource not found 
                //impide inicio de sesion
            }

            if (rawLogo == null)
                return null;

            return Convert.ToBase64String(rawLogo);
        }
    }
}
