using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.ServiceModel;

namespace Utils.Core.ViewModels
{
    /// <summary>
    /// The exception tree view item view model.
    /// </summary>
    public class ExceptionTreeViewItemViewModel : TreeViewItemViewModel
    {
        /// <summary>
        /// The _exception.
        /// </summary>
        private readonly Exception _exception;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionTreeViewItemViewModel"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        public ExceptionTreeViewItemViewModel(TreeViewItemViewModel parent, Exception exception)
            : this(parent, exception, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionTreeViewItemViewModel"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <param name="additionalInfo">
        /// The additional info.
        /// </param>
        public ExceptionTreeViewItemViewModel(TreeViewItemViewModel parent, Exception exception, string additionalInfo)
            : base(parent, string.Format(CultureInfo.InvariantCulture, "{0} {1}", exception == null ? "" : exception.GetType().ToString(), exception), true)
        {
            this.AdditionalInfo = additionalInfo;
            this._exception = exception ?? throw new ArgumentNullException(nameof(exception));
            if (this._exception is TargetInvocationException)
            {
                // for reflection exception look in inner exception.
                if (this._exception.InnerException != null)
                {
                    this._exception = this._exception.InnerException;
                }
            }

            this.IsExpanded = true;
        }

        /// <summary>
        /// Gets or sets the additional info.
        /// </summary>
        /// <value>Additional information.</value>
        public string AdditionalInfo { get; set; }

        /// <summary>
        /// Gets the message only.
        /// </summary>
        /// <value>Message string only.</value>
        public string MessageOnly
        {
            get { return this._exception == null ? string.Empty : this._exception.Message; }
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>Message string.</value>
        public string Message
        {
            get
            {
                return GetMessage(this._exception) + "\r\n\r\n" + this._exception.ToString();
            }
        }

        /// <summary>
        /// The load children.
        /// </summary>
        protected override void LoadChildren()
        {
            this.Children.Add(new NameValueTreeViewModel(this, "Message", this.Message));
        }

        /// <summary>
        /// The get message.
        /// </summary>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetMessage(Exception exception)
        {
            if (exception == null)
            {
                return string.Empty;
            }

            var msg = exception.Message;
            if (exception.GetType() == typeof(FaultException))
            {
                var fe = exception as FaultException;
                msg += "\r\n\t";
                msg += string.Format(CultureInfo.InvariantCulture, "{0} {1} {2}", fe.Action, fe.Code, fe.Reason);
            }
            else if (exception.GetType().Name.Contains("FaultException"))
            {
                // Is generic.
                if (exception.GetType().IsGenericType)
                {
                    Type[] typeParameters = exception.GetType().GetGenericArguments();
                    if (typeParameters.Length > 0)
                    {
                        msg += string.Format(CultureInfo.InvariantCulture, "\r\n\tFault of Type:{0}", typeParameters[0].Name.Split('.').Last());
                    }
                }
            }

            if (exception.InnerException != null)
            {
                msg += "\r\n\t";
                msg += GetMessage(exception.InnerException);
            }

            return msg;
        }
    }

}
