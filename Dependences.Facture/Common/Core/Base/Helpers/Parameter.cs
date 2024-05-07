using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Facture.Core.Helpers
{
    public class Parameter
    {
        [JsonRequired]
        [StringLength(maximumLength: 80)]
        [JsonProperty("identifier")]
        public String Identifier { get; set; }

        [JsonRequired]
        [StringLength(maximumLength: 4096)]
        [JsonProperty("text")]
        public String Text { get; set; }
    }

    public static class ParameterExtensions
    {
        public static Parameter MapToParameter(this KeyValuePair<String, String> self)
        {
            var p = new Parameter
            {
                Identifier = self.Key,
                Text = self.Value
            };
            return p;
        }
    }
}
