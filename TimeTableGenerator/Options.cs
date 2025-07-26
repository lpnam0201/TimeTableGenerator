using CommandLine;

namespace TimeTableGenerator
{
    public class Options
    {
        [Option("FilePath", Required = true)]
        public string FilePath { get; set; }
        [Option("SheetName")]
        public string SheetName { get; set; }
        [Option("DiscussionGroup")]
        public string DiscussionGroup { get; set; }
        [Option("GroupPeriods", Default = false)]
        public bool GroupPeriods { get; set; }


        [Option("WriteMode", Default = WriteMode.Excel)]
        public WriteMode WriteMode {get;set;}
        [Option("ProcessAll", Default = false)]
        public bool IsProcessAll { get; set; }
    }
}
