using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Facture.Core.Extensions
{
    public static class ExceptionExtensions
    {

        /// <summary>
        /// Gets a delegate method that extracts information from the specified exception.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string ToFullMessage(this Exception exception)
        {
            var errorList = new List<string>();
            if (exception.InnerException != null) { errorList.Add(exception.InnerException.ToFullMessage()); }

            errorList.Add(GetMessage(exception));

            return string.Join(" | ", errorList);
        }

        public static string[] ToFullDetails(this Exception exception)
        {
            var errorList = new List<string>();
            if (exception.InnerException != null) { errorList.AddRange(exception.InnerException.ToFullDetails()); }

            errorList.Add(GetMessage(exception));
            errorList.AddRange(RemoveInnerDotNetCalls(exception.StackTrace));

            return errorList.ToArray();
        }

        #region Private
        private static readonly List<string> _ExcludedExceptionList = new List<string> {
            "SqlException",
            "AssertionException",
            "CheckInvalidOperationException",
            "InvariantException",
            "PostconditionException",
            "PreconditionException",
            "RepExtException",
            "IssueException",
            "StageException",
            "ValidationException",
        };

        private static string GetMessage(Exception ex)
        {
            var message = ex.Message;

            var exceptionName = ex.GetType().Name;
            if (!_ExcludedExceptionList.Contains(exceptionName))
            { message = string.Format("{0}: {1}", exceptionName.Replace("Exception", ""), message); }

            return message;
        }


        private const string PATTERN_SYSTEM_CALLS_STACKTRACE = "(at|en)" +
                                                      " " +
                                                      "(System|Microsoft)" +
                                                      @"\.";
        private static readonly Regex _RegexSystemStackTrace = new Regex(PATTERN_SYSTEM_CALLS_STACKTRACE, RegexOptions.IgnoreCase);

        private static readonly Regex _RegexClassFileSeparator = new Regex(@" (at|en) [A-Z]:\\", RegexOptions.IgnoreCase);
        private static readonly Regex _RegexClassLineSeparator = new Regex(@":(line|línea)", RegexOptions.IgnoreCase);


        private static string[] RemoveInnerDotNetCalls(string text)
        {
            if (text == null) { return new string[] { }; }
            var outputLines = new List<string>();
            var textLines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in textLines)
            {
                var shouldRemove = _RegexSystemStackTrace.IsMatch(line)
                                || line.Contains("--- End of stack trace")
                                || line.Contains("--- Fin del seguimiento de la pila");

                if (shouldRemove) { continue; }

                var cleanedLine = _RegexClassFileSeparator.Replace(line, Environment.NewLine + "     $0");
                cleanedLine = _RegexClassLineSeparator.Replace(cleanedLine, Environment.NewLine + "      $0");
                cleanedLine = cleanedLine.Replace(@"\", @"/");

                var sublines = cleanedLine.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                outputLines.AddRange(sublines);
            }

            // for inner exceptions on .net framework code
            if (outputLines.Count == 0 && textLines.Length > 0) { outputLines.AddRange(textLines); }

            return outputLines.ToArray();
        }
        #endregion
    }
}
