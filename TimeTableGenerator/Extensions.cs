using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTableGenerator
{
    public static class Extensions
    {
        public static int ToArrayIndex(this int excelIndex)
        {
            return excelIndex - 1;
        }

        public static object GetCellValue(this object?[] itemArray, string columnName)
        {
            var excelIndex = Utils.ExcelColumnToIndex(columnName);
            if (excelIndex.ToArrayIndex() >= itemArray.Length)
            {
                return null;
            }
            return itemArray[excelIndex.ToArrayIndex()];
        }

        public static string ToPeriodTimeLabel(this TimeOnly time)
        {
            return $"{time.Hour}h:{time.Minute.ToString("00")}";
        }
    }
}
