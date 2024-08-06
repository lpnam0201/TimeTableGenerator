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
            for (int i = Constants.DataStartRow.ToArrayIndex(); i <= lastDataRow; i++)
            {
                var row = sheet.Rows[i];
                var occurrenceOfRow = ParseRowOccurrences(row, options);
                occurrences.AddRange(occurrenceOfRow);
            }

            return occurrences;
        }

        private IList<Occurrence> ParseRowOccurrences(DataRow row, Options options)
        {
            var occurrences = new List<Occurrence>();

            var subjectId = row.ItemArray.GetCellValue(Constants.SubjectIdColumn).ToString();
            var subjectName = row.ItemArray.GetCellValue(Constants.SubjectNameColumn).ToString();
            var subjectType = row.ItemArray.GetCellValue(Constants.SubjectTypeColumn).ToString();
            var room = row.ItemArray.GetCellValue(Constants.RoomColumn).ToString();
            var discussionGroup = !string.IsNullOrEmpty(options.DiscussionGroup)
                ? row.ItemArray.GetCellValue(Constants.DiscussionGroupColumn).ToString()
                : null;
            var weekday = row.ItemArray.GetCellValue(Constants.WeekdayColumn).ToString();
            string periodsPattern = row.ItemArray.GetCellValue(Constants.PeriodColumn).ToString();
            string weeksPattern = row.ItemArray.GetCellValue(Constants.WeekColumn).ToString();

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
                        DiscussionGroup = discussionGroup,
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
            var index = Constants.DataStartRow.ToArrayIndex();
            string ordinalNumberValue = null;

            do
            {
                var cellIndex = Utils.ExcelColumnToIndex(Constants.OrdinalNumberColumn).ToArrayIndex();
                ordinalNumberValue = sheet.Rows[index].ItemArray[cellIndex].ToString();

                index++;
            } while (!string.IsNullOrEmpty(ordinalNumberValue));

            var lastIndexWithData = index - 1;
            return lastIndexWithData;
        }
    }
}
