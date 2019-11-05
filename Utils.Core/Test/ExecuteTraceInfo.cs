using System;
using System.Collections.Generic;

namespace Utils.Core.Test
{
    /// <summary>
    /// Execution trace information.
    /// </summary>
    public class ExecuteTraceInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteTraceInfo"/> class.
        /// </summary>
        /// <param name="traceType">
        /// A <see cref="TraceType"/> type.
        /// </param>
        public ExecuteTraceInfo(TraceType traceType)
        {
            this.TraceType = traceType;
        }

        /// <summary>
        /// Gets trace type.
        /// </summary>
        public TraceType TraceType { get; }

        /// <summary>
        /// Gets or sets test information.
        /// </summary>
        public TestInfo TestInfo { get; set; }

        /// <summary>
        /// Gets or sets method executed.
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// Gets or sets method parameters.
        /// </summary>
        public IDictionary<string, object> MethodParameters { get; set; }

        /// <summary>
        /// Gets or sets method return value.
        /// </summary>
        public object MethodReturnValue { get; set; }

        /// <summary>
        /// Gets or sets method exception.
        /// </summary>
        public Exception MethodException { get; set; }

        /// <summary>
        /// Gets or sets method return type.
        /// </summary>
        public Type MethodReturnType { get; set; }

        /// <summary>
        /// Gets or sets expected value.
        /// </summary>
        public object Expected { get; set; }

        /// <summary>
        /// Gets or sets actual value.
        /// </summary>
        public object Actual { get; set; }
    }
}
