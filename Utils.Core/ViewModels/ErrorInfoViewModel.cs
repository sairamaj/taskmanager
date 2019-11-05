using System;
using System.Reflection;
using Utils.Core.Extensions;

namespace Utils.Core.ViewModels
{
    /// <summary>
    /// The error info view model.
    /// </summary>
    public class ErrorInfoViewModel : TreeViewItemViewModel
    {
        /// <summary>
        /// The exception.
        /// </summary>
        private readonly Exception _exception;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorInfoViewModel"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="exception">
        /// The e.
        /// </param>
        public ErrorInfoViewModel(string name, Exception exception)
            : base(null, name, true)
        {
            this._exception = exception;
            this.IsExpanded = true;
        }

        /// <summary>
        /// Gets or sets the message with stack trace.
        /// </summary>
        /// <value>Message containing stack trace.</value>
        public string MessageWithStackTrace
        {
            get
            {
                var name = this._exception.Message;
                name += "\r\n" + this._exception.StackTrace;
                var re = this._exception as ReflectionTypeLoadException;
                if (re != null)
                {
                    name += "\r\n";
                    foreach (var le in re.LoaderExceptions)
                    {
                        name += le.Message;
                        name += "\r\n";
                        name += "\r\n" + le.StackTrace;
                    }
                }

                return name;
            }

            set
            {
                this.Name = value;
            }
        }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>Message string.</value>
        public string Message
        {
            get
            {
                return this._exception.GetExceptionMessage();
            }

            set
            {
                this.Name = value;
            }
        }
    }
}
