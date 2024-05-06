using Facture.Core.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dependences.Facture.Contracts
{
    [DebuggerStepThrough]
    public class Check
    {
        public static void Assert(bool assertion, string message)
        {
            if (!assertion) { throw new AssertionException(message); }
        }

        public static void Ensure(bool assertion, string message)
        {
            if (!assertion) { throw new PostconditionException(message); }
        }

        public static void Require(bool assertion, string message)
        {
            if (!assertion) { throw new PreconditionException(message); }
        }

        public static void NotNull<T>(Func<T> memberExpression, string message)
        {
            var errorMessage = message ?? "The Property should not be Null";
            var object1 = memberExpression();

            if (object1 == null) { throw new CheckInvalidOperationException(errorMessage); }
        }

        public static void NotNull(object object1, string message)
        {
            if (object1 == null) { throw new CheckInvalidOperationException(message); }
        }

        public static void Null<T>(Func<T> memberExpression, string message)
        {
            var errorMessage = message ?? "The Property should not be Null";
            var object1 = memberExpression();

            if (object1 != null) { throw new CheckInvalidOperationException(errorMessage); }
        }

        public static void Null(object object1, string message)
        {
            if (object1 != null) { throw new CheckInvalidOperationException(message); }
        }

        public static void NotEquals(object object1, object object2, string message)
        {
            if (object1.Equals(object2)) { throw new CheckInvalidOperationException(message); }
        }

        public static void Equals(object object1, object object2, string message)
        {
            if (!object1.Equals(object2)) { throw new CheckInvalidOperationException(message); }
        }

        public static void NotEmpty(Func<string> memberExpression, string message)
        {
            var errorMessage = message ?? "The Property should not be Null";
            var stringValue = memberExpression();

            if (string.IsNullOrWhiteSpace(stringValue)) { throw new CheckInvalidOperationException(errorMessage); }
        }

        public static void NotEmpty(string stringValue, string message)
        {
            if (string.IsNullOrWhiteSpace(stringValue)) { throw new CheckInvalidOperationException(message); }
        }

        public static void NotEmptyArrayString(string stringValue, string message)
        {
            if (stringValue.Trim() == "[]") { throw new CheckInvalidOperationException(message); }
        }

        public static void Empty<T>(Func<T> memberExpression, string message) where T : IEnumerable
        {
            var errorMessage = message ?? "The Property should not be Null";
            var collection = memberExpression();

            if (collection.GetEnumerator().MoveNext()) { throw new CheckInvalidOperationException(errorMessage); }
        }

        public static void Empty(IEnumerable collection, string message)
        {
            if (collection.GetEnumerator().MoveNext()) { throw new CheckInvalidOperationException(message); }
        }

        public static void NotEmpty<T>(Func<T> memberExpression, string message) where T : IEnumerable
        {
            var errorMessage = message ?? "The Property should not be Null";
            var collection = memberExpression();

            if (!collection.GetEnumerator().MoveNext()) { throw new CheckInvalidOperationException(errorMessage); }
        }

        public static void NotEmpty(IEnumerable collection, string message)
        {
            if (!collection.GetEnumerator().MoveNext()) { throw new CheckInvalidOperationException(message); }
        }

        public static void Fail(string message) => throw new CheckInvalidOperationException(message);
    }
}
