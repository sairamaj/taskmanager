using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using JsonExecutorTasks.Model;
using Utils.Core;
using Utils.Core.Command;
using Utils.Core.Diagnostics;
using Utils.Core.ViewModels;

namespace JsonExecutorTasks.ViewModel
{
    public class TaskTestViewModel : CoreViewModel
    {
        private TestFileViewModel _seeFileViewModel;
        private bool _isRunning;
        public TaskTestViewModel(ILogger logger)
        {
            var testFilesPath = Path.Combine(Path.GetDirectoryName(new Uri(this.GetType().Assembly.CodeBase).AbsolutePath),"TestFiles");
            this.TestFiles = new SafeObservableCollection<TestFileViewModel>();
            if (Directory.Exists(testFilesPath))
            {
                Load(testFilesPath);
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
                await Run(false);
            });
            this.RunVerifyCommand = new DelegateCommand(async () => await Run(true));
        }

        private void Load(string rootPath)
        {
            this.TestFileContainers = new SafeObservableCollection<TestFileContainerViewModel>();
            Directory.GetDirectories(rootPath).ToList().ForEach(d=> this.TestFileContainers.Add(new TestFileContainerViewModel(d)));
                 
            return;
            //Directory.GetFiles(rootPath, "*.json")
            //    .Where(t => !(t.Contains("config.json") || t.Contains("variables.json")))
            //    .Select(t => new TestFileViewModel(t, true, async (testViewModel, verify) =>
            //    {
            //        await Execute(this.TestFileDataViewModels[testViewModel.Name], testViewModel, verify);
            //    })).ToList()
            //    .ForEach(t => this.TestFiles.Add(t));
            //this.TestFileDataViewModels = new Dictionary<string, TestFileDataViewModel>();
            //this.TestFiles.ToList().ForEach(t => this.TestFileDataViewModels.Add(t.Name, new TestFileDataViewModel(t.Name, t.TestFile)));
            //this.SelectedTestFile = this.TestFiles.FirstOrDefault();
        }

        public ICommand SelectAllCommand { get; set; }
        public ICommand SelectNoneCommand { get; set; }
        public ICommand RunCommand { get; set; }
        public ICommand RunVerifyCommand { get; set; }

        public bool IsRunning
        {
            get => this._isRunning;
            set
            {
                this._isRunning = value;
                OnPropertyChanged(()=> this.IsRunning);
            }
        }

        public ObservableCollection<TestFileViewModel> TestFiles { get; set; }
        public ObservableCollection<TestFileContainerViewModel> TestFileContainers { get; set; }
        

        public TestFileViewModel SelectedTestFile
        {
            get { return this._seeFileViewModel; }
            set
            {
                this._seeFileViewModel = value;
                this.SelectedTestFileDataViewModel = this.TestFileDataViewModels[value.Name];
                OnPropertyChanged(()=> this.SelectedTestFileDataViewModel);
            }
        }

        public TestFileDataViewModel SelectedTestFileDataViewModel { get; set; }

        public IDictionary<string,TestFileDataViewModel> TestFileDataViewModels { get; set; }
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
    }
}
