using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Utils.Core.ViewModels
{
    public class HttpMessagesViewModel : HttpClientHandler
    {
        private static HttpMessagesViewModel _instance = new HttpMessagesViewModel();
        public HttpMessagesViewModel()
        {
            this.HttpRequestResponses = new SafeObservableCollection<HttpRequestResponseMessageViewModel>();
        }

        public static HttpMessagesViewModel Instance => _instance;

        public ObservableCollection<HttpRequestResponseMessageViewModel> HttpRequestResponses { get; set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var requestResponseMessageViewModel = new HttpRequestResponseMessageViewModel(request);
            Trace.WriteLine($"[TASKManager] {requestResponseMessageViewModel.RequestMessage}");
            var task = base.SendAsync(request, cancellationToken);
            task.ContinueWith(t =>
            {
                //this._viewModel.AddMessage(t.Result.ToCustomString());
                requestResponseMessageViewModel.AddResponse(t.Result);
                Trace.WriteLine($"[TASKManager] {requestResponseMessageViewModel.ResponseMessage}");
            }, cancellationToken);

            HttpRequestResponses.Add(requestResponseMessageViewModel);
            return task;
        }
    }
}
