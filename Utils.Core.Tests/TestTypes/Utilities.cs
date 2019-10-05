using System;
using System.Collections.Generic;

namespace Utils.Core.Tests.TestTypes
{
    public static class Utilities
    {
        public static int Random()
        {
            return new Random().Next(1000);
        }

    }
}
