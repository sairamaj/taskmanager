using System;

namespace Utils.Core.Expressions
{
    /// <summary>
    /// Represents argument expression.
    /// </summary>
    public class Argument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Argument"/> class.
        /// </summary>
        /// <param name="name">
        /// Name of the argument.
        /// </param>
        /// <param name="val">
        /// Value of the argument.
        /// </param>
        /// <param name="pos">
        /// Postion of the argument.
        /// </param>
        public Argument(string name, string val, int pos)
        {
            this.Name = name;
            if (!string.IsNullOrEmpty(val))
            {
                if (val.StartsWith("var.", StringComparison.InvariantCultureIgnoreCase))
                {
                    this.IsVariable = true;
                    this.Val = val.Substring("var.".Length);
                }
                else
                {
                    this.Val = val;
                }
            }
            else
            {
                this.Val = val;
            }

            this.Pos = pos;
        }

        /// <summary>
        /// Gets name of the argument.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets value of the argument.
        /// </summary>
        public string Val { get; }

        /// <summary>
        /// Gets position of the argument.
        /// </summary>
        public int Pos { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this argument is variable or not.
        /// </summary>
        public bool IsVariable { get; set; }
    }
}
