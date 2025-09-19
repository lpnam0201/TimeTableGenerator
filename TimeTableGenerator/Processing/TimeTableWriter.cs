using ClosedXML.Excel;
using TimeTableGenerator.Data;

namespace TimeTableGenerator.Processing
{
    public class TimeTableWriter : ITimeTableWriter
    {
        public void Write(string fileName, string directory, IList<Occurrence> occurrences, Options options)
        {
            occurrences = FilterOccurrenceByDiscussionGroup(occurrences, options);

            using (var workbook = new XLWorkbook())
            {
                var occurrencesByWeek = occurrences.GroupBy(x => x.Week)
                    .OrderBy(x => x.Key);

                foreach (var grouping in occurrencesByWeek)
                {
                    var sheetName = $"Tuần {grouping.Key}";
                    var worksheet = workbook.AddWorksheet(sheetName);

                    SetHeaders(worksheet);
                    SetPeriodLabels(worksheet);
                    SetOccurrencesOfWeek(worksheet, grouping.ToList());

                    if (options.GroupPeriods)
                    {
                        GroupPeriodLabels(worksheet);
                        GroupOccurrencesOfWeek(worksheet);
                        SetFixedWidthForColumns(worksheet);
                    }
                    else
                    {
                        worksheet.Columns().AdjustToContents();
                    }

                    Paint(worksheet);
                }
                var path = Path.Combine(directory, $"{fileName}.xlsx");
                workbook.SaveAs(path);
            }
        }

        private void Paint(IXLWorksheet worksheet)
        {
            //throw new NotImplementedException();
        }

        private void GroupOccurrencesOfWeek(IXLWorksheet worksheet)
        {
            foreach (var weekdayColumn in Constants.WeekdayColumns)
            {
                var column = weekdayColumn.Item2;
                var rowEnd = Constants.PeriodRowStart + Constants.PeriodTimes.Count;

                var cells = worksheet.Cells($"{column}{Constants.PeriodRowStart}:{column}{rowEnd}");
                var cellValues = cells.Select(x => x.Value.ToString()).ToList();

                var groups = Utils.FindRunsOfSameString(cellValues);
                foreach (var group in groups)
                {
                    if (string.IsNullOrEmpty(group.Item1))
                    {
                        continue;
                    }

                    var text = group.Item1;

                    var firstOccurrenceRow = Constants.PeriodRowStart + group.Item2;
                    var lastOccurrenceRow = Constants.PeriodRowStart + group.Item3;

                    var merged = worksheet
                        .Range($"{column}{firstOccurrenceRow}:{column}{lastOccurrenceRow}")
                        .Merge();
                    merged.Value = text;
                    merged.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    merged.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                }
            }
        }

        private void GroupPeriodLabels(IXLWorksheet worksheet)
        {
            var periodColumn = "B";

            for (var i = 0; i < Constants.PeriodTimes.Count; i += 2)
            {
                var firstPeriod = Constants.PeriodTimes[i];
                var secondPeriod = Constants.PeriodTimes[i + 1];

                var startFirstPeriod = firstPeriod.Item2;
                var endSecondPeriod = secondPeriod.Item2.Add(Constants.TimePerPeriod);

                var shiftNumber = i / 2 + 1;
                var text = $"Ca {shiftNumber} ({startFirstPeriod.ToPeriodTimeLabel()} - {endSecondPeriod.ToPeriodTimeLabel()})";

                var merged = worksheet
                    .Range($"{periodColumn}{Constants.PeriodRowStart + i}:{periodColumn}{Constants.PeriodRowStart + i + 1}")
                    .Merge();
                merged.Value = text;
                merged.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                merged.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            }
        }

        private void SetHeaders(IXLWorksheet worksheet)
        {
            var headerRow = 2;
            var periodCell = worksheet.Cell(headerRow, "B");
            periodCell.Value = "Tiết";
            ApplyHeaderStyle(periodCell);
            //periodCell.Style.Fill.SetBackgroundColor(XLColor.Green);

            var weekdayNumbers = new List<ValueTuple<string, char>>();
            var weekdayNameStart = 2;
            for (var i = 0; i <= 5; i++)
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

                //weekdayCell.Style.Fill.SetBackgroundColor(XLColor.Green);
            }

            //var roomCell = worksheet.Cell(headerRow, "H");
            //roomCell.Value = "Phòng";
            //ApplyHeaderStyle(roomCell);
        }

        private void SetPeriodLabels(IXLWorksheet worksheet)
        {
            var periodColumn = "B";

            for (var i = 0; i < Constants.PeriodTimes.Count; i++)
            {
                var periodTimeData = Constants.PeriodTimes[i];
                var startTime = periodTimeData.Item2;
                var endTime = startTime.Add(Constants.TimePerPeriod);

                var timeText = $"{startTime.ToPeriodTimeLabel()} - {endTime.ToPeriodTimeLabel()}";
                var text = $"Tiết {periodTimeData.Item1}({timeText})";

                var periodCell = worksheet.Cell(Constants.PeriodRowStart + i, periodColumn);
                periodCell.Value = text;
                periodCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                //periodCell.Style.Fill.SetBackgroundColor(XLColor.LightGreen);
            }

            var morningCell = worksheet.Cell(3, "A");
            morningCell.Value = "Sáng";
            morningCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            morningCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            //morningCell.Style.Fill.SetBackgroundColor(XLColor.LightGreen);
            worksheet.Range("A3:A8").Merge();

            var afternoonCell = worksheet.Cell(9, "A");
            afternoonCell.Value = "Chiều";
            afternoonCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            afternoonCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            //afternoonCell.Style.Fill.SetBackgroundColor(XLColor.LightGreen);
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
                //cell.Style.Fill.SetBackgroundColor(XLColor.LightGreen);
            }
        }

        private void ApplyHeaderStyle(IXLCell cell)
        {
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            cell.Style.Font.SetBold();
        }

        private void SetFixedWidthForColumns(IXLWorksheet worksheet)
        {
            worksheet.Column(Constants.PeriodWriteColumn).Width = 20;

            var firstWeekdayColumn = Constants.WeekdayColumns.First().Item2;
            var lastWeekdayColumn = Constants.WeekdayColumns.Last().Item2;
            worksheet.Columns($"{firstWeekdayColumn}:{lastWeekdayColumn}").Width = 55;
        }

        private IList<Occurrence> FilterOccurrenceByDiscussionGroup(IList<Occurrence> occurrences, Options options)
        {
            if (string.IsNullOrEmpty(options.DiscussionGroup))
            {
                return occurrences;
            }
            
            return occurrences
                .Where(x => string.IsNullOrEmpty(x.DiscussionGroup) || x.DiscussionGroup == options.DiscussionGroup)
                .ToList();
        }
    }
}
