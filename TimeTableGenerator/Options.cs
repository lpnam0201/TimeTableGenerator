using CommandLine;

namespace TimeTableGenerator
{
    public class Options
    {
        [Option("FilePath", Required = true)]
        public string FilePath { get; set; }
        [Option("SheetName", Required = true)]
        public string SheetName { get; set; }
    }
}
