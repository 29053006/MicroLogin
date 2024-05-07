using System;

namespace Facture.IdentityAccess.Domain.Services
{
    public interface IPasswordService
    {
        String GenerateStrongPassword();
        Boolean IsStrong(String plainTextPassword);
        Boolean IsVeryStrong(String plainTextPassword);
        Boolean IsWeak(String plainTextPassword);

        Boolean LookLikeASaltedPassword(string textPassword);
    }
}