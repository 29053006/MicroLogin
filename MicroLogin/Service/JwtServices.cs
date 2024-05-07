using Dependences.Facture.Common.IdentityAccess.Contracts.Repositories;
using Dependences.Facture.Contracts;
using Facture.Core.Helpers;
using Facture.IdentityAccess.Application.Components.Entity;
using Facture.IdentityAccess.Application.Components.Services;
using Facture.IdentityAccess.Contracts.Model;
using Facture.IdentityAccess.Domain;
using System.Data;
using Facture.IdentityAccess.Application;
using Facture.IdentityAccess.Domain.Services;
using Facture.IdentityAccess.Contracts.Repositories;
using Microsoft.AspNetCore.Components;


namespace MicroLogin.Service
{
    public class JwtServices : IJwtServices
    {
        [Inject]
        List<ILoginValidator> _validators { get; set; }
        [Inject]
        ICreateJwtServices _createJwtServices { get; set; }
        [Inject]
        List<ITokenEnrichService> _tokenEnrichServices { get; set; }
        [Inject]
        IAuthorizedSoftwareRepository _authorizedSoftwareRepository { get; set; }
        [Inject]
        ITenantRepository _tenantRepository { get; set; }
        [Inject]
        IAuthenticationService _authenticationService { get; set; }

        private string HardCodedIssuer ;
        private int ParentSessionTokenTTLInMonths;
        private int SessionTokenTTLInMonths;
        private int ParentSessionTokenTTLInHours;
        private int SessionTokenTTLInHours;

        public const string AuthScheme = "Bearer";
        public virtual string SchemeType => "JWT";
        public JwtServices(
            IEnumerable<ILoginValidator> loginValidators)
        {
            var baseUrl = IdentityConfig.AppSettings.Url.ApiRoot;
            HardCodedIssuer = string.IsNullOrWhiteSpace(baseUrl) ? Environment.MachineName : new Uri(baseUrl).Host;
            ParentSessionTokenTTLInMonths = IdentityConfig.AppSettings.IdentityAccess.ParentSessionTokenTTLInMonths;
            SessionTokenTTLInMonths = IdentityConfig.AppSettings.IdentityAccess.SessionTokenTTLInMonths;

            ParentSessionTokenTTLInHours = IdentityConfig.AppSettings.IdentityAccess.ParentSessionTokenTTLInHours;
            SessionTokenTTLInHours = IdentityConfig.AppSettings.IdentityAccess.SessionTokenTTLInHours;
            _validators = loginValidators.ToList();
        }
        private readonly IConfiguration _configuration;

        public LoginResultData LoginUser(HttpRequest request, LoginData loginData)
        {
            Check.NotNull(() => loginData, "La propiedad 'loginData' no puede ser null");
            Check.NotNull(() => loginData.Username, "La propiedad 'loginData.Username' no puede ser null");
            Check.NotNull(() => loginData.Password, "La propiedad 'loginData.Password' no puede ser null");
            var traceStatus = TraceStatus.Get(request);

            var tenants = Enumerable.Empty<Tenant>();
            UserDescriptor user;

            var hasTenantId = loginData.TenantId == null ? false : true;

            traceStatus.StepBegin("[JwtController.LoginUser] Authenticate");
            if (loginData.TenantId == null)
            {
                user = _authenticationService.Authenticate(username: loginData.Username, password: loginData.Password, tenants: out tenants);
                if (user == UserDescriptor.NullDescriptorInstance() || !tenants.Any()) { return EmptyWithError(SN.BadCredentials); }
                if (!user.IsEnabled) { return EmptyWithError(SN.RecordNotActive); }

                if (user.DefaultTenant == null)
                {
                    Check.Require(tenants.Count() == 1, SN.TenantIdRequiredWhenMultipleFound);
                    loginData.TenantId = tenants.First().Id;
                }
                else
                {
                    loginData.TenantId = user.DefaultTenant;
                }


            }
            else
            {
                user = _authenticationService.Authenticate(tenantId: loginData.TenantId, username: loginData.Username, password: loginData.Password, tenants: out tenants);
                if (user == UserDescriptor.NullDescriptorInstance()) { return EmptyWithError(SN.BadCredentials); }
                if (!user.IsEnabled) { return EmptyWithError(SN.RecordNotActive); }
            }

            traceStatus.StepBegin("[JwtController.LoginUser] Impersonation");
            Tenant tryToImpersonateTenant = null;
            if (loginData.InNamedOfTenantId.HasValue || !string.IsNullOrWhiteSpace(loginData.InNamedOfTenantNumber))
            {
                tryToImpersonateTenant = loginData.InNamedOfTenantId.HasValue
                                            ? _tenantRepository.Get(id: loginData.InNamedOfTenantId.Value)
                                            : _tenantRepository.Get(tenantName: loginData.InNamedOfTenantNumber);

                Check.NotNull(tryToImpersonateTenant, SN.TenantToImpersonateNotFound);
                Check.NotNull(tryToImpersonateTenant.Parent, $"Permission to impersonate Tenant [{tryToImpersonateTenant.Id}] denied.");
                //Check.Require(tryToImpersonateTenant.Parent.Id == loginData.TenantId, $"Illegal data access to impersonate Tenant [{ tryToImpersonateTenant.Id }].");

                if (!loginData.InNamedOfTenantId.HasValue) { loginData.InNamedOfTenantId = tryToImpersonateTenant.Id; }
            }

            // se verifica si el login es desde la Web.
            var headerIsLoginWeb = request.Headers["X-WEB-LOGIN"].FirstOrDefault();
            var isWeb = Convert.ToBoolean(headerIsLoginWeb ?? "false");
            if (!hasTenantId && !isWeb)
            {
                // se verifica que el Token es requerido.
                var settingTokenRequired = _tenantRepository.GetTenantSettingValueByName(loginData.TenantId.Value, "TokenRequired");
                var tokenRequired = String.IsNullOrEmpty(settingTokenRequired) ? false : (settingTokenRequired.Equals("1") ? true : false);
                if (tokenRequired) { Check.Require(!String.IsNullOrEmpty(loginData.Token), $"Software token required for this Tenant [{loginData.TenantId}]."); }
            }

            Guid? rawParentTenantId = null;
            String rawParentTenantName = null;
            if (!String.IsNullOrEmpty(loginData.Token))
            {
                var authorizedSoftware = _authorizedSoftwareRepository.Get(parentTenantId: loginData.TenantId.Value, token: loginData.Token);
                Check.NotNull(authorizedSoftware, $"Software token to impersonate not found.");
                Check.NotNull(authorizedSoftware.ParentTenant, $"Permission to impersonate denied.");
                Check.NotNull(authorizedSoftware.Tenant, $"Permission to impersonate denied.");

                loginData.TenantId = authorizedSoftware.Tenant.Id;
                tryToImpersonateTenant = authorizedSoftware.Tenant;
                rawParentTenantId = (authorizedSoftware.ParentTenant.Id == authorizedSoftware.Tenant.Id) ? authorizedSoftware.Tenant.Parent?.Id : authorizedSoftware.ParentTenant.Id;
                rawParentTenantName = (authorizedSoftware.ParentTenant.Name == authorizedSoftware.Tenant.Name) ? authorizedSoftware.Tenant.Parent?.Name : authorizedSoftware.ParentTenant.Name;
            }

            foreach (var validator in _validators)
            {
                validator.Validate(request, loginData, user);
            }

            var tenantId = loginData.InNamedOfTenantId ?? loginData.TenantId;
            var tenant = tryToImpersonateTenant ?? user.Tenants;
            var parentTenantId = rawParentTenantId ?? tenant.Parent?.Id;
            var parentTenantName = rawParentTenantName ?? tenant.Parent?.Name;

            traceStatus.StepBegin("[JwtController.LoginUser] CreateJwtToken()");

            var jwtIdentity = new JwtIdentity(username: loginData.Username, tenantId: tenantId.Value, tenantName: tenant.Name, tenantDescription: tenant.Description, authenticationType: SchemeType, providerIssuer: loginData.ProviderIssuerId)
            {
                ParentTenantId = parentTenantId ?? Guid.Empty,
                ParentTenantName = parentTenantName ?? null
            };
            _tokenEnrichServices.ForEach(t => t.Enrich(tenantId: tenantId.Value, tenantName: tenant.Name, jwt: ref jwtIdentity));

            // WARNING: for parent tenants, token expires in one year
            var expires = tenant.Parent == null
                            ? DateTime.UtcNow.AddMonths(ParentSessionTokenTTLInMonths)
                            : DateTime.UtcNow.AddMonths(SessionTokenTTLInMonths);


            if (isWeb)
                expires = tenant.Parent == null
                            ? DateTime.UtcNow.AddHours(ParentSessionTokenTTLInHours)
                            : DateTime.UtcNow.AddHours(SessionTokenTTLInHours);

            var jwt = _createJwtServices.CreateJwtToken(HardCodedIssuer, tenantId.Value, loginData.Username, expires, jwtIdentity, TokenService.SaltKey, ip: loginData.IP, isWeb: isWeb);
            var accessToken = jwt.RawData;

            traceStatus.StepEnd();

            return new LoginResultData
            {
                DisplayName = user.Username,
                AccessToken = accessToken,
                TenantId = tenantId.Value
            };
        }
        private static LoginResultData EmptyWithError(string error)
        {
            return new LoginResultData { Error = error };
        }
    }
}
