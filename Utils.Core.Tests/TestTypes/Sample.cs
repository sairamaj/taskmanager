using System.Collections.Generic;

namespace Utils.Core.Tests.TestTypes
{
    public static class Sample
    {
        public static IDictionary<string,object> GetInfo()
        {
            return new Dictionary<string, object>
            {
                {"result", 0 },
                {"val1", "val1" },
                {"val2", true},
                {"val3", 1.0 }
            };
        }

    }
}
