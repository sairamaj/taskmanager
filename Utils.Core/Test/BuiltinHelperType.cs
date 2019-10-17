namespace Utils.Core.Test
{
    public static class BuiltinHelperType
    {
        public static string ToLower(string input)
        {
            return input?.ToLowerInvariant();
        }
    }
}
