using System.Linq;

namespace Utils.Core.Expressions
{
    public class Variable
    {
        public Variable(string name)
        {
            Name = name.Split('.').Last();
        }
        public string Name { get; }

        public bool IsVariable { get; set; }
    }
}
