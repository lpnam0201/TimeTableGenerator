using DocumentFormat.OpenXml.Spreadsheet;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTableGenerator.Data;

namespace TimeTableGenerator.Processing
{
    public class TimeTableParser
    {
        public IList<Occurrence> ParseOccurrences(DataTable sheet, Options options)
        {
            var occurrences = new List<Occurrence>();

            var lastDataRow = FindLastDataRow(sheet);
            var dataStartRow = FindDataStartRow(sheet);
            var headerLookUp = BuildHeaderLookUp(sheet, dataStartRow);
            for (int i = FindDataStartRow(sheet); i <= lastDataRow; i++)
            {
                var row = sheet.Rows[i];
                var occurrenceOfRow = ParseRowOccurrences(row, headerLookUp, options);
                occurrences.AddRange(occurrenceOfRow);
            }

            return occurrences;
        }

        private IDictionary<string, string> BuildHeaderLookUp(DataTable sheet, int dataStartRow)
        {
            var lookup = new Dictionary<string, string>();
            var headerRow = dataStartRow - 1;
            var row = sheet.Rows[headerRow];
            var columnNames = Enumerable.Range('A', 'Z' - 'A' + 1)
                .Select(x => ((char)x).ToString());
            foreach (var column in columnNames)
            {
                var headerValue = row.ItemArray.GetCellValue(column).ToString();
                var matchedHeader = Constants.Headers.FirstOrDefault(x => headerValue.Contains(x));

                if (!string.IsNullOrEmpty(headerValue) && matchedHeader != null)
                {
                    lookup.Add(matchedHeader, column);
                }    
            }

            return lookup;
        }

        private int FindDataStartRow(DataTable sheet)
        {
            var i = 0;
            do
            {
                var index = 1 + i;
                var dataRow = sheet.Rows[index];
                var ordinalNumber = dataRow.ItemArray.GetCellValue(Constants.OrdinalNumberColumn);
                if (ordinalNumber.ToString() == "1")
                {
                    return index;
                }
                i++;
            } while (true);
        }

        private IList<Occurrence> ParseRowOccurrences(DataRow row, IDictionary<string, string> headerLookup, Options options)
        {
            var occurrences = new List<Occurrence>();

            var subjectId = row.ItemArray.GetCellValue(
                headerLookup[Constants.SubjectId]).ToString();
            var subjectName = row.ItemArray.GetCellValue(
                headerLookup[Constants.SubjectName]).ToString();
            var subjectType = row.ItemArray.GetCellValue(
                headerLookup[Constants.SubjectType]).ToString();
            var room = row.ItemArray.GetCellValue(
                headerLookup[Constants.Room]).ToString();
            var weekday = row.ItemArray.GetCellValue(
                headerLookup[Constants.Weekday]).ToString();
            string periodsPattern = row.ItemArray.GetCellValue(
                headerLookup[Constants.Period]).ToString();
            string weeksPattern = row.ItemArray.GetCellValue(
                headerLookup[Constants.Week]).ToString();

            var weeks = ParseWeeks(weeksPattern);
            var periods = ParsePeriods(periodsPattern);
            foreach (var week in weeks)
            {
                foreach (var period in periods)
                {
                    var occurrence = new Occurrence
                    {
                        SubjectId = subjectId,
                        SubjectName = subjectName,
                        SubjectType = subjectType,
                        Room = room,
                        Weekday = weekday,
                        Period = period,
                        Week = week,
                    };

                    occurrences.Add(occurrence);
                }
            }

            return occurrences;
        }

        private IList<int> ParseWeeks(string weekPattern)
        {
            return Utils.ParsePatternToInts(weekPattern, Constants.WeekEmptyValue);
        }

        private IList<int> ParsePeriods(string periodPattern)
        {
            return Utils.ParsePatternToInts(periodPattern, Constants.PeriodEmptyValue);
        }

        private int FindLastDataRow(DataTable sheet)
        {
            var index = 0;
            string ordinalNumberValue = null;
            string nextRowValue = null;

            do
            {
                var cellIndex = Utils.ExcelColumnToIndex(Constants.OrdinalNumberColumn).ToArrayIndex();
                ordinalNumberValue = sheet.Rows[index].ItemArray[cellIndex].ToString();
                nextRowValue = sheet.Rows[index + 1].ItemArray[cellIndex].ToString();

                // Current cell is number but next row is not -> exit
                if (int.TryParse(ordinalNumberValue, out _) && !int.TryParse(nextRowValue, out _))
                {
                    break;
                }
                index++;
            } while (true);

            return index;
        }
    }
}
