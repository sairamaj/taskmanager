using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Utils.Core;
using Utils.Core.Command;
using Utils.Core.Diagnostics;
using Utils.Core.ViewModels;

namespace JsonExecutorTasks.ViewModel
{
    /// <summary>
    /// Test Task view.
    /// </summary>
    public class TaskTestViewModel : CoreViewModel
    {
        /// <summary>
        /// Selected test view model in tree.
        /// </summary>
        private TestFileViewModel _seeFileViewModel;

        /// <summary>
        /// Flag for test running status.
        /// </summary>
        private bool _isRunning;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskTestViewModel"/> class.
        /// </summary>
        /// <param name="logger"></param>
        public TaskTestViewModel(ILogger logger)
        {
            var testFilesPath = Path.Combine(Path.GetDirectoryName(new Uri(this.GetType().Assembly.CodeBase).AbsolutePath), "TestFiles");
            this.TestFiles = new SafeObservableCollection<TestFileViewModel>();
            if (Directory.Exists(testFilesPath))
            {
                this.Load(testFilesPath);
            }

            this.SelectAllCommand = new DelegateCommand(() =>
            {
                this.TestFileContainers.ToList().ForEach(container => container.SelectAll());
            });

            this.SelectNoneCommand = new DelegateCommand(() =>
            {
                this.TestFileContainers.ToList().ForEach(container => container.SelectNone());
            });

            this.RunCommand = new DelegateCommand(async () =>
            {
                await this.Run(false);
            });
            this.RunVerifyCommand = new DelegateCommand(async () => await this.Run(true));
        }

        /// <summary>
        /// Gets or sets Select all commands handler.
        /// </summary>
        public ICommand SelectAllCommand { get; set; }

        /// <summary>
        /// Gets or sets Select none command.
        /// </summary>
        public ICommand SelectNoneCommand { get; set; }

        /// <summary>
        /// Gets or sets run command handler.
        /// </summary>
        public ICommand RunCommand { get; set; }

        /// <summary>
        /// Gets or run verify command handler.
        /// </summary>
        public ICommand RunVerifyCommand { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a test is running or not.
        /// </summary>
        public bool IsRunning
        {
            get => this._isRunning;
            set
            {
                this._isRunning = value;
                this.OnPropertyChanged(() => this.IsRunning);
            }
        }

        /// <summary>
        /// Gets or sets test files.
        /// </summary>
        public ObservableCollection<TestFileViewModel> TestFiles { get; set; }

        /// <summary>
        /// Gets or sets test file containers.
        /// </summary>
        public ObservableCollection<TestFileContainerViewModel> TestFileContainers { get; set; }

        /// <summary>
        /// Gets or sets selected test file view model.
        /// </summary>
        public TestFileViewModel SelectedTestFile
        {
            get => this._seeFileViewModel;
            set
            {
                this._seeFileViewModel = value;
                this.SelectedTestFileDataViewModel = this.TestFileDataViewModels[value.Name];
                this.OnPropertyChanged(() => this.SelectedTestFileDataViewModel);
            }
        }

        /// <summary>
        /// Gets or sets selected test file data view model.
        /// </summary>
        public TestFileDataViewModel SelectedTestFileDataViewModel { get; set; }

        /// <summary>
        /// Gets or sets test file data view models.
        /// </summary>
        public IDictionary<string, TestFileDataViewModel> TestFileDataViewModels { get; set; }

        /// <summary>
        /// Run all test cases.
        /// </summary>
        /// <param name="verify">
        /// true to also verify with expected values.
        /// </param>
        /// <returns>
        /// A run task.
        /// </returns>
        private async Task Run(bool verify)
        {
            try
            {
                this.IsRunning = true;
                foreach (var viewModel in this.TestFileContainers)
                {
                    await viewModel.RunAsync(verify);
                }
            }
            finally
            {
                this.IsRunning = false;
            }
        }

        /// <summary>
        /// Load test case tree items.
        /// </summary>
        /// <param name="rootPath">
        /// Path containing the test files.
        /// </param>
        private void Load(string rootPath)
        {
            this.TestFileContainers = new SafeObservableCollection<TestFileContainerViewModel>();
            Directory.GetDirectories(rootPath).ToList().ForEach(d => this.TestFileContainers.Add(new TestFileContainerViewModel(d)));
        }
    }
}