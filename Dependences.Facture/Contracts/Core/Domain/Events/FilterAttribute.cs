using System;

namespace Facture.Core.Domain.Events
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class FilterAttribute : Attribute
    {
        public String Expression { get; set; }

        public FilterAttribute(String expression)
        {
            this.Expression = expression;
        }
    }
}