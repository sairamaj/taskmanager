using System;

namespace Utils.Core.Expressions
{
    public class Argument
    {
        public Argument(string name, string val, int pos)
        {
            Name = name;
            if (!string.IsNullOrEmpty(val))
            {
                if (val.StartsWith("var.", StringComparison.InvariantCultureIgnoreCase))
                {
                    IsVariable = true;
                    Val = val.Substring("var.".Length);
                }
                else
                {
                    Val = val;
                }
            }
            else
            {
                Val = val;
            }
            Pos = pos;
        }

        public string Name { get; }
        public string Val { get; }
        public int Pos { get; }
        public bool IsVariable { get; set; }
    }
}
