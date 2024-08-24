using CommandLine;

namespace TimeTableGenerator
{
    public class Options
    {
        [Option("FilePath", Required = true)]
        public string FilePath { get; set; }
        [Option("SheetName", Required = true)]
        public string SheetName { get; set; }
        [Option("DiscussionGroup", Required = false)]
        public string DiscussionGroup { get; set; }
        [Option("GroupPeriods", Default = false)]
        public bool GroupPeriods { get; set; }


        [Option("WriteMode", Default = WriteMode.Excel)]
        public WriteMode WriteMode {get;set;}
    }
}
