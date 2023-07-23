using CommandLine;
using ExcelDataReader;
using System.Data;
using System.Linq;
using System.Text;
using TimeTableGenerator.Data;
using TimeTableGenerator.Processing;

namespace TimeTableGenerator
{
    public class Program
    {
        static void Main(string[] args)
        {
            EnsureCodePage1252Registered();
            var options = ReadOptions(args);

            var sheet = ReadSheet(options);
            var occurrences = new TimeTableParser().ParseOccurrences(sheet);

        }

        private static DataTable ReadSheet(Options options)
        {
            DataTableCollection dataTableCollection;
            using (var stream = File.Open(options.FilePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();
                    dataTableCollection = result.Tables;
                }
            }

            var sheet = dataTableCollection[options.SheetName];
            return sheet;
        }

        private static void EnsureCodePage1252Registered()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        private static Options ReadOptions(string[] args)
        {
            return Parser.Default.ParseArguments<Options>(args).Value;
        }
    }
}