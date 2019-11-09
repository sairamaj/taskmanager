using System.IO;
using System.Reflection;
using System.Windows.Controls;
using System.Xml;

namespace JsonExecutorTasks.Views
{
    /// <summary>
    /// Interaction logic for TestView.xaml.
    /// </summary>
    public partial class TestView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestView"/> class.
        /// </summary>
        public TestView()
        {
            this.InitializeComponent();

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "JsonExecutorTasks.AvalonJsonSyntax.xml";

            using (var xshd_stream = assembly.GetManifestResourceStream(resourceName))
            {
                var xshd_reader = new XmlTextReader(xshd_stream);
                this.TextEditor.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(xshd_reader, ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance);
            }
        }
    }
}
