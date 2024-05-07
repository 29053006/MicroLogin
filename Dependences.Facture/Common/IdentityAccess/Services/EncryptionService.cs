using Facture.IdentityAccess.Domain.Services;
using System;

namespace Facture.IdentityAccess.Application.Services
{
    /// <summary>
    /// Implementation of <see cref="IEncryptionService"/>
    /// using an <see cref="MD5"/> hasher to create a
    /// one-way hash of a plain text string.
    /// </summary>

    public class EncryptionService : IEncryptionService
    {
        public String CreateSaltedHash(String plainTextPassword)
        {
            return Facture.Core.Security.PasswordStorage.CreateHash(password: plainTextPassword);
        }

        public Boolean VerifyPassword(String plainTextPassword, String saltedHash)
        {
            return Facture.Core.Security.PasswordStorage.VerifyPassword(password: plainTextPassword, goodHash: saltedHash);
        }
    }
}
