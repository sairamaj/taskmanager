using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Threading;

namespace Utils.Core
{
    /// <summary>
    /// The safe observable collection.
    /// </summary>
    /// <typeparam name="T">Collection type name.
    /// </typeparam>
    public class SafeObservableCollection<T> : ObservableCollection<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SafeObservableCollection{T}"/> class.
        /// </summary>
        public SafeObservableCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeObservableCollection{T}"/> class.
        /// </summary>
        /// <param name="list">
        /// The list.
        /// </param>
        public SafeObservableCollection(List<T> list)
            : base(list)
        {
        }

        /// <summary>
        /// The collection changed.
        /// </summary>
        public override event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// The on collection changed.
        /// </summary>
        /// <param name="e">
        /// The arguments.
        /// </param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            // Be nice - use BlockReentrancy like MSDN said
            using (this.BlockReentrancy())
            {
                NotifyCollectionChangedEventHandler eventHandler = this.CollectionChanged;
                if (eventHandler == null)
                {
                    return;
                }

                Delegate[] delegates = eventHandler.GetInvocationList();

                // Walk thru invocation list
                foreach (NotifyCollectionChangedEventHandler handler in delegates)
                {
                    var dispatcherObject = handler.Target as DispatcherObject;

                    // If the subscriber is a DispatcherObject and different thread
                    if (dispatcherObject != null && dispatcherObject.CheckAccess() == false)
                    {

                        // Invoke handler in the target dispatcher's thread
                        dispatcherObject.Dispatcher.Invoke(DispatcherPriority.DataBind, handler, this, e);
                    }
                    else
                    {
                        // Execute handler as is
                        handler(this, e);
                    }
                }
            }
        }
    }

}
