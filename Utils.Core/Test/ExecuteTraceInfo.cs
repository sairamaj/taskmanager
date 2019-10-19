using System;
using System.Collections.Generic;

namespace Utils.Core.Test
{
    public class ExecuteTraceInfo
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteTraceInfo"/> class.
        /// </summary>
        /// <param name="traceType"></param>
        public ExecuteTraceInfo(TraceType traceType)
        {
            TraceType = traceType;
        }

        public TraceType TraceType { get; }
        public string MethodName { get; set; }
        public IDictionary<string,object> MethodParameters { get; set; }
        public object MethodReturnValue { get; set; }
        public Exception MethodException { get; set; }
        public Type MethodReturnType { get; set; }
    }
}
