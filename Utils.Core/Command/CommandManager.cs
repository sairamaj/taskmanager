using System;
using System.Collections.Generic;

namespace Utils.Core.Command
{
    /// <summary>
    /// The command manager helper.
    /// </summary>
    internal static class CommandManager
    {
        /// <summary>
        /// The call weak reference handlers.
        /// </summary>
        /// <param name="handlers">
        /// The handlers.
        /// </param>
        internal static void CallWeakReferenceHandlers(List<WeakReference> handlers)
        {
            if (handlers != null)
            {
                //// Take a snapshot of the handlers before we call out to them since the handlers
                //// could cause the array to me modified while we are reading it.

                EventHandler[] callees = new EventHandler[handlers.Count];
                int count = 0;

                for (int i = handlers.Count - 1; i >= 0; i--)
                {
                    WeakReference reference = handlers[i];
                    EventHandler handler = reference.Target as EventHandler;
                    if (handler == null)
                    {
                        // Clean up old handlers that have been collected
                        handlers.RemoveAt(i);
                    }
                    else
                    {
                        callees[count] = handler;
                        count++;
                    }
                }

                // Call the handlers that we snapshotted
                for (int i = 0; i < count; i++)
                {
                    EventHandler handler = callees[i];
                    handler(null, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// The add handlers to re query suggested.
        /// </summary>
        /// <param name="handlers">
        /// The handlers.
        /// </param>
        internal static void AddHandlersToRequerySuggested(List<WeakReference> handlers)
        {
            if (handlers != null)
            {
                foreach (WeakReference handlerRef in handlers)
                {
                    EventHandler handler = handlerRef.Target as EventHandler;
                    if (handler != null)
                    {
                        System.Windows.Input.CommandManager.RequerySuggested += handler;
                    }
                }
            }
        }

        /// <summary>
        /// The remove handlers from re query suggested.
        /// </summary>
        /// <param name="handlers">
        /// The handlers.
        /// </param>
        internal static void RemoveHandlersFromRequerySuggested(List<WeakReference> handlers)
        {
            if (handlers != null)
            {
                foreach (WeakReference handlerRef in handlers)
                {
                    EventHandler handler = handlerRef.Target as EventHandler;
                    if (handler != null)
                    {
                        System.Windows.Input.CommandManager.RequerySuggested -= handler;
                    }
                }
            }
        }

        /// <summary>
        /// The add weak reference handler.
        /// </summary>
        /// <param name="handlers">
        /// The handlers.
        /// </param>
        /// <param name="handler">
        /// The handler.
        /// </param>
        /// <param name="defaultListSize">
        /// The default list size.
        /// </param>
        internal static void AddWeakReferenceHandler(ref List<WeakReference> handlers, EventHandler handler, int defaultListSize)
        {
            if (handlers == null)
            {
                handlers = defaultListSize > 0 ? new List<WeakReference>(defaultListSize) : new List<WeakReference>();
            }

            handlers.Add(new WeakReference(handler));
        }

        /// <summary>
        /// The remove weak reference handler.
        /// </summary>
        /// <param name="handlers">
        /// The handlers.
        /// </param>
        /// <param name="handler">
        /// The handler.
        /// </param>
        internal static void RemoveWeakReferenceHandler(List<WeakReference> handlers, EventHandler handler)
        {
            if (handlers != null)
            {
                for (int i = handlers.Count - 1; i >= 0; i--)
                {
                    WeakReference reference = handlers[i];
                    EventHandler existingHandler = reference.Target as EventHandler;
                    if ((existingHandler == null) || (existingHandler == handler))
                    {
                        // Clean up old handlers that have been collected
                        // in addition to the handler that is to be removed.
                        handlers.RemoveAt(i);
                    }
                }
            }
        }
    }
}