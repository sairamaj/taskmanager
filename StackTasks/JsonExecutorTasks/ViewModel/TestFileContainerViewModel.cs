using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Utils.Core;
using Utils.Core.Command;
using Utils.Core.ViewModels;

namespace JsonExecutorTasks.ViewModel
{
    public class TestFileContainerViewModel : CommandTreeViewModel
    {
        public string TestFileContainerPath { get; }

        public TestFileContainerViewModel(string testFileContainerPath):base(null, Path.GetFileNameWithoutExtension(testFileContainerPath), Path.GetFileNameWithoutExtension(testFileContainerPath))
        {
            TestFileContainerPath = testFileContainerPath ?? throw new ArgumentNullException(nameof(testFileContainerPath));
            IsExpanded = true;
        }

        public void SelectNone()
        {
            this.Children.OfType<TestFileViewModel>().ToList().ForEach(t => t.IsEnabled = false);
        }
        public void SelectAll()
        {
            this.Children.OfType<TestFileViewModel>().ToList().ForEach(t => t.IsEnabled = true);
        }

        public async Task RunAsync(bool verify)
        {
            foreach (var testFileViewModel in this.Children.OfType<TestFileViewModel>()
                .Where(t => t.IsEnabled))
            {
                await testFileViewModel.RunAsync(verify);
            }
        }

        protected override void LoadChildren()
        {
            base.LoadChildren();
            Directory.GetFiles(TestFileContainerPath, "*.json")
                .Where(t => !(t.Contains("config.json") || t.Contains("variables.json")))
                .Select(t => new TestFileViewModel(t, true)).ToList()
                .ForEach(t => this.Children.Add(t));
        }
    }
}
