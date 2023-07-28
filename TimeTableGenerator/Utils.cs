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

        public static IList<(string, int, int)> FindRunsOfSameString(IList<string> values)
        {
            var result = new List<(string, int, int)>();
            if (values.Count == 0)
            {
                return result;
            }

            var begin = 0;
            var rangeValue = values[0];

            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] != rangeValue)
                {
                    result.Add((rangeValue, begin, i - 1));

                    rangeValue = values[i];
                    begin = i;
                }

                // Reached end, conclude range
                if (i == values.Count - 1)
                {
                    result.Add((rangeValue, begin, i));
                }
            }

            return result;
        }
    }
}
