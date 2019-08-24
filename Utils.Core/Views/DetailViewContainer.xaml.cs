using System.Windows.Controls;

namespace Utils.Core.Views
{
    /// <summary>
    /// Interaction logic for DetailViewContainer.xaml
    /// </summary>
    public partial class DetailViewContainer
    {
        public DetailViewContainer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Shows the given view.
        /// </summary>
        /// <param name="view">View to be show. <see cref="UserControl"/></param>
        public void ShowView(UserControl view)
        {
            detailPanel.Children.Clear();
            if (view != null)
            {
                detailPanel.Children.Add(view);
                detailPanel.UpdateLayout();
            }
        }
    }
}
