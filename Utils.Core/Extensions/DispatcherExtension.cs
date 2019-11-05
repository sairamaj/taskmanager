using System;
using System.Windows.Threading;

namespace Utils.Core.Extensions
{
    /// <summary>
    /// The dispatcher extension.
    /// </summary>
    public static class DispatcherExtension
    {
        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="dispatcher">
        /// The dispatcher.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="dispatcher"/> or <paramref name="action"/> is <see language="null"/>.
        /// </exception>
        public static void Execute(this Dispatcher dispatcher, Action action)
        {
            if (dispatcher == null)
            {
                throw new ArgumentNullException(nameof(dispatcher));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            dispatcher.BeginInvoke(
                new DispatcherOperationCallback(
                    (a) =>
                    {
                        action();
                        return null;
                    }),
                new object[] { "a" });
        }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="dispatcher">
        /// The dispatcher.
        /// </param>
        /// <param name="func">
        /// The function.
        /// </param>
        /// <typeparam name="T">Function input type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>Return object.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="dispatcher"/> or <paramref name="func"/> is <see language="null"/>.
        /// </exception>
        public static T Execute<T>(this Dispatcher dispatcher, Func<T> func)
        {
            if (dispatcher == null)
            {
                throw new ArgumentNullException(nameof(dispatcher));
            }

            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return (T)dispatcher.Invoke(new DispatcherOperationCallback((a) => func()), new object[] { "a" });
        }
    }
}
