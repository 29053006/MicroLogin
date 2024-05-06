using Dependences.Facture.Contracts;
using Facture.Core.Base.Configurations;
using Facture.Core.Domain;
using Facture.IdentityAccess.Contracts.Repositories;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Facture.IdentityAccess.Application.Components.Services
{
    public class TokenService : ITokenService
    {
        /// <summary>
        /// <see cref="https://en.wikipedia.org/wiki/Year_2038_problem">Futuro yo, espero no estar leyendo este codigo en este año.</see>
        /// </summary>
        private readonly DateTime MaxEpoch = new DateTime(2038, 1, 19, 3, 14, 7, DateTimeKind.Utc);

        private const String LdfClaimType = "l";
        private const String BranchClaimType = "b";
        private const String ProcessClaimType = "p";
        private const String TenantClaimType = "t";
        private const String TenantIdClaimType = "tid";

        private static readonly byte[] SaltKeyLegacy = new byte[] { 0x38, 0x46, 0x44, 0x34, 0x38, 0x42, 0x31, 0x38, 0x42, 0x36, 0x46, 0x41, 0x43, 0x30, 0x37, 0x34, 0x30, 0x32, 0x44, 0x44, 0x34, 0x44, 0x41, 0x42, 0x34, 0x37, 0x41, 0x41, 0x31, 0x41, 0x39, 0x45, 0x44, 0x44, 0x33, 0x31, 0x46, 0x33, 0x37, 0x44, 0x42, 0x45, 0x41, 0x38, 0x30, 0x39, 0x32, 0x42, 0x30, 0x35, 0x38, 0x30, 0x33, 0x32, 0x33, 0x34, 0x43, 0x33, 0x41, 0x33, 0x30, 0x46, 0x36, 0x32 };
        //#r "System.Linq"
        //string byteArray = "new byte[] { " + string.Join(", ", System.Text.Encoding.UTF8.GetBytes(...).Select(t => "0x" + t.ToString("X2")).ToArray()) + " }"
        private static byte[] _SaltKey;
        // WARNING: Make it a lazy property
        public static byte[] SaltKey
        {
            get
            {
                if (_SaltKey == null)
                {
                    var firstKey = FactureConfig.AppSettings.App.SaltKey;
                    if (!string.IsNullOrWhiteSpace(firstKey)) { _SaltKey = Encoding.UTF8.GetBytes(FactureConfig.AppSettings.App.SaltKey); }
                    else { _SaltKey = SaltKeyLegacy; }
                }
                return _SaltKey;
            }
        }


        private readonly IUserRepository _userRepository;

        public TokenService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public String Generate(String branchCode, String processCode, String tenantName, String LDF, DateTime? expirationDate = null)
        {
            var user = _userRepository.UserWithUsername(username: tenantName);
            Check.NotNull(() => user, "La propiedad 'user' no puede ser null");

            var jwt = CreateJwtToken(branchCode, processCode, tenantName, LDF, expirationDate, TokenService.SaltKey);
            var jwtToken = jwt.RawData;

            return jwtToken;
        }

        public String Generate(Guid tenantId, String branchCode, String processCode, String tenantName, String LDF, DateTime? expirationDate = null)
        {
            var user = _userRepository.UserWithUsername(tenantId: tenantId, username: tenantName);
            Check.NotNull(() => user, "La propiedad 'user' no puede ser null");

            var jwt = CreateJwtToken(branchCode, processCode, tenantName, LDF, expirationDate, TokenService.SaltKey, tenantId);
            var jwtToken = jwt.RawData;

            return jwtToken;
        }

        private static JwtSecurityToken CreateJwtToken(String branchCode, String processCode, String tenantName, String LDF, DateTime? expirationDate, byte[] saltKey, Guid? tenantId = null)
        {
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(LdfClaimType, LDF));
            claimsIdentity.AddClaim(new Claim(BranchClaimType, branchCode));
            claimsIdentity.AddClaim(new Claim(ProcessClaimType, processCode));
            claimsIdentity.AddClaim(new Claim(TenantClaimType, tenantName));

            if (tenantId != null)
            {
                claimsIdentity.AddClaim(new Claim(TenantIdClaimType, tenantId.ToString()));
            }

            var credentials = new SigningCredentials(new SymmetricSecurityKey(saltKey), SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(issuer: null, audience: null, subject: claimsIdentity,
                                            notBefore: null, expires: expirationDate, issuedAt: null,
                                            signingCredentials: credentials);
            return token;
        }

        public string Generate(String issuer, IDictionary<String, Object> claims, DateTime notAfter, DateTime? notBefore = null)
        {
            var credentials = new SigningCredentials(new SymmetricSecurityKey(TokenService.SaltKey), SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);

            var payload = new JwtPayload(issuer: issuer, audience: null, claims: new List<Claim>(), notBefore: notBefore, expires: notAfter);
            foreach (var c in claims) { payload.Add(c.Key, c.Value); }

            var jwtToken = new JwtSecurityToken(new JwtHeader(credentials), payload);
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            return jwtTokenHandler.WriteToken(jwtToken);
        }

        public string Generate(String issuer, IDictionary<String, Object> claims, DateTime issueAt, DateTime notAfter, DateTime? notBefore = null)
        {
            var credentials = new SigningCredentials(new SymmetricSecurityKey(TokenService.SaltKey), SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);

            var payload = new JwtPayload(issuer: issuer, audience: null, claims: new List<Claim>(), notBefore: notBefore, expires: notAfter, issuedAt: issueAt);
            foreach (var c in claims) { payload.Add(c.Key, c.Value); }

            var jwtToken = new JwtSecurityToken(new JwtHeader(credentials), payload);
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            return jwtTokenHandler.WriteToken(jwtToken);
        }

        public bool ValidateToken(string rawToken, string issuer, bool checkExpiry)
            => ValidateToken(rawToken, issuer, checkExpiry, TokenService.SaltKey);

        private static bool ValidateToken(string rawToken, string issuer, bool checkExpiry, byte[] key)
        {
            var validationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuers = new[] { issuer },
                ValidateAudience = false,
                RequireExpirationTime = checkExpiry
            };

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(rawToken, validationParameters, out _);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
