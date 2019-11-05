using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Utils.Core.Extensions
{
    /// <summary>
    /// Http request message extensions.
    /// </summary>
    internal static class HttpRequestMessageExtension
    {
        /// <summary>
        /// Formats request to get a string format.
        /// </summary>
        /// <param name="requestMessage">
        /// A <see cref="HttpRequestMessage"/> class.
        /// </param>
        /// <returns>
        /// String representation of the message.
        /// </returns>
        public static string ToCustomString(this HttpRequestMessage requestMessage)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(requestMessage.Method);
            stringBuilder.Append(" ");

            stringBuilder.Append(requestMessage.RequestUri == (Uri)null
                ? "<null>"
                : requestMessage.RequestUri.ToString());
            stringBuilder.Append(System.Environment.NewLine);
            stringBuilder.Append(
                GetHeaders(
                    requestMessage.Headers,
                    requestMessage.Content?.Headers));

            stringBuilder.Append(System.Environment.NewLine);
            if (requestMessage.Content != null)
            {
                stringBuilder.Append(requestMessage.Content.ReadAsStringAsync().Result);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Gets title representation of the http request message.
        /// </summary>
        /// <param name="requestMessage">
        /// A <see cref="HttpRequestMessage"/> instance.
        /// </param>
        /// <returns>
        /// Returns title of the request message.
        /// </returns>
        public static string GetTitle(this HttpRequestMessage requestMessage)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(requestMessage.RequestUri == (Uri)null
                ? "<null>"
                : requestMessage.RequestUri.ToString());
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Gets headers in string format.
        /// </summary>
        /// <param name="headers">
        /// Http headers instance.
        /// </param>
        /// <returns>
        /// String representation of the Http request.
        /// </returns>
        internal static string GetHeaders(params HttpHeaders[] headers)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(System.Environment.NewLine);
            for (int index = 0; index < headers.Length; ++index)
            {
                if (headers[index] != null)
                {
                    foreach (KeyValuePair<string, IEnumerable<string>> keyValuePair in headers[index])
                    {
                        foreach (string str in keyValuePair.Value)
                        {
                            stringBuilder.Append("  ");
                            stringBuilder.Append(keyValuePair.Key);
                            stringBuilder.Append(": ");
                            stringBuilder.Append(str);
                            stringBuilder.Append("\r\n");
                        }
                    }
                }
            }

            return stringBuilder.ToString();
        }
    }
}
