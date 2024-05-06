namespace Facture.Core.Domain.PersistenceSupport
{
    using System;

    /// <summary>
    ///     Note that outside of CommitChanges(), you shouldn't have to invoke this object very often.  
    ///     If you're using the <see cref = "Facture.Core.Domain.Web.NHibernate.TransactionAttribute" /> on your 
    ///     controller actions, then the transaction opening/committing will be taken care of for you.
    /// </summary>
    public interface IDbContext
    {
        IDisposable BeginTransaction();

        void CommitChanges();

        void CommitTransaction();

        void RollbackTransaction();
    }
}