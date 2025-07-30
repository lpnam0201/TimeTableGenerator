using ClosedXML.Excel;
using CommandLine;
using DocumentFormat.OpenXml.Packaging;
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

            switch (options.ProcessMode)
            {
                case ProcessMode.One:
                    ProcessOne(options);
                    break;
                case ProcessMode.All:
                    ProcessAll(options);
                    break;
                case ProcessMode.Summer:
                    ProcessSummer(options);
                    break;
                default:
                    break;
            }
            
        }

        private static void ProcessSummer(Options options)
        {
            if (string.IsNullOrEmpty(options.SummerClasses))
            {
                throw new ArgumentNullException("SummerClasses is not specified");
            }

            var dataTable = ReadDataTable(options);
            var summerClasses = options.SummerClasses.Split(",");
            var result = new List<Occurrence>();
            foreach (var clazz in summerClasses)
            {
                var sheet = dataTable[clazz];
                var occurrences = new TimeTableParser().ParseOccurrences(sheet, options);
                result.AddRange(occurrences);
            }
            WriteResult("result_summer", Directory.GetCurrentDirectory(), result, options);
        }

        private static void ProcessAll(Options options)
        {
            var dataTables = ReadDataTable(options);
            foreach (DataTable sheet in dataTables)
            {
                var sheetName = sheet.TableName;
                var occurrences = new TimeTableParser().ParseOccurrences(sheet, options);

                var resultFolder = Path.Combine(Directory.GetCurrentDirectory(), "results");
                WriteResult(sheetName, resultFolder, occurrences, options);
            }


        }

        private static DataTableCollection ReadDataTable(Options options)
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

            return dataTableCollection;
        }

        private static void ProcessOne(Options options)
        {
            var dataTable = ReadDataTable(options);
            var sheet = dataTable[options.SheetName];
            var occurrences = new TimeTableParser().ParseOccurrences(sheet, options);

            WriteResult("result", Directory.GetCurrentDirectory(), occurrences, options);
        }

        private static void WriteResult(string fileName, string directory, IList<Occurrence> occurrences, Options options)
        {
            ITimeTableWriter writer;
            switch (options.WriteMode)
            {
                case WriteMode.Excel:
                    writer = new TimeTableWriter();
                    break;
                case WriteMode.Json:
                    writer = new JsonTimeTableWriter();
                    break;
                default:
                    throw new NotSupportedException("Unknown write mode");
            }
            writer.Write(fileName, directory, occurrences, options);
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
