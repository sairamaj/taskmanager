using System;
using System.Diagnostics;
using System.Net.Http;
using Utils.Core.Extensions;

namespace Utils.Core.ViewModels
{
    /// <summary>
    /// Http request response view model.
    /// </summary>
    public class HttpRequestResponseMessageViewModel
    {
        /// <summary>
        /// Counter to keep requests.
        /// </summary>
        private static int Counter = 1;

        /// <summary>
        /// Measuring the time.
        /// </summary>
        private readonly Stopwatch _watch;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequestResponseMessageViewModel"/> class.
        /// </summary>
        /// <param name="request">
        /// A <see cref="HttpRequestMessage"/> instance.
        /// </param>
        public HttpRequestResponseMessageViewModel(HttpRequestMessage request)
        {
            this.RequestMessage = request.ToCustomString();
            this.RequestUri = request.RequestUri == (Uri)null ? "<null>" : request.RequestUri.ToString();
            this.Method = request.Method.ToString();
            this.Id = Counter++;
            this._watch = new Stopwatch();
            this._watch.Start();
        }

        /// <summary>
        /// Gets or sets request uri.
        /// </summary>
        public string RequestUri { get; set; }

        /// <summary>
        /// Gets or sets request message.
        /// </summary>
        public string RequestMessage { get; set; }

        /// <summary>
        /// Gets or sets response message.
        /// </summary>
        public string ResponseMessage { get; set; }

        /// <summary>
        /// Gets or sets http method.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Gets or sets http status code string.
        /// </summary>
        public string HttpStatusCodeString { get; set; }

        /// <summary>
        /// Gets or sets http status code.
        /// </summary>
        public int HttpStatusCode { get; set; }

        /// <summary>
        /// Gets or sets reason phrase.
        /// </summary>
        public string ReasonPhrase { get; set; }

        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets time taken in milliseconds.
        /// </summary>
        public long TimeTakenInMilliSeconds { get; set; }

        /// <summary>
        /// Adds response to individual properties.
        /// </summary>
        /// <param name="response">
        /// A <see cref="HttpRequestMessage"/>.
        /// </param>
        public void AddResponse(HttpResponseMessage response)
        {
            this._watch.Stop();
            this.ResponseMessage = response.ToCustomString();
            // this.SummaryText = this.SummaryText + "  " + response.GetTitle();
            this.HttpStatusCodeString = response.StatusCode.ToString();
            this.HttpStatusCode = (int)response.StatusCode;
            this.ReasonPhrase = response.ReasonPhrase ?? "<null>";
            this.TimeTakenInMilliSeconds = this._watch.ElapsedMilliseconds;
        }
    }
}