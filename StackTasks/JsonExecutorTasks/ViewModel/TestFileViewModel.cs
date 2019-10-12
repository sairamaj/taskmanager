using System;
using System.IO;
using System.Windows.Input;
using JsonExecutorTasks.Model;
using Utils.Core.Command;
using Utils.Core.ViewModels;

namespace JsonExecutorTasks.ViewModel
{
    public class TestFileViewModel : CoreViewModel
    {
        private TestStatus _testStatus;
        private bool _isEnabled;

        public TestFileViewModel(string testFile, bool isEnabled, Action<TestFileViewModel, bool> onRun)
        {
            this.TestFile = testFile;
            this.IsEnabled = isEnabled;
            this.Name = Path.GetFileNameWithoutExtension(testFile);
            this.RunCommand = new DelegateCommand(() =>
            {
                onRun(this, false);
            });
            this.RunVerifyCommand = new DelegateCommand(() =>
            {
                onRun(this, true);
            });
            this.TestStatus = TestStatus.None;
        }

        public string TestFile { get; }
        public string Name { get; }

        public bool IsEnabled
        {
            get => this._isEnabled;
            set
            {
                this._isEnabled = value;
                OnPropertyChanged(()=> this.IsEnabled);
            }
        }

        public ICommand RunCommand { get; set; }
        public ICommand RunVerifyCommand { get; set; }

        public TestStatus TestStatus
        {
            get { return this._testStatus; }
            set
            {
                this._testStatus = value;
                OnPropertyChanged(()=> this.TestStatus);
            }
        }
    }
}
