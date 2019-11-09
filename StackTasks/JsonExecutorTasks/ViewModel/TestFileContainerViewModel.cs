using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utils.Core.ViewModels;

namespace JsonExecutorTasks.ViewModel
{
    /// <summary>
    /// Test file container view model.
    /// </summary>
    public class TestFileContainerViewModel : CommandTreeViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestFileContainerViewModel"/> class.
        /// </summary>
        /// <param name="testFileContainerPath">
        /// Test file container path containing the test files.
        /// </param>
        public TestFileContainerViewModel(string testFileContainerPath)
            : base(null, Path.GetFileNameWithoutExtension(testFileContainerPath), Path.GetFileNameWithoutExtension(testFileContainerPath))
        {
            this.TestFileContainerPath = testFileContainerPath ?? throw new ArgumentNullException(nameof(testFileContainerPath));
            this.IsExpanded = true;
        }

        /// <summary>
        /// Gets test file container path.
        /// </summary>
        public string TestFileContainerPath { get; }

        /// <summary>
        /// Gets or sets a value indicating whether test is checked or not.
        /// </summary>
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

        /// <summary>
        /// Select none handler.
        /// </summary>
        public void SelectNone()
        {
            this.Children.OfType<TestFileViewModel>().ToList().ForEach(t => t.IsEnabled = false);
            this.OnPropertyChanged(() => this.IsChecked);
        }

        /// <summary>
        /// Select all handler.
        /// </summary>
        public void SelectAll()
        {
            this.Children.OfType<TestFileViewModel>().ToList().ForEach(t => t.IsEnabled = true);
            this.OnPropertyChanged(() => this.IsChecked);
        }

        /// <summary>
        /// Runs test files.
        /// </summary>
        /// <param name="verify">
        /// true for run with verify option.
        /// </param>
        /// <returns>
        /// A run task.
        /// </returns>
        public async Task RunAsync(bool verify)
        {
            foreach (var testFileViewModel in this.Children.OfType<TestFileViewModel>()
                .Where(t => t.IsEnabled))
            {
                await testFileViewModel.RunAsync(verify);
            }
        }

        /// <summary>
        /// Loads test files.
        /// </summary>
        protected override void LoadChildren()
        {
            base.LoadChildren();
            Directory.GetFiles(this.TestFileContainerPath, "*.json")
                .Where(t => !(t.Contains("config.json") || t.Contains("variables.json")))
                .Select(t => new TestFileViewModel(t, true)).ToList()
                .ForEach(t => this.Children.Add(t));
        }
    }
}
