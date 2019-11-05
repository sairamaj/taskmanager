using System;
using System.Runtime.Serialization;

namespace Utils.Core.Expressions
{
    /// <summary>
    /// Invalid syntax exception.
    /// </summary>
    [Serializable]
    public class InvalidSyntaxException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidSyntaxException"/> class.
        /// </summary>
        public InvalidSyntaxException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidSyntaxException"/> class.
        /// </summary>
        /// <param name="message">
        /// Message of the exception.
        /// </param>
        public InvalidSyntaxException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidSyntaxException"/> class.
        /// </summary>
        /// <param name="message">
        /// Message of the exception.
        /// </param>
        /// <param name="inner">
        /// Inner exception.
        /// </param>
        public InvalidSyntaxException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidSyntaxException"/> class.
        /// </summary>
        /// <param name="info">
        /// A <see cref="SerializationInfo"/> instance.
        /// </param>
        /// <param name="context">
        /// A <see cref="StreamingContext"/> instance.
        /// </param>
        protected InvalidSyntaxException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}
