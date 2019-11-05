using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Utils.Core.ViewModels
{
    /// <summary>
    /// Http message view model.
    /// </summary>
    public class HttpMessagesViewModel : HttpClientHandler
    {
        /// <summary>
        /// Single instance.
        /// </summary>
        private static readonly HttpMessagesViewModel _instance = new HttpMessagesViewModel();

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpMessagesViewModel"/> class.
        /// </summary>
        public HttpMessagesViewModel()
        {
            this.HttpRequestResponses = new SafeObservableCollection<HttpRequestResponseMessageViewModel>();
        }

        /// <summary>
        /// Gets instance.
        /// </summary>
        public static HttpMessagesViewModel Instance => _instance;

        /// <summary>
        /// Gets or sets Http request and responses.
        /// </summary>
        public ObservableCollection<HttpRequestResponseMessageViewModel> HttpRequestResponses { get; set; }

        /// <summary>
        /// Send http request.
        /// </summary>
        /// <param name="request">
        /// A <see cref="HttpRequestMessage"/> instance.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> instance.
        /// </param>
        /// <returns>
        /// A <see cref="Task{T}"/> of <see cref="HttpResponseMessage"/> instance.
        /// </returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var requestResponseMessageViewModel = new HttpRequestResponseMessageViewModel(request);
            Trace.WriteLine($"[TASKManager] {requestResponseMessageViewModel.RequestMessage}");
            var task = base.SendAsync(request, cancellationToken);
            task.ContinueWith(
                t =>
            {
                requestResponseMessageViewModel.AddResponse(t.Result);
                Trace.WriteLine($"[TASKManager] {requestResponseMessageViewModel.ResponseMessage}");
            }, cancellationToken);

            this.HttpRequestResponses.Add(requestResponseMessageViewModel);
            return task;
        }
    }
}
