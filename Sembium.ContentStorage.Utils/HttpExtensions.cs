using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Utils
{
    public static class HttpExtensions
    {
        public static async Task<HttpResponseMessage> CheckedSendAsync(this HttpClient httpClient, HttpRequestMessage request, HttpCompletionOption httpCompletionOption = HttpCompletionOption.ResponseContentRead)
        {
            var response = await httpClient.SendAsync(request);

            await response.CheckSuccessAsync();

            return response;
        }

        public static async Task CheckSuccessAsync(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var newExceptionMessage = response.ReasonPhrase + " " + (int)response.StatusCode;

                var responseErrorMessage = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(responseErrorMessage))
                {
                    newExceptionMessage = newExceptionMessage + Environment.NewLine + responseErrorMessage.Substring(0, Math.Min(responseErrorMessage.Length, 1000));
                }

                throw new HttpRequestException(newExceptionMessage);
            }
        }

        public static async Task<HttpResponseMessage> CheckedGetAsync(this HttpClient httpClient, string url, HttpCompletionOption httpCompletionOption = HttpCompletionOption.ResponseContentRead, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                request.SetHeaders(headers);

                return await httpClient.CheckedSendAsync(request, httpCompletionOption);
            }
        }

        public static async Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, string url, HttpCompletionOption httpCompletionOption = HttpCompletionOption.ResponseContentRead, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                request.SetHeaders(headers);

                return await httpClient.SendAsync(request, httpCompletionOption);
            }
        }

        public static async Task<string> CheckedGetStringAsync(this HttpClient httpClient, string url, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var response = await httpClient.CheckedGetAsync(url, headers: headers);
            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<System.IO.Stream> CheckedGetStreamAsync(this HttpClient httpClient, string url, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var response = await httpClient.CheckedGetAsync(url, HttpCompletionOption.ResponseHeadersRead, headers);
            return await response.Content.ReadAsStreamAsync();
        }

        public static void SetHeaders(this HttpRequestMessage request, IEnumerable<KeyValuePair<string, string>> headers)
        {
            request.Headers.Clear();
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
        }
    }
}
