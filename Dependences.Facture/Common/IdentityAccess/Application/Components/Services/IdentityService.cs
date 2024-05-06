using Facture.Core.Domain;
using Facture.IdentityAccess.Contracts.Model;
using System;
using System.Security.Principal;
using System.Threading;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using Dependences.Facture.Contracts;
using Facture.Core;

namespace Facture.IdentityAccess.Application.Components.Services
{
    public static class IdentityService
    {
        public static IHttpContextAccessor HttpContextAccessor { get; set; }
        public static ServiceProvider ServiceProvider;
        public static void Configure(IHttpContextAccessor accessor, ServiceProvider serviceProvider)
        {
            HttpContextAccessor = accessor;
            ServiceProvider = serviceProvider;
        }

        //[ThreadStatic]

        //private static bool IsWebContext => HttpContext.Current != null;

        //public static void CurrentUser(this ControllerBase cntrlBase) 
        //{
        //    var algo = cntrlBase.User.FindFirst("");
        //}


        public static ClaimsPrincipal CurrentUser
        {
            get
            {
                var user = (ClaimsPrincipal)HttpContextAccessor.HttpContext.Items["CurrentUser"];
                // WARNING: waiting for thread sync
                if (user == null)
                {
                    Thread.Sleep(millisecondsTimeout: 10);
                    user = HttpContextAccessor.HttpContext.User;
                }
                return user;
            }
            private set
            {
                HttpContextAccessor.HttpContext.Items["CurrentUser"] = value;
            }
        }

        public static void SetCurrent(Facture.IdentityAccess.Domain.Tenant tenant)
        {
            Check.NotNull(() => tenant, "La propiedad 'tenant' no puede ser null");
            SetCurrent(new JwtIdentity(tenant));
        }

        public static void SetCurrent(Facture.IdentityAccess.Domain.User userFacture)
        {
            Check.NotNull(() => userFacture, "La propiedad 'userFacture' no puede ser null");
            SetCurrent(new JwtIdentity(userFacture));
        }

        public static void SetCurrentWithoutTaxtCategory(Facture.IdentityAccess.Domain.User userFacture)
        {
            Check.NotNull(() => userFacture, "La propiedad 'userFacture' no puede ser null");
            SetCurrent(new JwtIdentity(userFacture, null));
        }

        public static void SetCurrent(JwtIdentity jwtIdentity)
        {
            Check.NotNull(() => jwtIdentity, "La propiedad 'jwtIdentity' no puede ser null");
            jwtIdentity.ScopeTenantId = jwtIdentity.TenantId;
            var principal = new GenericPrincipal(jwtIdentity, null);
            CurrentUser = principal;
        }

        public static bool IsAuthenticated => CurrentIdentity?.IsAuthenticated ?? false;


        public static JwtIdentity CurrentIdentityRequired
        {
            get
            {
                var currentJwtIdentity = CurrentIdentity;
                Check.NotNull(() => currentJwtIdentity, "La propiedad 'currentJwtIdentity' no puede ser null");
                return currentJwtIdentity;
            }
        }


        public static JwtIdentity CurrentIdentity
        {
            get
            {
                var userIdentity = CurrentUser?.Identity;
                if (userIdentity == null) { return null; }

                if (userIdentity is JwtIdentity identity) { return identity; }

                if (userIdentity is WindowsIdentity winIdentity)
                {
                    if (winIdentity.ImpersonationLevel == TokenImpersonationLevel.Anonymous) { return null; }
                    else
                    {
                        throw new ArgumentOutOfRangeException(nameof(winIdentity.ImpersonationLevel), winIdentity.ImpersonationLevel.ToString(), "Unexpected Impersonation Level");
                    }
                }
                // WARNING: dnn returns this user on anonymous sessions
                else if (userIdentity is GenericIdentity /*|| userIdentity is System.Web.Security.FormsIdentity*/) { return null; }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(userIdentity), userIdentity.GetType().FullName, SN.UnexpectdUserIdentity);
                }
            }
        }

        // Scope.cs
        public static Guid CurrentTenantId => CurrentIdentity?.TenantId ?? Guid.Empty;
        public static Guid ScopeTenantId
        {
            get => CurrentIdentity?.ScopeTenantId ?? Guid.Empty;
            set
            {
                if (CurrentIdentity != null) { CurrentIdentity.ScopeTenantId = value; }
                else { SetCurrent(new JwtIdentity() { ScopeTenantId = value }); }
            }
        }

        // PLColabScope.cs
        public static string CurrentTenantNumber => CurrentIdentityRequired.TenantName;

        public static String CurrentParentTenantNumber => CurrentIdentityRequired.ParentTenantName;

        public static Guid CurrentParentTenantId => CurrentIdentityRequired.ParentTenantId;
        public static String Rol => CurrentIdentityRequired.Rol;

        public static Guid? CurrentParentTenantIdNullable
        {
            get
            {
                var parentTenantId = CurrentParentTenantId;
                return parentTenantId == Guid.Empty ? new Nullable<Guid>() : parentTenantId;
            }
        }
    }
}
