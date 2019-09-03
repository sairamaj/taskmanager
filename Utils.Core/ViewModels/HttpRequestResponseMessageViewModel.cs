using System;
using System.Diagnostics;
using System.Net.Http;
using Utils.Core.Extensions;

namespace Utils.Core.ViewModels
{
    public class HttpRequestResponseMessageViewModel
    {
        private static int Counter = 1;
        private Stopwatch _watch;

        public HttpRequestResponseMessageViewModel(HttpRequestMessage request)
        {
            this.RequestMessage = request.ToCustomString();
            this.RequestUri = request.RequestUri == (Uri)null ? "<null>" : request.RequestUri.ToString();
            this.Method = request.Method.ToString();
            this.Id = Counter++;
            this._watch = new Stopwatch();
            this._watch.Start();
        }
        public string RequestUri { get; set; }
        public string RequestMessage { get; set; }
        public string ResponseMessage { get; set; }
        public string Method { get; set; }
        public string HttpStatusCodeString { get; set; }
        public int HttpStatusCode { get; set; }
        public string ReasonPhrase { get; set; }
        public int Id { get; set; }
        public long TimeTakenInMilliSeconds { get; set; }

        public void AddResponse(HttpResponseMessage response)
        {
            this._watch.Stop();
            this.ResponseMessage = response.ToCustomString();
            // this.SummaryText = this.SummaryText + "  " + response.GetTitle();
            this.HttpStatusCodeString = response.StatusCode.ToString();
            this.HttpStatusCode = (int)response.StatusCode;
            this.ReasonPhrase = response.ReasonPhrase ?? "<null>";
            this.TimeTakenInMilliSeconds = this._watch.ElapsedMilliseconds;

            //OnPropertyChanged2(() => this.ResponseMessage);
            //OnPropertyChanged2(() => this.HttpStatusCode);
            //OnPropertyChanged2(() => this.HttpStatusCodeString);
            //OnPropertyChanged2(() => this.ReasonPhrase);
            //OnPropertyChanged2(() => this.TimeTakenInMilliSeconds);

        }

    }

}
