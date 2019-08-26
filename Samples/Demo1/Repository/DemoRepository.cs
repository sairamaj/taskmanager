using System;

namespace Demo1Task.Repository
{
    class DemoRepository : IDemoRepository
    {
        public string GetDemoName()
        {
            return $"demo_{new System.Random().Next(Int32.MaxValue)}";
        }
    }
}
