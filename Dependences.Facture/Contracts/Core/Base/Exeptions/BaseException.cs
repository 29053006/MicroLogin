using System;
using System.Net;
using System.Runtime.Serialization;
using System.Security;

namespace Facture.Core.Exceptions
{
    public abstract class BaseException : Exception
    {
        public BaseException() : base() { }

        public BaseException(string message) : base(message) { }

        [SecuritySafeCritical]
        public BaseException(SerializationInfo info, StreamingContext context) : base(info: info, context: context) { }

        public BaseException(string message, Exception innerException) : base(message, innerException) { }


        public virtual HttpStatusCode StatusCode { get { return HttpStatusCode.BadRequest; } }
    }
}
