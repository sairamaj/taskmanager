using System.IO;
using System.Reflection;
using System.Windows.Controls;
using System.Xml;

namespace JsonExecutorTasks.Views
{
    /// <summary>
    /// Interaction logic for TestView.xaml
    /// </summary>
    public partial class TestView : UserControl
    {
        public TestView()
        {
            InitializeComponent();

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "JsonExecutorTasks.AvalonJsonSyntax.xml";

            using (var xshd_stream = assembly.GetManifestResourceStream(resourceName))
            {
                var xshd_reader = new XmlTextReader(xshd_stream);
                TextEditor.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(xshd_reader, ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance);
            }
        }
    }
}
