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

        // Column
        public const string OrdinalNumberColumn = "A"; // Số thứ tự

        public const string SubjectId = "Lớp học phần";
        public const string SubjectName = "Tên học phần"; 
        public const string SubjectType = "Loại HP";
        public const string Weekday = "Thứ";
        public const string Period = "Tiết";
        public const string Room = "Phòng";
        public const string Week = "Tuần học";

        public static IList<string> Headers = new List<string>
        {
            SubjectId,
            SubjectName,
            SubjectType,
            Weekday,
            Period,
            Room,
            Week
        };

        // Patterns
        public const char WeekEmptyValue = '_';
        public const char PeriodEmptyValue = '_';

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
                ("Thứ Bảy", "H")
            };
        public const int PeriodRowStart = 3;
        public const char WeekDayColumnStart = 'C';
        public static readonly TimeSpan TimePerPeriod = new TimeSpan(0, 50, 0);
        public const string PeriodWriteColumn = "B";

        public const string LightGreen = "E2F0D9";
        public const string Green = "70AD47";
    }
}
