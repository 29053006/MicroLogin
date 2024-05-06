using Facture.Core.Exceptions;
using System;

namespace Facture.Core.Domain
{

    /// <summary>
    ///     Exception raised when a contract is broken.
    ///     Catch this exception type if you wish to differentiate between 
    ///     any DesignByContract exception and other runtime exceptions.
    /// </summary>
    public class DesignByContractException : BaseException
    {
        protected DesignByContractException() : base() { }

        protected DesignByContractException(string message)
            : base(message)
        {
        }

        protected DesignByContractException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}