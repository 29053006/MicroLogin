using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace Facture.Core.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        public static readonly string KEY_TRACE = "X-Trace";


        public static bool IsTraceHeaderEnabled(this HttpRequestMessage request)
        {
            if (request.Headers == null) { return false; }

            IEnumerable<string> headerValues;
            var keyFound = request.Headers.TryGetValues(KEY_TRACE, out headerValues);
            if (!keyFound) { return false; }

            return headerValues.FirstOrDefault() == Boolean.TrueString;
        }


        public static HttpResponseMessage Download(this HttpRequest request, Byte[] content, String fileName = "")
        {
            var response = new HttpResponseMessage() { StatusCode = HttpStatusCode.OK };
            response.Content = new StreamContent(new MemoryStream(content));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            if (!string.IsNullOrEmpty(fileName))
            {
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileName
                };
            }

            return response;
        }

        public static HttpResponseMessage DownloadFromFile(this HttpRequest request, string path, String downloadFileName = "")
        {
            var response = new HttpResponseMessage() { StatusCode = HttpStatusCode.OK };
            response.Content = new StreamContent(File.OpenRead(path), bufferSize: 4096);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            if (!string.IsNullOrEmpty(downloadFileName))
            {
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = downloadFileName };
            }

            return response;
        }

        public static HttpResponseMessage Download(this HttpRequest request, String content, String fileName = "")
        {
            var response = new HttpResponseMessage() { StatusCode = HttpStatusCode.OK };
            response.Content = new StringContent(content, Encoding.UTF8);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            if (!string.IsNullOrEmpty(fileName))
            {
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileName
                };
            }

            return response;
        }
    }
}
