using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTableGenerator
{
    // Defined exactly as seen in Excel sheet
    // Later code will convert to base-0 array index
    internal class Constants
    {
        // Row (Excel starts at 1)
        public const int DataStartRow = 14;

        // Column
        public const string OrdinalNumberColumn = "A"; // Số thứ tự
        public const string SubjectIdColumn = "F"; // Lớp học phần
        public const string DiscussionGroupColumn = "H"; // Lớp học phần
        public const string SubjectNameColumn = "J"; // Tên học phần
        public const string SubjectTypeColumn = "N"; // Loại học phần
        public const string WeekdayColumn = "O"; // Thứ
        public const string PeriodColumn = "P"; // Tiết
        public const string RoomColumn = "U"; // Phòng
        public const string WeekColumn = "Z"; // Tuần

        // Patterns
        public const char WeekEmptyValue = '_';
        public const char PeriodEmptyValue = '-';

        // Write
        public static readonly IReadOnlyList<ValueTuple<int, TimeOnly>> PeriodTimes 
            = new List<ValueTuple<int, TimeOnly>>()
            {
                (1, new TimeOnly(6, 40, 0)),
                (2, new TimeOnly(7, 30, 0)),
                (3, new TimeOnly(8, 30, 0)),
                (4, new TimeOnly(9, 40, 0)),
                (5, new TimeOnly(10, 10, 0)),
                (6, new TimeOnly(11, 00, 0)),
                (7, new TimeOnly(12, 40, 0)),
                (8, new TimeOnly(13, 30, 0)),
                (9, new TimeOnly(14, 30, 0)),
                (10, new TimeOnly(15, 20, 0)),
                (11, new TimeOnly(16, 20, 0)),
                (12, new TimeOnly(17, 10, 0))
            };
        public static readonly IReadOnlyList<ValueTuple<string, string>> WeekdayColumns
            = new List<ValueTuple<string, string>>()
            {
                ("Thứ Hai", "C"),
                ("Thứ Ba", "D"),
                ("Thứ Tư", "E"),
                ("Thứ Năm", "F"),
                ("Thứ Sáu", "G"),
            };
        public const int PeriodRowStart = 3;
        public const char WeekDayColumnStart = 'C';
        public static readonly TimeSpan TimePerPeriod = new TimeSpan(0, 50, 0);
        public const string PeriodWriteColumn = "B";

        public const string LightGreen = "E2F0D9";
        public const string Green = "70AD47";
    }
}
