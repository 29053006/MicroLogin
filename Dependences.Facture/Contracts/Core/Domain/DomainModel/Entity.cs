using System;

namespace Facture.Core.Domain.DomainModel
{
    /// <summary>
    ///     Provides a base class for your objects which will be persisted to the database.
    ///     Benefits include the addition of an Id property along with a consistent manner for comparing
    ///     entities.
    /// 
    ///     Since nearly all of the entities you create will have a type of int Id, this 
    ///     base class leverages this assumption.  If you want an entity with a type other 
    ///     than int, such as string, then use <see cref = "EntityWithTypedId{IdT}" /> instead.
    /// </summary>
    [Serializable]
    public abstract class Entity : EntityWithTypedId<Guid>
    {
        public static class Patterns
        {
            public const string Code = @"[a-zA-Z0-9_-]+";
            public const string IPAddress = @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5]).){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$";
        }
    }
}