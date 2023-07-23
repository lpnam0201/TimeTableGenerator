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
            return itemArray[excelIndex.ToArrayIndex()];
        }
    }
}
