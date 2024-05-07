using System;

namespace Facture.Core.Domain.Events
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PriorityAttribute : Attribute
    {
        public Int32 Level { get; set; }

        public PriorityAttribute(Int32 level = 0)
        {
            this.Level = level;
        }
    }
}
