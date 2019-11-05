using System.Linq;

namespace Utils.Core.Expressions
{
    /// <summary>
    /// Represents a variable.
    /// </summary>
    public class Variable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Variable"/> class.
        /// </summary>
        /// <param name="name">
        /// Name of the variable.
        /// </param>
        public Variable(string name)
        {
            this.Name = name.Split('.').Last();
        }

        /// <summary>
        /// Gets name of the variable.
        /// </summary>
        public string Name { get; }
    }
}
