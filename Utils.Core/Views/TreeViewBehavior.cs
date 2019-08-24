using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Utils.Core.Views
{
    /// <summary>
	/// The tree view behavior.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Behaviours", Justification = "Copied from different project.")]
    public static class TreeViewBehavior
    {
        /// <summary>
        /// The expanding behavior property.
        /// </summary>
        public static readonly DependencyProperty ExpandingBehaviourProperty =
            DependencyProperty.RegisterAttached(
                "ExpandingBehaviour",
                typeof(ICommand),
                typeof(TreeViewBehavior),
                new PropertyMetadata(OnExpandingBehaviourChanged));

        /// <summary>
        /// The set expanding behavior.
        /// </summary>
        /// <param name="o">
        /// The object.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Copied code."), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Behaviour", Justification = "Copied code.")]
        public static void SetExpandingBehaviour(DependencyObject o, ICommand value)
        {
            o.SetValue(ExpandingBehaviourProperty, value);
        }

        /// <summary>
        /// The get expanding behavior.
        /// </summary>
        /// <param name="o">
        /// The object.
        /// </param>
        /// <returns>
        /// The <see cref="ICommand"/>Command object.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Copied code from different project."), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Behaviour", Justification = "Code copied from some other project."), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "o", Justification = "Copied from some other project, need to work on these.")]
        public static ICommand GetExpandingBehaviour(DependencyObject o)
        {
            return (ICommand)o.GetValue(ExpandingBehaviourProperty);
        }

        /// <summary>
        /// The on expanding behavior changed.
        /// </summary>
        /// <param name="d">
        /// The dependency object.
        /// </param>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        private static void OnExpandingBehaviourChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tvi = d as TreeViewItem;
            if (tvi != null)
            {
                var ic = e.NewValue as ICommand;
                if (ic != null)
                {
                    tvi.Expanded += (s, a) =>
                    {
                        if (ic.CanExecute(a))
                        {
                            ic.Execute(a);
                        }

                        a.Handled = true;
                    };
                }
            }
        }
    }
}
