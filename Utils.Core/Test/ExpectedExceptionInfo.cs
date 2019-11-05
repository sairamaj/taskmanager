namespace Utils.Core.Test
{
    /// <summary>
    /// Expected exception information.
    /// </summary>
    internal class ExpectedExceptionInfo
    {
        /// <summary>
        /// Gets or sets a value indicating whether this is exception.
        /// </summary>
        public bool Exception { get; set; }

        /// <summary>
        /// Gets or sets expected exception type.
        /// </summary>
        public string ExceptionType { get; set; }

        /// <summary>
        /// Gets or sets expected exception message like string.
        /// </summary>
        public string ExceptionMessageLike { get; set; }
    }
}
