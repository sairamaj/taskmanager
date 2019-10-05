using System;
using System.Runtime.Serialization;

namespace Utils.Core.Expressions
{
    [Serializable]
    public class InvalidSyntaxException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public InvalidSyntaxException()
        {
        }

        public InvalidSyntaxException(string message) : base(message)
        {
        }

        public InvalidSyntaxException(string message, Exception inner) : base(message, inner)
        {
        }

        protected InvalidSyntaxException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
