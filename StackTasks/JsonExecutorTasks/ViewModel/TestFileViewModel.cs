using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using JsonExecutorTasks.Model;
using Utils.Core.Command;
using Utils.Core.ViewModels;

namespace JsonExecutorTasks.ViewModel
{
    /// <summary>
    /// Test file view model.
    /// </summary>
    public class TestFileViewModel : CommandTreeViewModel
    {
        /// <summary>
        /// Test status.
        /// </summary>
        private TestStatus _testStatus;

        /// <summary>
        /// Flag to check whether test is enabled or not.
        /// </summary>
        private bool _isEnabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestFileViewModel"/> class.
        /// </summary>
        /// <param name="testFile">
        /// Test file name.
        /// </param>
        /// <param name="isEnabled">
        /// Flag for enabling or disabling the test.
        /// </param>
        public TestFileViewModel(string testFile, bool isEnabled)
            : base(null, Path.GetFileNameWithoutExtension(testFile), testFile)
        {
            this.TestFile = testFile;
            this.IsEnabled = isEnabled;
            this.RunCommand = new DelegateCommand(async () => { await this.RunAsync(false); });
            this.RunVerifyCommand = new DelegateCommand(async () =>
            {
                await this.RunAsync(true);
            });

            this.DataContext = this.TestDataViewModel = new TestFileDataViewModel(Path.GetFileNameWithoutExtension(testFile), testFile);
        }

        /// <summary>
        /// Gets test file name.
        /// </summary>
        public string TestFile { get; }

        /// <summary>
        /// Gets or sets a value indicating whether test is enabled or not.
        /// </summary>
        public bool IsEnabled
        {
            get => this._isEnabled;
            set
            {
                this._isEnabled = value;
                this.OnPropertyChanged(() => this.IsEnabled);
            }
        }

        /// <summary>
        /// Gets or sets test status.
        /// </summary>
        public TestStatus TestStatus
        {
            get => this._testStatus;
            set
            {
                this._testStatus = value;
                this.OnPropertyChanged(() => this.TestStatus);
            }
        }

        /// <summary>
        /// Gets or sets test data view model.
        /// </summary>
        public TestFileDataViewModel TestDataViewModel { get; set; }

        /// <summary>
        /// Gets or sets run command.
        /// </summary>
        public ICommand RunCommand { get; set; }

        /// <summary>
        /// Gets or sets run verify command.
        /// </summary>
        public ICommand RunVerifyCommand { get; set; }

        /// <summary>
        /// Rus the test.
        /// </summary>
        /// <param name="verify">
        /// true for test verification.
        /// </param>
        /// <returns>
        /// Task instance.
        /// </returns>
        public async Task RunAsync(bool verify)
        {
            try
            {
                this.TestStatus = TestStatus.Running;
                await this.TestDataViewModel.Execute(verify);
            }
            finally
            {
                this.TestStatus = this.TestDataViewModel.TestStatus;
            }
        }
    }
}