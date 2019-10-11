using System.IO;

namespace JsonExecutorTasks.ViewModel
{
    public class TestFileViewModel
    {
        public TestFileViewModel(string testFile, bool isEnabled)
        {
            this.TestFile = testFile;
            this.IsEnabled = isEnabled;
            this.Name = Path.GetFileNameWithoutExtension(testFile);
        }

        public string TestFile { get; }
        public string Name { get; }
        public bool IsEnabled { get; set; }
    }
}
