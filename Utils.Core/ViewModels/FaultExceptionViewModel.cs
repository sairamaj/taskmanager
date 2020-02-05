using System;
using System.Linq;
using System.ServiceModel;

namespace Utils.Core.ViewModels
{
    /// <summary>
    /// Represents WCF fault exception.
    /// </summary>
    public class FaultExceptionViewModel : TreeViewItemViewModel
    {
        /// <summary>
        /// Fault exception.
        /// </summary>
        private readonly FaultException _faultException;

        /// <summary>
        /// Initializes a new instance of the <see cref="FaultExceptionViewModel"/> class.
        /// </summary>
        /// <param name="faultException">
        /// A <see cref="FaultException"/> instance.
        /// </param>
        public FaultExceptionViewModel(FaultException faultException)
            : base(null, faultException.GetType().FullName, true)
        {
            this._faultException = faultException;
            this.IsExpanded = true;
        }

        /// <summary>
        /// Gets action part of the <see cref="FaultException"/>.
        /// </summary>
        public String Action
        {
            get
            {
                if (this._faultException == null)
                {
                    return string.Empty;
                }

                return this._faultException.Action;
            }
        }

        /// <summary>
        /// Gets code part of the <see cref="FaultException"/>.
        /// </summary>
        public String Code
        {
            get
            {
                if (this._faultException == null)
                {
                    return string.Empty;
                }

                return $"{this._faultException.Code.Name} {this._faultException.Code.SubCode}";
            }
        }

        /// <summary>
        /// Gets reason part of <see cref="FaultException"/>.
        /// </summary>
        public String Reason
        {
            get
            {
                if (this._faultException == null)
                {
                    return string.Empty;
                }

                return this._faultException.Reason.ToString();
            }
        }

        /// <summary>
        /// Gets detail part of <see cref="FaultException"/>.
        /// </summary>
        public string Detail
        {
            get
            {
                object detail = this.GetDetail();
                if (detail == null)
                {
                    return null;
                }

                if (detail.GetType().IsClass && detail.GetType() != typeof(string))
                {
                    return null;    // this one is meant for only value types (except string).
                }

                return detail.ToString();
            }
        }

        /// <summary>
        /// Loads parts of fault exception.
        /// </summary>
        protected override void LoadChildren()
        {
            this.Children.Add(new NameValueTreeViewModel(this, "Action", this.Action));
            this.Children.Add(new NameValueTreeViewModel(this, "Reason", this.Reason));
            this.Children.Add(new NameValueTreeViewModel(this, "Detail", this.Detail));

            var detailObj = this.GetDetail();
            if (detailObj != null)
            {
                this.Children.Add(new ObjectTreeViewModel(this, "Detail(s)", this.GetDetail(), detailObj.GetType().GetProperties().Any() ? Utils.Core.Model.InfoType.Properties : Utils.Core.Model.InfoType.Fields));
            }
        }

        /// <summary>
        /// Gets detail object.
        /// </summary>
        /// <returns>
        /// Detail property value of <see cref="FaultException"/>.
        /// </returns>
        private object GetDetail()
        {
            if (this._faultException == null)
            {
                return null;
            }

            var detailProperty = this._faultException.GetType().GetProperty("Detail");
            if (detailProperty == null)
            {
                return null;
            }

            return detailProperty.GetValue(this._faultException, null);
        }
    }
}
