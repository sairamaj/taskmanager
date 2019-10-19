using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using FluentAssertions.Execution;
using JsonExecutorTasks.Model;
using JsonExecutorTasks.Views;
using Newtonsoft.Json;
using Utils.Core;
using Utils.Core.Command;
using Utils.Core.Diagnostics;
using Utils.Core.Test;
using Utils.Core.ViewModels;

namespace JsonExecutorTasks.ViewModel
{
    public class TestFileDataViewModel : CoreViewModel
    {
        private readonly string _fileName;

        public TestFileDataViewModel(string name, string fileName)
        {
            this.Name = name;
            this._fileName = fileName;
            if (File.Exists(fileName))
            {
                this.Data = File.ReadAllText(fileName);
            }
            this.TraceMessages = new SafeObservableCollection<TreeViewItemViewModel>();
            this.RunTestFileCommand = new DelegateCommand(async () => { await Execute(false); });
            this.RunTestFileWithVerifyCommand = new DelegateCommand(async () => { await Execute(true); });
            this.TestStatus = TestStatus.None;
        }

        public string Name { get; }
        public string Data { get; set; }
        public string ResultsData { get; set; }
        public TestStatus TestStatus{ get; set; }
        public ICommand RunTestFileCommand { get; }
        public ICommand RunTestFileWithVerifyCommand { get; }
        
        public ObservableCollection<TreeViewItemViewModel> TraceMessages { get;  }

        public string ConfigJson
        {
            get
            {
                var configJsonFile = Path.Combine(Path.GetDirectoryName(this._fileName), "config.json");
                if (File.Exists(configJsonFile))
                {
                    return File.ReadAllText(configJsonFile);
                }

                return string.Empty;
            }
        }

        public IDictionary<string, object> Variables
        {
            get
            {
                var variablesJsonFile = Path.Combine(Path.GetDirectoryName(this._fileName), "variables.json");
                if (File.Exists(variablesJsonFile))
                {
                    return JsonConvert.DeserializeObject<IDictionary<string, object>>(File.ReadAllText(variablesJsonFile));
                }

                return new Dictionary<string, object>();
            }
        }

        public async Task Execute(bool isVerify)
        {
            await new TaskFactory().StartNew(() =>
            {
                try
                {
                    this.TraceMessages.Clear();
                    this.TestStatus = TestStatus.Running;
                    var executor = new JsonExecutor(this.Data, this.ConfigJson, TraceAction);
                    if (isVerify)
                    {
                        executor.ExecuteAndVerify(this.Variables);
                        this.ResultsData = "Success";
                    }
                    else
                    {
                        var output = executor.Execute(this.Variables);
                        this.ResultsData = JsonConvert.SerializeObject(output);
                    }
                    this.TestStatus = TestStatus.Success;
                }
                catch (AssertionFailedException ae)
                {
                    this.TestStatus = TestStatus.Error;
                    this.ResultsData = ae.Message;      // good with message.
                }
                catch (Exception e)
                {
                    this.TestStatus = TestStatus.Error;
                    this.ResultsData = e.ToString();
                }
                finally
                {
                    OnPropertyChanged(()=> this.ResultsData);
                    OnPropertyChanged(() => this.TestStatus);
                }
            });
        }

        private void TraceAction(ExecuteTraceInfo traceInfo)
        {
            ExecuteAsync(() => { this.TraceMessages.Add(new MethodTreeViewModel(traceInfo)); });
        }
    }
}
