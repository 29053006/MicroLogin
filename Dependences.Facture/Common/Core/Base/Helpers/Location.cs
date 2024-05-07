using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Facture.Core.Helpers
{
    public class Location
    {
        [JsonRequired]
        [JsonProperty("identifier")]
        public String Identifier { get; set; }

        [JsonProperty("text")]
        public String Text { get; set; }
    }

    public static class LocationExtensions
    {
        public static Location MapToLocation(this KeyValuePair<String, String> self)
        {
            var l = new Location
            {
                Identifier = self.Key,
                Text = self.Value
            };
            return l;
        }
    }
}
