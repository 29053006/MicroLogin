using Facture.IdentityAccess.Application.Components.Entity;
using Facture.IdentityAccess.Domain;
using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace Facture.IdentityAccess.Application.Components.Services
{
    public interface ILoginValidator
    {
        void Validate(HttpRequest request, LoginData login, UserDescriptor user);
    }
}
