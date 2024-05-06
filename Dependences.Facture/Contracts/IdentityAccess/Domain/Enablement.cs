using Facture.Core.Domain.DomainModel;
using Facture.IdentityAccess.Application;
using System.Data.SqlTypes;

namespace Facture.IdentityAccess.Domain
{
    public class Enablement : ValueObject
    {
        public static Enablement IndefiniteEnablement()
        {
            return new Enablement(false, SqlDateTime.MinValue.Value, SqlDateTime.MinValue.Value);
        }

        public static Enablement FromNow
        {
            get
            {
                return new Enablement(true, DateTimeOffset.Now, null);
            }
        }

        public Enablement(bool enabled, DateTimeOffset startDate, DateTimeOffset? endDate)
        {
            if (startDate > endDate)
            {
                throw new InvalidOperationException(SN.EnablementInvalidDateRange);
            }

            this.Enabled = enabled;
            this.EndDate = endDate;
            this.StartDate = startDate;
        }

        protected Enablement()
        {

        }

        public virtual bool Enabled { get; set; }

        public virtual Nullable<DateTimeOffset> EndDate { get; set; }

        public virtual DateTimeOffset StartDate { get; set; }

        public virtual Nullable<DateTimeOffset> LastModified { get; set; }

        public virtual String IP { get; set; }

        public virtual String Reason { get; set; }

        public virtual String Description { get; set; }

        public virtual Boolean IsEnablementEnabled()
        {
            bool enabled = false;

            if (this.Enabled)
            {
                if (!this.IsTimeExpired())
                {
                    enabled = true;
                }
            }

            return enabled;
        }

        private Boolean IsTimeExpired()
        {
            bool timeExpired = false;

            if (this.StartDate != DateTimeOffset.MinValue && this.EndDate != DateTimeOffset.MinValue)
            {
                DateTimeOffset now = DateTimeOffset.Now;
                if (now < this.StartDate || now > this.EndDate)
                {
                    timeExpired = true;
                }
            }

            return timeExpired;
        }

        public override Boolean Equals(Object anotherObject)
        {
            bool equalObjects = false;

            if (anotherObject != null && this.GetType() == anotherObject.GetType())
            {
                Enablement typedObject = (Enablement)anotherObject;
                equalObjects =
                    this.Enabled == typedObject.Enabled &&
                    this.StartDate == typedObject.StartDate &&
                    this.EndDate == typedObject.EndDate;
            }

            return equalObjects;
        }

        public override Int32 GetHashCode()
        {
            int hashCodeValue =
                +(19563 * 181)
                + (this.Enabled ? 1 : 0)
                + this.StartDate.GetHashCode()
                + (this.EndDate == null ? 0 : this.EndDate.GetHashCode());

            return hashCodeValue;
        }

        public override string ToString()
        {
            return "Enablement [enabled=" + Enabled + ", endDate=" + EndDate + ", startDate=" + StartDate + "]";
        }
    }
}
