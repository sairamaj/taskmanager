using System;
using System.Windows;
using System.Windows.Interactivity;
using ICSharpCode.AvalonEdit;

namespace Utils.Core.Views
{
    /// <summary>
    /// Source: https://stackoverflow.com/questions/18964176/two-way-binding-to-avalonedit-document-text-using-mvvm.
    /// </summary>
    public sealed class AvalonEditBehaviour : Behavior<TextEditor>
    {
        /// <summary>
        /// Text property.
        /// </summary>
        public static readonly DependencyProperty GiveMeTheTextProperty =
            DependencyProperty.Register(
                "GiveMeTheText",
                typeof(string),
                typeof(AvalonEditBehaviour),
                new FrameworkPropertyMetadata(
                    default(string),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    PropertyChangedCallback));

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string GiveMeTheText
        {
            get => (string)this.GetValue(GiveMeTheTextProperty);
            set
            {
                this.SetValue(GiveMeTheTextProperty, value);
            }
        }

        /// <summary>
        /// On attached override.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            if (this.AssociatedObject != null)
            {
                this.AssociatedObject.TextChanged += this.AssociatedObjectOnTextChanged;
            }
        }

        /// <summary>
        /// On detach override.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (this.AssociatedObject != null)
            {
                this.AssociatedObject.TextChanged -= this.AssociatedObjectOnTextChanged;
            }
        }

        /// <summary>
        /// Property changed call back.
        /// </summary>
        /// <param name="dependencyObject">
        /// Dependency object.
        /// </param>
        /// <param name="dependencyPropertyChangedEventArgs">
        /// Property changed event arguments.
        /// </param>
        private static void PropertyChangedCallback(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var behavior = dependencyObject as AvalonEditBehaviour;
            if (behavior != null && behavior.AssociatedObject != null)
            {
                var editor = behavior.AssociatedObject as TextEditor;
                if (editor.Document != null)
                {
                    var caretOffset = editor.CaretOffset;
                    editor.Document.Text = dependencyPropertyChangedEventArgs.NewValue.ToString();
                    editor.CaretOffset = caretOffset;
                }
            }
        }

        /// <summary>
        /// Raises text changed method.
        /// </summary>
        /// <param name="sender">
        /// Sender of the event.
        /// </param>
        /// <param name="eventArgs">
        /// A <see cref="EventArgs"/> args.
        /// </param>
        private void AssociatedObjectOnTextChanged(object sender, EventArgs eventArgs)
        {
            var textEditor = sender as TextEditor;
            if (textEditor?.Document != null)
            {
                this.GiveMeTheText = textEditor.Document.Text;
            }
        }
    }
}
