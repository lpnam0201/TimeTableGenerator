using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTableGenerator.Data;

namespace TimeTableGenerator.Processing
{
    public class TimeTableWriter
    {
        public void Write(string savePath, IList<Occurrence> occurrences)
        {
            using (var workbook = new XLWorkbook())
            {
                var occurrencesByWeek = occurrences.GroupBy(x => x.Week);

                foreach (var grouping in occurrencesByWeek)
                {
                    var sheetName = $"Tuần {grouping.Key}";
                    var worksheet = workbook.AddWorksheet(sheetName);

                    SetHeaders(worksheet);
                    SetPeriodLabels(worksheet);
                    SetOccurrencesOfWeek(worksheet, grouping.ToList());
                    worksheet.Columns().AdjustToContents();
                }

                workbook.SaveAs(savePath);
            }
        }

        private void SetHeaders(IXLWorksheet worksheet)
        {
            var headerRow = 2;
            var periodCell = worksheet.Cell(headerRow, "B");
            periodCell.Value = "Tiết";
            ApplyHeaderStyle(periodCell);

            var weekdayNumbers = new List<ValueTuple<string, char>>();
            var weekdayNameStart = 2;
            for (var i = 0; i <= 4; i++)
            {
                var weekdayName = (weekdayNameStart + i).ToString();
                var weekdayColumn = (char)(Constants.WeekDayColumnStart + i);

                weekdayNumbers.Add((weekdayName, weekdayColumn));
            }

            foreach (var weekdayNumber in weekdayNumbers)
            {
                var column = weekdayNumber.Item2.ToString();
                var weekdayCell = worksheet.Cell(headerRow, column);
                weekdayCell.Value = $"Thứ {weekdayNumber.Item1}";
                ApplyHeaderStyle(weekdayCell);
            }

            var roomCell = worksheet.Cell(headerRow, "H");
            roomCell.Value = "Phòng";
            ApplyHeaderStyle(roomCell);
        }

        private void SetPeriodLabels(IXLWorksheet worksheet)
        {
            var periodColumn = "B";

            var timePerPeriod = new TimeSpan(0, 50, 0);

            for (var i = 0; i < Constants.PeriodTimes.Count; i++)
            {
                var periodTimeData = Constants.PeriodTimes[i];
                var startTime = periodTimeData.Item2;
                var endTime = startTime.Add(timePerPeriod);

                var timeText = $"{startTime.ToPeriodTimeLabel()} - {endTime.ToPeriodTimeLabel()}";
                var text = $"Tiết {periodTimeData.Item1}({timeText})";

                var periodCell = worksheet.Cell(Constants.PeriodRowStart + i, periodColumn);
                periodCell.Value = text;
                periodCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            }

            var morningCell = worksheet.Cell(3, "A");
            morningCell.Value = "Sáng";
            morningCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            morningCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range("A3:A8").Merge();

            var afternoonCell = worksheet.Cell(9, "A");
            afternoonCell.Value = "Chiều";
            afternoonCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            afternoonCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range("A9:A14").Merge();
        }

        private void SetOccurrencesOfWeek(IXLWorksheet worksheet, IList<Occurrence> occurrencesOfWeek)
        {
            foreach (var occurrence in occurrencesOfWeek)
            {
                var column = Constants.WeekdayColumns.First(x => x.Item1 == occurrence.Weekday).Item2;
                var row = Constants.PeriodRowStart + occurrence.Period - 1;

                var cell = worksheet.Cell(row, column);
                var text = $"{occurrence.SubjectName} ({occurrence.Room})";
                cell.Value = text;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            }
        }

        private void ApplyHeaderStyle(IXLCell cell)
        {
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            cell.Style.Font.SetBold();
        }
    }
}
