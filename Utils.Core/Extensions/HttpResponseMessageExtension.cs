using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Utils.Core.Extensions
{
    internal static class HttpResponseMessageExtension
    {
        public static string ToCustomString(this HttpResponseMessage responseMessage)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("StatusCode: ");
            stringBuilder.Append((int)responseMessage.StatusCode);
            stringBuilder.Append(", ReasonPhrase: '");
            stringBuilder.Append(responseMessage.ReasonPhrase ?? "<null>");
            stringBuilder.Append("', Version: ");
            stringBuilder.Append((object)responseMessage.Version);

            stringBuilder.Append("\r\n");
            stringBuilder.Append("Headers:\r\n");
            stringBuilder.Append(GetHeaders((HttpHeaders)responseMessage.Headers, (HttpHeaders)(responseMessage.Content == null ? (HttpContentHeaders)null : responseMessage.Content.Headers)));
            stringBuilder.Append("\r\n");
            if (responseMessage.Content != null)
            {
                stringBuilder.Append(", Content: " + System.Environment.NewLine);
                stringBuilder.Append(responseMessage.Content.ReadAsStringAsync().Result);
                stringBuilder.Append(System.Environment.NewLine);
            }

            return stringBuilder.ToString();
        }

        public static string GetTitle(this HttpResponseMessage responseMessage)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("StatusCode: ");
            stringBuilder.Append((int)responseMessage.StatusCode);
            stringBuilder.Append(", ReasonPhrase: '");
            stringBuilder.Append(responseMessage.ReasonPhrase ?? "<null>");
            stringBuilder.Append("', Version: ");
            stringBuilder.Append((object)responseMessage.Version);

            return stringBuilder.ToString();
        }

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
            //  stringBuilder.Append('}');
            return stringBuilder.ToString();
        }
    }
}
