using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Facture.Core.Helpers
{
    public class EventItem
    {
        [JsonRequired]
        [JsonProperty("errorCode")]
        public ErrorResponseCode ErrorCode { get; set; }

        [JsonRequired]
        [JsonProperty("severityCode")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SeverityCode SeverityCode { get; set; }

        [StringLength(maximumLength: 100)]
        [JsonProperty("shortDescription")]
        public String ShortDescription { get; set; }

        [JsonProperty("detailDescription", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Details { get; set; }

        [JsonProperty("parameters", NullValueHandling = NullValueHandling.Ignore)]
        public List<Parameter> Parameters { get; set; }

        [JsonProperty("locations", NullValueHandling = NullValueHandling.Ignore)]
        public List<Location> Locations { get; set; }
    }
}
