using Facture.Core.Domain.PersistenceSupport;
using Facture.IdentityAccess.Domain;
using System;
using System.Collections.Generic;

namespace Facture.IdentityAccess.Contracts.Repositories
{
    /// <summary>
    /// Contract for a collection-oriented repository of <see cref="User"/> entities.
    /// </summary>
    /// <remarks>
    /// Because this is a collection-oriented repository, the <see cref="Add"/>
    /// method needs to be called no more than once per stored entity.
    /// Subsequent changes to any stored <see cref="User"/> are implicitly
    /// persisted, and adding the same entity a second time will have no effect.
    /// </remarks>
    public interface IUserRepository : IRepositoryWithTypedId<User, Int32>
    {
        /// <summary>
        /// Retrieves a <see cref="User"/> from the repository
        /// based on a username and password for authentication,
        /// and implicitly persists any changes to the retrieved entity.
        /// </summary>
        /// <param name="tenantId">
        /// The identifier of a <see cref="Tenant"/> to which
        /// a <see cref="User"/> may belong, corresponding
        /// to its <see cref="User.TenantId"/>.
        /// </param>
        /// <param name="username">
        /// The unique name of a <see cref="User"/>, matching
        /// the value of its <see cref="User.Username"/>.
        /// </param>
        /// <param name="encryptedPassword">
        /// A one-way hash of the password paired with
        /// the <paramref name="username"/>.
        /// </param>
        /// <returns>
        /// The instance of <see cref="User"/> retrieved,
        /// or a null reference if no matching entity exists
        /// in the repository.
        /// </returns>
        User UserFromAuthenticCredentials(Guid tenantId, String username, String encryptedPassword);

        /// <summary>
        /// Retrieves a <see cref="User"/> from the repository
        /// based on a username and password for authentication,
        /// and implicitly persists any changes to the retrieved entity.
        /// </summary>
        /// <param name="username">
        /// The unique name of a <see cref="User"/>, matching
        /// the value of its <see cref="User.Username"/>.
        /// </param>
        /// <param name="encryptedPassword">
        /// A one-way hash of the password paired with
        /// the <paramref name="username"/>.
        /// </param>
        /// <returns>
        /// The instance of <see cref="User"/> retrieved,
        /// or a null reference if no matching entity exists
        /// in the repository.
        /// </returns>
        User UserFromAuthenticCredentials(String username, String encryptedPassword);

        /// <summary>
        /// Retrieves a <see cref="User"/> from the repository
        /// based only on a username, when authentication
        /// is not needed or already assumed, and implicitly
        /// persists any changes to the retrieved entity.
        /// </summary>
        /// <param name="tenantId">
        /// The identifier of a <see cref="Tenant"/> to which
        /// a <see cref="User"/> may belong, corresponding
        /// to its <see cref="User.TenantId"/>.
        /// </param>
        /// <param name="username">
        /// The unique name of a <see cref="User"/>, matching
        /// the value of its <see cref="User.Username"/>.
        /// </param>
        /// <returns>
        /// The instance of <see cref="User"/> retrieved,
        /// or a null reference if no matching entity exists
        /// in the repository.
        /// </returns>
        User UserWithUsername(Guid tenantId, string username);
        User UserWithUsernameWithNoCache(Guid tenantId, string username);
        User UserWithTenantAndUsername(string tenantNumber, string username);

        User UserWithUsername(Guid tenantId, string username, Boolean readOnly, bool cacheable);

        /// <summary>
        /// Retrieves a <see cref="User"/> from the repository
        /// based only on a username, when authentication
        /// is not needed or already assumed, and implicitly
        /// persists any changes to the retrieved entity.
        /// </summary>
        /// <param name="username">
        /// The unique name of a <see cref="User"/>, matching
        /// the value of its <see cref="User.Username"/>.
        /// </param>
        /// <returns>
        /// The instance of <see cref="User"/> retrieved,
        /// or a null reference if no matching entity exists
        /// in the repository.
        /// </returns>
        User UserWithUsername(string username);
        User UserWithUsernameWithNoCache(string username);

        User UserWithUsername(string portal, string username);
        User UserWithUsernameWithNoCache(string portal, string username);
        User UserWithUsernameWithNoCache(string portal, string username,Guid tenantId);


        /// <summary>
        /// Retrieves a <see cref="User"/> from the repository
        /// based only on a username, when authentication
        /// is not needed or already assumed, and implicitly
        /// persists any changes to the retrieved entity.
        /// </summary>
        /// <param name="username">
        /// The unique name of a <see cref="User"/>, matching
        /// the value of its <see cref="User.Username"/>.
        /// </param>
        /// <returns>
        /// The instance of <see cref="User"/> retrieved,
        /// or a null reference if no matching entity exists
        /// in the repository.
        /// </returns>
        User UserWithUsernameOrImpersonated(Guid tenantId, string username);

        /// <summary>
        /// Retrieves a <see cref="User"/> from the repository
        /// based only on a username, when authentication
        /// is not needed or already assumed, and implicitly
        /// persists any changes to the retrieved entity.
        /// </summary>
        /// <param name="username">
        /// The email of a <see cref="User"/>, matching
        /// the value of its <see cref="User.Person.ContactInformation.EmailAddress"/>.
        /// </param>
        /// <returns>
        /// The instance of <see cref="User"/> retrieved,
        /// or a null reference if no matching entity exists
        /// in the repository.
        /// </returns>
        User UserWithEmail(string emailAddress);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        IEnumerable<Tenant> TenantsFromUser(string username);

        IEnumerable<User> GetAllUsers(Guid tenantId, Int32 pageIndex, Int32 pageSize, out Int32 rowCount);

        User UserFromUserId(Guid? tenantId, Int32 userId);

        User UserFromUserId(Int32 userId);

        User FirstUserWithUsername(string username);

        IEnumerable<User> UsersWithUsername(string username);

        IEnumerable<User> UsersWithUsernameThirdPortal(string username);

        IEnumerable<User> FindSSOUsers(Guid tenantId);

        User UserWithUsernameAndTenantId(Guid tenantId, string username);

    }
}
