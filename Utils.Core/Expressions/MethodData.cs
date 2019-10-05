using System.Collections.Generic;
using System.Linq;

namespace Utils.Core.Expressions
{
    public class MethodData
    {
        List<Argument> _args = new List<Argument>();
        public MethodData(string name)
        {
            this.Name = name;
        }

        public string Name { get; }

        public IEnumerable<Argument> Arguments => this._args;

        public void AddParameter(string name, string val, int pos)
        {
            this._args.Add(new Argument(name, val, pos));
        }

        public override string ToString()
        {
            var info = $"{this.Name}{System.Environment.NewLine}";
            this.Arguments.ToList().ForEach(a => { info += $"{a.Name} {a.Val} {a.Pos} {a.IsVariable} {System.Environment.NewLine}"; });
            return info;
        }
    }

}
