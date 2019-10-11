using System.IO;

namespace JsonExecutorTasks.ViewModel
{
    public class TestFileDataViewModel
    {
        private readonly string _fileName;

        public TestFileDataViewModel(string name, string fileName)
        {
            Name = name;
            if (File.Exists(fileName))
            {
                this.Data = File.ReadAllText(fileName);
            }
        }

        public string Name { get; }
        public string Data { get; set; }
    }
}
