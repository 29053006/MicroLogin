using Dependences.Facture.Common.IdentityAccess.Contracts.Repositories;
using Dependences.Facture.Contracts;
using Facture.Core.Domain.PersistenceSupport;
using Facture.IdentityAccess.Contracts.Repositories;
using Facture.IdentityAccess.Domain;
using Facture.IdentityAccess.Domain.Services;

namespace Facture.IdentityAccess.Application.Services
{
    /// <summary>
    /// A domain service providing a method
    /// to authenticate a <see cref="User"/>.
    /// </summary>
    public class AuthenticationService(IUserRepository _userRepository,
                                       ITenantRepository _tenantRepository,
                                       IEncryptionService _encryptionService) : IAuthenticationService
    {

        public UserDescriptor Authenticate(string username, string password, out IEnumerable<Tenant> tenants, Guid? tenantId = null, string portal = null)
        {
            Check.NotNull(username, SN.UsernameNotNull);
            Check.NotNull(password, SN.PasswordNotNull);

            UserDescriptor userDescriptor = UserDescriptor.NullDescriptorInstance();
            tenants = Enumerable.Empty<Tenant>();
            User user = null;

            if (string.IsNullOrEmpty(portal))
            {
                if (tenantId.HasValue)
                {
                    var tenant = _tenantRepository.Get(tenantId.Value);
                    Check.NotNull(tenant, SN.TenantNotFound);
                    Check.Require(tenant.Enablement.IsEnablementEnabled(), SN.RecordNotActive);

                    //RC: 19-02-2019 Autenticacion por web, Problemas al iniciar sesion luego de cambiar contraseña
                    user = _userRepository.UserWithUsernameWithNoCache(tenant.Id, username);
                }
                else
                {
                    //RC: 19-02-2019 Autenticacion por web, Problemas al iniciar sesion luego de cambiar contraseña
                    //Buscar usuarios excepto en el tenant FactureThirdPortal
                    user = _userRepository.UserWithUsernameWithNoCache(username);
                }
            }
            else
            {
                //RC: 19-02-2019 Autenticacion por web, Problemas al iniciar sesion luego de cambiar contraseña
                user = _userRepository.UserWithUsernameWithNoCache(portal: portal, username: username);
            }


            //para validar que el usuario se encuentre activo
            if ((user != null) && !user.IsEnabled)
            {
                Check.Assert(user.IsEnabled, SN.RecordNotActive);
            }

            //para validar usuarion en DNN por orden de ronald cuello
            if ((user != null) && _encryptionService.VerifyPassword(password, user.Password))
            {
                Check.Require(!user.IsPasswordExpired(), $"{SN.PasswordIsExpired} - {user.Username}:{user.PasswordExpiresOnDate}");

                userDescriptor = user.UserDescriptor;
                userDescriptor.DefaultTenant = user.DefaultTenant;
                tenants = _userRepository.TenantsFromUser(username);
            }

            return userDescriptor;
        }
    }
}
