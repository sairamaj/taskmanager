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
    /// <summary>
    /// Test file data view model.
    /// </summary>
    public class TestFileDataViewModel : CoreViewModel
    {
        /// <summary>
        /// Test file name.
        /// </summary>
        private readonly string _fileName;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestFileDataViewModel"/> class.
        /// </summary>
        /// <param name="name">
        /// Test name.
        /// </param>
        /// <param name="fileName">
        /// Test file data.
        /// </param>
        public TestFileDataViewModel(string name, string fileName)
        {
            this.Name = name;
            this._fileName = fileName;
            if (File.Exists(fileName))
            {
                this.Data = File.ReadAllText(fileName);
            }
            this.TraceMessages = new SafeObservableCollection<TreeViewItemViewModel>();
            this.RunTestFileCommand = new DelegateCommand(async () => { await this.Execute(false); });
            this.RunTestFileWithVerifyCommand = new DelegateCommand(async () => { await this.Execute(true); });
            this.TestStatus = TestStatus.None;
        }

        /// <summary>
        /// Gets test name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets test data.
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Gets or sets result data.
        /// </summary>
        public string ResultsData { get; set; }

        /// <summary>
        /// Gets or sets test status.
        /// </summary>
        public TestStatus TestStatus { get; set; }

        /// <summary>
        /// Gets run test file command.
        /// </summary>
        public ICommand RunTestFileCommand { get; }

        /// <summary>
        /// Gets run test file with verify command.
        /// </summary>
        public ICommand RunTestFileWithVerifyCommand { get; }

        /// <summary>
        /// Gets trace messages.
        /// </summary>
        public ObservableCollection<TreeViewItemViewModel> TraceMessages { get; }

        /// <summary>
        /// Gets configuration JSON file data.
        /// </summary>
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

        /// <summary>
        /// Gets variables associated with this data.
        /// </summary>
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

        /// <summary>
        /// Executes the test.
        /// </summary>
        /// <param name="isVerify">
        /// true for verifying the results with expected one.
        /// </param>
        /// <returns>
        /// Task instance.
        /// </returns>
        public async Task Execute(bool isVerify)
        {
            await new TaskFactory().StartNew(() =>
            {
                try
                {
                    this.TraceMessages.Clear();
                    this.TestStatus = TestStatus.Running;
                    var executor = new JsonExecutor(this.Data, this.ConfigJson, this.TraceAction);
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
                    this.OnPropertyChanged(() => this.ResultsData);
                    this.OnPropertyChanged(() => this.TestStatus);
                }
            });
        }

        /// <summary>
        /// Trace action.
        /// </summary>
        /// <param name="traceInfo">
        /// A <see cref="ExecuteTraceInfo"/> instance.
        /// </param>
        private void TraceAction(ExecuteTraceInfo traceInfo)
        {
            ExecuteAsync(() =>
            {
                if (traceInfo.TraceType == TraceType.Verification)
                {
                    this.TraceMessages.Add(new VerificationViewModel(traceInfo));
                }
                else
                {
                    this.TraceMessages.Add(new MethodViewTreeViewModel(traceInfo));
                }
            });
        }
    }
}