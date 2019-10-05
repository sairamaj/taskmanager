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

        public static IDictionary<string, object> GetInfoReturnArrayInDictionary()
        {
            return new Dictionary<string, object>
            {
                {"val1", new string[]{"item1_1","item1_2"} },
                {"val2", new string[]{"item2_1","item2_2"} },
            };

        }

    }
}
