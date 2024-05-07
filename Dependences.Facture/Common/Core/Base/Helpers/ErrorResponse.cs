using Facture.Core.Extensions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Net;

namespace Facture.Core.Helpers
{
    public class ErrorResponse
    {
        public static ErrorResponse FromCodeAndMessage(ErrorResponseCode code, Exception exception, HttpRequest request)
        {
            var message = exception.ToFullMessage();
            var response = new ErrorResponse(code, message);
            return response;
        }

        public static HttpResponseMessage ToBadRequestResponse(ErrorResponseCode code, Exception exception, HttpRequest request)
        {
            var error = FromCodeAndMessage(code: ErrorResponseCode.InvalidInput, exception: exception, request: request);
            return new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest , Content = new StringContent(JsonConvert.SerializeObject(error)) };
        }



        public ErrorResponse(ErrorResponseCode code, String message, string[] details = null)
        {
            MaximumSeverityCode = SeverityCode.Error;
            EventItems = new List<EventItem>
            {
                new EventItem
                {
                    ErrorCode = code,
                    SeverityCode = SeverityCode.Error,
                    ShortDescription = message,
                    Details = details,
                    Parameters = null,
                    Locations = null
                }
            };
        }

        [JsonRequired]
        [JsonProperty("maximumSeverityCode")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SeverityCode MaximumSeverityCode { get; set; }

        [JsonRequired]
        [JsonProperty("eventItems")]
        public List<EventItem> EventItems { get; set; }
    }
}
