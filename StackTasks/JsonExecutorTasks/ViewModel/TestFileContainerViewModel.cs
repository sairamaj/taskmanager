using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
            OnPropertyChanged(() => this.IsChecked);
        }
        public void SelectAll()
        {
            this.Children.OfType<TestFileViewModel>().ToList().ForEach(t => t.IsEnabled = true);
            OnPropertyChanged(()=> this.IsChecked);
        }

        public bool IsChecked
        {
            get
            {
                return this.Children.OfType<TestFileViewModel>().All(c => c.IsEnabled);
            }
            set
            {
                this.Children.OfType<TestFileViewModel>().ToList().ForEach(t => t.IsEnabled = value);
            }
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
