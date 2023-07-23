using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTableGenerator.Data
{
    public class Occurrence
    {
        // Unique
        public string SubjectId { get; set; }
        // Nhiều học phần trùng tên nhưng khác Id
        public string SubjectName { get; set; }
        public string DiscussionGroup { get; set; }
        public string SubjectType { get; set; }
        // Thứ hai, thứ ba...
        public string Weekday { get; set; }
        public int Period { get; set; }
        public string Room { get; set; }
        public int Week { get; set; }
    }
}
