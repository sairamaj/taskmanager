using System.Collections.Generic;
using System.Linq;

namespace Utils.Core.Expressions
{
    /// <summary>
    /// Represents method data.
    /// </summary>
    public class MethodData
    {
        /// <summary>
        /// method arguments.
        /// </summary>
        private List<Argument> _args = new List<Argument>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodData"/> class.
        /// </summary>
        /// <param name="name"></param>
        public MethodData(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets method name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets arguments.
        /// </summary>
        public IEnumerable<Argument> Arguments => this._args;

        /// <summary>
        /// Adds a method parameter.
        /// </summary>
        /// <param name="name">
        /// Name of the parameter.
        /// </param>
        /// <param name="val">
        /// Value of the parameter.
        /// </param>
        /// <param name="pos">
        /// Position of the parameter.
        /// </param>
        public void AddParameter(string name, string val, int pos)
        {
            this._args.Add(new Argument(name, val, pos));
        }

        /// <summary>
        /// Gets string representation.
        /// </summary>
        /// <returns>
        /// Returns the arguments information in string form.
        /// </returns>
        public override string ToString()
        {
            var info = $"{this.Name}{System.Environment.NewLine}";
            this.Arguments.ToList().ForEach(a => { info += $"{a.Name} {a.Val} {a.Pos} {a.IsVariable} {System.Environment.NewLine}"; });
            return info;
        }
    }
}
