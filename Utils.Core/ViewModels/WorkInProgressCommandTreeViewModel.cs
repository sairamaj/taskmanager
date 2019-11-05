using System.Threading;

namespace Utils.Core.ViewModels
{
    /// <summary>
    /// The work in progress command tree view model.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "Background thread is used")]
    public class WorkInProgressCommandTreeViewModel : CommandTreeViewModel
    {
        /// <summary>
        /// The _stop event.
        /// </summary>
        private readonly AutoResetEvent _stopEvent = new AutoResetEvent(false);

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkInProgressCommandTreeViewModel"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public WorkInProgressCommandTreeViewModel(string message)
            : base(null, message, "workinprogress")
        {
            ////var thread = new Thread(() =>
            ////{
            ////	try
            ////	{
            ////		var startTime = DateTime.Now;
            ////		do
            ////		{
            ////			var timeSpan = DateTime.Now - startTime;
            ////			Name = string.Format("{0} ({1} sec)", _msg, timeSpan.Seconds);
            ////			OnPropertyChanged("Name");
            ////			if (_stopEvent.WaitOne(1000))
            ////			{
            ////				break;
            ////			}
            ////		}
            ////		while (true);
            ////	}
            ////	catch (Exception e)
            ////	{
            ////		// not a critical one if we don't update time.
            ////		Trace.WriteLine(string.Format("WorkInProgressCommandViewModel.UpdateThread exception and not a critical one.:{0}", e));
            ////	}
            ////}) { IsBackground = true };
            ////thread.Start();
        }

        /// <summary>
        /// The flag work done.
        /// </summary>
        public void MarkWorkDone()
        {
            this._stopEvent.Set();
        }
    }
}
