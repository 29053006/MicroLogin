using System;

namespace Facture.Core.Domain
{
    /// <summary>
    ///     Exception raised when an valid operation fails.
    /// </summary>
    public class CheckInvalidOperationException : DesignByContractException
    {
        /// <summary>
        ///     Invalid Exception.
        /// </summary>
        public CheckInvalidOperationException()
        {
        }

        /// <summary>
        ///     Invalid Exception.
        /// </summary>
        public CheckInvalidOperationException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Invalid Exception.
        /// </summary>
        public CheckInvalidOperationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
