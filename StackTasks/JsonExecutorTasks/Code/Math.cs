using System;

namespace JsonExecutorTasks.Code
{
    /// <summary>
    /// Sample test class.
    /// </summary>
    public static class Math
    {
        /// <summary>
        /// Adds two numbers.
        /// </summary>
        /// <param name="num1">
        /// Number1.
        /// </param>
        /// <param name="num2">
        /// Number2.
        /// </param>
        /// <returns>
        /// Sum of the two numbers given.
        /// </returns>
        public static int Add(int num1, int num2)
        {
            Console.WriteLine($"Math.Add {num1} {num2}");
            return num1 + num2;
        }

        /// <summary>
        /// Subtracts number2 from number1.
        /// </summary>
        /// <param name="num1">
        /// Number1.
        /// </param>
        /// <param name="num2">
        /// Number2.
        /// </param>
        /// <returns>
        /// Subtracted value.
        /// </returns>
        public static int Sub(int num1, int num2)
        {
            Console.WriteLine($"Math.Sub {num1} {num2}");
            return num1 - num2;
        }
    }
}