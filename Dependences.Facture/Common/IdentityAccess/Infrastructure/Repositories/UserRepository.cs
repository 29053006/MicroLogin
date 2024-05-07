using Facture.Core.Domain;
using Facture.Core.Domain.PersistenceSupport;
using Facture.IdentityAccess.Contracts.Repositories;
using Facture.IdentityAccess.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Facture.IdentityAccess.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        public IDbContext DbContext => throw new NotImplementedException();

        //public User UserFromAuthenticCredentials(string username, string encryptedPassword)
        //{
        //    var result = Session.CreateCriteria<User>()
        //                .CreateCriteria("User.Tenant", "Tenant", JoinType.InnerJoin)
        //                .Add(Restrictions.Eq("Username", username))
        //                .Add(Restrictions.Eq("Password", encryptedPassword))
        //                .Add(Restrictions.Not(Restrictions.Eq("Tenant.Name", "FactureThirdPortal")))
        //                .SetCacheable(true)
        //                .SetFlushMode(NHibernate.FlushMode.Manual)
        //                .SetReadOnly(true)
        //                .UniqueResult<User>();

        //    return result;
        //}

        //public User UserFromAuthenticCredentials(Guid tenantId, string username, string encryptedPassword)
        //{
        //    var result = Session.CreateCriteria<User>("User")
        //                .CreateCriteria("User.Tenant", "Tenant", JoinType.InnerJoin)
        //                .Add(Restrictions.Eq("User.Username", username))
        //                .Add(Restrictions.Eq("User.Password", encryptedPassword))
        //                .Add(Restrictions.Eq("Tenant.Id", tenantId))
        //                .SetCacheable(true)
        //                .SetFlushMode(NHibernate.FlushMode.Manual)
        //                .SetReadOnly(true)
        //                .UniqueResult<User>();

        //    return result;
        //}

        //public User UserWithUsername(Guid tenantId, string username)
        //{
        //    var result = Session.CreateCriteria<User>("User")
        //                .CreateCriteria("User.Tenant", "Tenant", JoinType.InnerJoin)
        //                .Add(Restrictions.Eq("User.Username", username))
        //                .Add(Restrictions.Eq("Tenant.Id", tenantId))
        //                .SetCacheable(true)
        //                .SetFlushMode(NHibernate.FlushMode.Manual)
        //                .UniqueResult<User>();

        //    return result;
        //}

        //public User UserWithUsernameWithNoCache(Guid tenantId, string username)
        //{
        //    var result = Session.CreateCriteria<User>("User")
        //                .CreateCriteria("User.Tenant", "Tenant", JoinType.InnerJoin)
        //                .Add(Restrictions.Eq("User.Username", username))
        //                .Add(Restrictions.Eq("Tenant.Id", tenantId))
        //                .SetCacheable(false)
        //                .SetFlushMode(NHibernate.FlushMode.Manual)
        //                .UniqueResult<User>();

        //    return result;
        //}

        //public User UserWithTenantAndUsername(string tenantNumber, string username)
        //{
        //    var result = Session.CreateCriteria<User>("User")
        //                .CreateCriteria("User.Tenant", "Tenant", JoinType.InnerJoin)
        //                .Add(Restrictions.Eq("User.Username", username))
        //                .Add(Restrictions.Eq("Tenant.Name", tenantNumber))
        //                .SetCacheable(true)
        //                .SetFlushMode(NHibernate.FlushMode.Manual)
        //                .UniqueResult<User>();

        //    return result;
        //}

        //public User UserWithUsername(Guid tenantId, string username, Boolean readOnly, bool cacheable)
        //{
        //    var result = Session.CreateCriteria<User>("User")
        //                .CreateCriteria("User.Tenant", "Tenant", JoinType.InnerJoin)
        //                .Add(Restrictions.Eq("User.Username", username))
        //                .Add(Restrictions.Eq("Tenant.Id", tenantId))
        //                .SetCacheable(cacheable)
        //                .SetFlushMode(NHibernate.FlushMode.Manual)
        //                .SetReadOnly(readOnly)
        //                .UniqueResult<User>();

        //    return result;
        //}

        //public User UserWithUsernameOrImpersonated(Guid tenantId, string username)
        //{
        //    var result = Session.CreateCriteria<User>("User")
        //               .CreateCriteria("User.Tenant", "Tenant", JoinType.InnerJoin)
        //               .Add(Restrictions.Eq("User.Username", username))
        //               .Add(Restrictions.Eq("Tenant.Id", tenantId))
        //               .SetCacheable(true)
        //               .SetFlushMode(NHibernate.FlushMode.Manual)
        //               .SetReadOnly(true)
        //               .UniqueResult<User>();

        //    if (result != null)
        //        return result;

        //    var parentTenant = Session.CreateCriteria<Tenant>("Tenant")
        //      .Add(Restrictions.Eq("Tenant.Id", tenantId))
        //      .SetProjection(Projections.Property<Tenant>(t => t.Parent.Id))
        //      .SetCacheable(true)
        //      .SetFlushMode(NHibernate.FlushMode.Manual)
        //      .SetReadOnly(true)
        //      .UniqueResult<Guid>();

        //    Check.NotNull(() => parentTenant, "La propiedad 'parentTenant' no puede ser null");

        //    var parentUser = Session.CreateCriteria<User>("User")
        //                .CreateCriteria("User.Tenant", "Tenant", JoinType.FullJoin)
        //                .Add(Restrictions.Eq("Tenant.Id", parentTenant))
        //                .Add(Restrictions.Eq("User.Username", username))
        //                .SetCacheable(true)
        //                .SetFlushMode(NHibernate.FlushMode.Manual)
        //                .SetReadOnly(true)
        //                .UniqueResult<User>();

        //    Check.NotNull(() => parentUser, "La propiedad 'parentUser' no puede ser null");

        //    var impersonatedUser = Session.CreateCriteria<User>("User")
        //                            .CreateCriteria("User.Tenant", "Tenant", JoinType.FullJoin)
        //                            .Add(Restrictions.Eq("Tenant.Id", tenantId))
        //                            .SetCacheable(true)
        //                            .SetFlushMode(NHibernate.FlushMode.Manual)
        //                            .SetReadOnly(true)
        //                            .UniqueResult<User>();

        //    return impersonatedUser;
        //}

        //public IEnumerable<User> UsersWithUsername(string username)
        //{
        //    var result = Session.CreateCriteria<User>("User")
        //                .CreateCriteria("User.Tenant", "Tenant", JoinType.InnerJoin)
        //                .Add(Restrictions.Eq("User.Username", username))
        //                .Add(Restrictions.Not(Restrictions.Eq("Tenant.Name", "FactureThirdPortal")))
        //                .List<User>();

        //    return result;
        //}

        //public IEnumerable<User> UsersWithUsernameThirdPortal(string username)
        //{
        //    var result = Session.CreateCriteria<User>("User")
        //                .CreateCriteria("User.Tenant", "Tenant", JoinType.InnerJoin)
        //                .Add(Restrictions.Eq("User.Username", username))
        //                .Add(Restrictions.Eq("Tenant.Name", "FactureThirdPortal"))
        //                .List<User>();

        //    return result;
        //}

        //public User UserWithUsername(string username)
        //{
        //    var result = Session.CreateCriteria<User>("User")
        //                .CreateCriteria("User.Tenant", "Tenant", JoinType.InnerJoin)
        //                .Add(Restrictions.Eq("User.Username", username))
        //                .Add(Restrictions.Not(Restrictions.Eq("Tenant.Name", "FactureThirdPortal")))
        //                .SetCacheable(true)
        //                .SetFlushMode(NHibernate.FlushMode.Manual)
        //                .SetMaxResults(1)
        //                .List<User>()
        //                .SingleOrDefault();

        //    return result;
        //}

        //public User UserWithUsernameAndTenantId(Guid tenantId, string username)
        //{
        //    var result = Session.CreateCriteria<User>("User")
        //                .CreateCriteria("User.Tenant", "Tenant", JoinType.InnerJoin)
        //                .Add(Restrictions.Eq("User.Username", username))
        //                .Add(Restrictions.Not(Restrictions.Eq("Tenant.Name", "FactureThirdPortal")))
        //                .Add(Restrictions.Eq("Tenant.Id", tenantId))
        //                .SetCacheable(true)
        //                .SetFlushMode(NHibernate.FlushMode.Manual)
        //                .SetMaxResults(1)
        //                .List<User>()
        //                .SingleOrDefault();

        //    return result;
        //}


        //public User UserWithUsernameWithNoCache(string username)
        //{
        //    var result = Session.CreateCriteria<User>("User")
        //                .CreateCriteria("User.Tenant", "Tenant", JoinType.InnerJoin)
        //                .Add(Restrictions.Eq("User.Username", username))
        //                .Add(Restrictions.Not(Restrictions.Eq("Tenant.Name", "FactureThirdPortal")))
        //                .SetCacheable(false)
        //                .SetFlushMode(NHibernate.FlushMode.Manual)
        //                .SetMaxResults(1)
        //                .List<User>()
        //                .SingleOrDefault();

        //    return result;
        //}


        //public User UserWithUsername(string portal, string username)
        //{
        //    var result = Session.CreateCriteria<User>("User")
        //               .Add(Restrictions.Eq("User.Username", username))
        //               .Add(Restrictions.Eq("User.Portal", portal))
        //               .SetCacheable(true)
        //               .SetFlushMode(NHibernate.FlushMode.Manual)
        //               .SetMaxResults(1)
        //               .List<User>()
        //               .SingleOrDefault();

        //    return result;
        //}

        //public User UserWithUsernameWithNoCache(string portal, string username)
        //{
        //    var result = Session.CreateCriteria<User>("User")
        //               .Add(Restrictions.Eq("User.Username", username))
        //               .Add(Restrictions.Eq("User.Portal", portal))
        //               .SetCacheable(false)
        //               .SetFlushMode(NHibernate.FlushMode.Manual)
        //               .SetMaxResults(1)
        //               .List<User>()
        //               .SingleOrDefault();

        //    return result;
        //}

        //public User UserWithUsernameWithNoCache(string portal, string username,Guid tenantId)
        //{
        //    var result = Session.CreateCriteria<User>("User")
        //                .CreateCriteria("User.Tenant", "Tenant", JoinType.InnerJoin)
        //               .Add(Restrictions.Eq("User.Username", username))
        //               .Add(Restrictions.Eq("User.Portal", portal))
        //                .Add(Restrictions.Eq("Tenant.Id", tenantId))
        //               .SetCacheable(false)
        //               .SetFlushMode(NHibernate.FlushMode.Manual)
        //               .SetMaxResults(1)
        //               .List<User>()
        //               .SingleOrDefault();

        //    return result;
        //}

        //public User FirstUserWithUsername(string username)
        //{
        //    var results = Session.CreateCriteria<User>("User")
        //                .Add(Restrictions.Eq("User.Username", username))
        //                .SetCacheable(true)
        //                .SetFlushMode(NHibernate.FlushMode.Manual)
        //                .Future<User>();

        //    return results.FirstOrDefault();
        //}

        //public IEnumerable<Tenant> TenantsFromUser(string username)
        //{
        //    var result = Session.CreateCriteria<User>("User")
        //                    .CreateCriteria("User.Tenant", "Tenant", JoinType.InnerJoin)
        //                    .Add(Restrictions.Eq("User.Username", username))
        //                    .Add(Restrictions.Eq("Tenant.Enablement.Enabled", true))
        //                    .Add(Restrictions.Not(Restrictions.Eq("Tenant.Name", "FactureThirdPortal")))
        //                    .SetCacheable(true)
        //                    .SetFlushMode(NHibernate.FlushMode.Manual)
        //                    .List<User>();

        //    return result.Select(t => t.Tenant);
        //}

        //public IEnumerable<User> GetAllUsers(Guid tenantId, Int32 pageIndex, Int32 pageSize, out Int32 rowCount)
        //{
        //    var RowCount = Session.CreateCriteria<User>("User")
        //        .CreateCriteria("User.Tenant", "Tenant", JoinType.InnerJoin)
        //        .Add(Restrictions.Eq("Tenant.Id", tenantId))
        //        //.Add(Restrictions.Not(Restrictions.Eq("Tenant.Name", "FactureThirdPortal")))
        //        .SetProjection(Projections.RowCount())
        //        .FutureValue<Int32>();

        //    rowCount = RowCount.Value;

        //    var result = Session.CreateCriteria<User>("User")
        //        .CreateCriteria("User.Tenant", "Tenant", JoinType.InnerJoin)
        //        .Add(Restrictions.Eq("Tenant.Id", tenantId))
        //        .Add(Restrictions.Not(Restrictions.Eq("Tenant.Name", "FactureThirdPortal")))
        //        .SetCacheable(true)
        //        .SetFlushMode(NHibernate.FlushMode.Manual)
        //        .SetReadOnly(true)
        //        .Future<User>();

        //    return result;
        //}

        //public User UserFromUserId(Guid? tenantId, int userId)
        //{
        //    var query = Session.CreateCriteria<User>("User")
        //        .CreateCriteria("User.Tenant", "Tenant", JoinType.InnerJoin)
        //        .Add(Restrictions.Eq("User.Id", userId));

        //    if (tenantId.HasValue)
        //        query.Add(Restrictions.Eq("Tenant.Id", tenantId.Value));

        //    var result = query
        //                .SetCacheable(true)
        //                .SetFlushMode(NHibernate.FlushMode.Manual)
        //                .SetReadOnly(true)
        //                .UniqueResult<User>();

        //    return result;
        //}

        //public User UserWithEmail(string emailAddress)
        //{
        //    var result = Session.CreateCriteria<User>("User")
        //                .CreateCriteria("User.Person", "Person", JoinType.InnerJoin)
        //                .Add(Restrictions.Eq("Person.ContactInformation.EmailAddress", emailAddress))
        //                .SetCacheable(true)
        //                .SetFlushMode(NHibernate.FlushMode.Manual)
        //                .SetReadOnly(true)
        //                .UniqueResult<User>();

        //    return result;
        //}

        //public User UserFromUserId(int userId)
        //{
        //    var query = Session.CreateCriteria<User>("User")
        //        .Add(Restrictions.Eq("User.Id", userId));


        //    var result = query
        //                .SetCacheable(true)
        //                .SetFlushMode(NHibernate.FlushMode.Manual)
        //                .SetReadOnly(true)
        //                .UniqueResult<User>();

        //    return result;
        //}

        //public IEnumerable<User> FindSSOUsers(Guid tenantId)
        //{
        //    var result = Session.CreateCriteria<User>("User")
        //                .CreateCriteria("User.Tenant", "Tenant", JoinType.InnerJoin)
        //                .Add(Restrictions.Eq("User.IsSSO", true))
        //                .Add(Restrictions.Eq("Tenant.Id", tenantId))
        //                .List<User>();

        //    return result;
        //}

        public User Get(int id)
        {
            throw new NotImplementedException();
        }

        public IList<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public User SaveOrUpdate(User entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(User entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public User UserFromAuthenticCredentials(Guid tenantId, string username, string encryptedPassword)
        {
            throw new NotImplementedException();
        }

        public User UserFromAuthenticCredentials(string username, string encryptedPassword)
        {
            throw new NotImplementedException();
        }

        public User UserWithUsername(Guid tenantId, string username)
        {
            throw new NotImplementedException();
        }

        public User UserWithUsernameWithNoCache(Guid tenantId, string username)
        {
            throw new NotImplementedException();
        }

        public User UserWithTenantAndUsername(string tenantNumber, string username)
        {
            throw new NotImplementedException();
        }

        public User UserWithUsername(Guid tenantId, string username, bool readOnly, bool cacheable)
        {
            throw new NotImplementedException();
        }

        public User UserWithUsername(string username)
        {
            throw new NotImplementedException();
        }

        public User UserWithUsernameWithNoCache(string username)
        {
            throw new NotImplementedException();
        }

        public User UserWithUsername(string portal, string username)
        {
            throw new NotImplementedException();
        }

        public User UserWithUsernameWithNoCache(string portal, string username)
        {
            throw new NotImplementedException();
        }

        public User UserWithUsernameWithNoCache(string portal, string username, Guid tenantId)
        {
            throw new NotImplementedException();
        }

        public User UserWithUsernameOrImpersonated(Guid tenantId, string username)
        {
            throw new NotImplementedException();
        }

        public User UserWithEmail(string emailAddress)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tenant> TenantsFromUser(string username)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAllUsers(Guid tenantId, int pageIndex, int pageSize, out int rowCount)
        {
            throw new NotImplementedException();
        }

        public User UserFromUserId(Guid? tenantId, int userId)
        {
            throw new NotImplementedException();
        }

        public User UserFromUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public User FirstUserWithUsername(string username)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> UsersWithUsername(string username)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> UsersWithUsernameThirdPortal(string username)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> FindSSOUsers(Guid tenantId)
        {
            throw new NotImplementedException();
        }

        public User UserWithUsernameAndTenantId(Guid tenantId, string username)
        {
            throw new NotImplementedException();
        }
    }
}
