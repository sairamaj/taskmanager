using System.Windows.Controls;

namespace Utils.Core.Views
{
    /// <summary>
    /// Interaction logic for DetailViewContainer.xaml
    /// </summary>
    public partial class DetailViewContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DetailViewContainer"/> class.
        /// </summary>
        public DetailViewContainer()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Shows the given view.
        /// </summary>
        /// <param name="view">View to be show. <see cref="UserControl"/></param>
        public void ShowView(UserControl view)
        {
            this.detailPanel.Children.Clear();
            if (view != null)
            {
                this.detailPanel.Children.Add(view);
                this.detailPanel.UpdateLayout();
            }
        }
    }
}
