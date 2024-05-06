using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLColab.Contracts.Internal.Base.Contracts.Model.RazonSocialFactureDian
{
    public class RazonSocialFactureModel
    {
        public bool Success { get; set; }
        public string TenantName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string FirstSurName { get; set; }
        public string SecondSurName { get; set; }
        public int? VerificationDigit { get; set; }
        public string LegalConstitution { get; set; }
        public string IdentificationType { get; set; }
    }
}
