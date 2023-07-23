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
    }
}
