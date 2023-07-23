using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTableGenerator
{
    public static class Utils
    {
        public static int ExcelColumnToIndex(string columnName)
        {
            int number = 0;
            int pow = 1;
            for (int i = columnName.Length - 1; i >= 0; i--)
            {
                var diffToA = columnName[i] - 'A';
                number += (diffToA + 1) * pow;
                pow *= 26;
            }

            // Array index starts at 0
            return number;
        }

        public static IList<int> ParsePatternToInts(string valuesStr, char emptyValue)
        {
            var result = new List<int>();

            var values = valuesStr.ToCharArray();

            var valueAndIndexTuples = values.Select((value, index) => new { Index = index, Value = value });
            var filterEmpty = valueAndIndexTuples.Where(tuple => tuple.Value != emptyValue);
            var toRealWorldInts = filterEmpty.Select(tuple => tuple.Index + 1);

            return toRealWorldInts.ToList();
        }
    }
}
