using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Utils.Core.ViewModels;

namespace Utils.Core.Views
{
    /// <summary>
    /// Core user control to add some more helper functions.
    /// </summary>
    public class CoreUserControl : UserControl
    {
        /// <summary>
        /// On mouse move.
        /// </summary>
        /// <param name="e">
        /// MouseEvent arguments.
        /// </param>
        protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var viewModel = this.DataContext as CoreViewModel;
                var data = viewModel?.GetDragData();
                if (data == null)
                {
                    return;
                }

                DragDrop.DoDragDrop(this, data, DragDropEffects.Copy);
            }
        }
    }
}