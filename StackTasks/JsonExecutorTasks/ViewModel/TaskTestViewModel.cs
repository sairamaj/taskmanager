using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Utils.Core;
using Utils.Core.ViewModels;

namespace JsonExecutorTasks.ViewModel
{
    public class TaskTestViewModel : CoreViewModel
    {
        private TestFileViewModel _selectedTestViewModel;
        public TaskTestViewModel()
        {
            var testFilesPath = Path.Combine(Path.GetDirectoryName(new Uri(this.GetType().Assembly.CodeBase).AbsolutePath),"TestFiles");
            this.TestFiles = new SafeObservableCollection<TestFileViewModel>();
            if (Directory.Exists(testFilesPath))
            {
                Directory.GetFiles(testFilesPath, "*.json")
                    .Where(t=> !(t.Contains("config.json") || t.Contains("variables.json")))
                    .Select(t => new TestFileViewModel(t, true)).ToList().ForEach(t=> this.TestFiles.Add(t));
                this._selectedTestViewModel = this.TestFiles.LastOrDefault();
                this.TestFileDataViewModel = new TestFileDataViewModel(this.SelectedTestFile?.Name, this.SelectedTestFile?.TestFile);
            }
        }

        public ObservableCollection<TestFileViewModel> TestFiles { get; set; }

        public TestFileViewModel SelectedTestFile
        {
            get => this._selectedTestViewModel;
            set
            {
                this._selectedTestViewModel = value;
                this.TestFileDataViewModel = new TestFileDataViewModel(this.SelectedTestFile?.Name, this.SelectedTestFile?.TestFile);
                OnPropertyChanged(()=> this.TestFileDataViewModel);
            }
        }

        public TestFileDataViewModel TestFileDataViewModel { get; set; }
    }
}
