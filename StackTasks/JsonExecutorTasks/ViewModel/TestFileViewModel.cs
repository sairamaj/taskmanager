using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using JsonExecutorTasks.Model;
using Utils.Core.Command;
using Utils.Core.ViewModels;

namespace JsonExecutorTasks.ViewModel
{
    public class TestFileViewModel : CommandTreeViewModel
    {
        private TestStatus _testStatus;
        private bool _isEnabled;

        public TestFileViewModel(string testFile, bool isEnabled)
            : base(null, Path.GetFileNameWithoutExtension(testFile), testFile)
        {
            this.TestFile = testFile;
            this.IsEnabled = isEnabled;
            this.RunCommand = new DelegateCommand(async () => { await RunAsync(false); });
            this.RunVerifyCommand = new DelegateCommand(async () =>
            {
                await RunAsync(true);
            });

            this.DataContext = this.TestDataViewModel = new TestFileDataViewModel(Path.GetFileNameWithoutExtension(testFile), testFile);
        }

        public string TestFile { get; }

        public bool IsEnabled
        {
            get => this._isEnabled;
            set
            {
                this._isEnabled = value;
                OnPropertyChanged(() => this.IsEnabled);
            }
        }

        public TestStatus TestStatus
        {
            get => this._testStatus;
            set
            {
                this._testStatus = value;
                OnPropertyChanged(()=>this.TestStatus);
            }
        }

        public TestFileDataViewModel TestDataViewModel { get; set; }

        public ICommand RunCommand { get; set; }
        public ICommand RunVerifyCommand { get; set; }

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
