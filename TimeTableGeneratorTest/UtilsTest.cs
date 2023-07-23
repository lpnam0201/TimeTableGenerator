using NUnit.Framework;
using System.Collections;
using TimeTableGenerator;

namespace TimeTableGeneratorTest
{
    [TestFixture]
    public class UtilTests
    {
        [Test]
        [TestCase("A", 1)]
        [TestCase("Z", 26)]
        [TestCase("AA", 27)]
        public void ExcelColumnToIndex__ShouldConvertCorrectly(string columnName, int index)
        {
            var actualIndex = Utils.ExcelColumnToIndex(columnName);

            Assert.AreEqual(index, actualIndex);
        }

        [Test]
        [TestCaseSource(nameof(ParsePatternToInts_TestData))]
        public void ParsePatternToInts__ShouldReturnCorrectly(string pattern, IList<int> expectedInts)
        {
            var actualInts = Utils.ParsePatternToInts(pattern, '_');

            CollectionAssert.AreEqual(expectedInts, actualInts);
        }

        private static IEnumerable ParsePatternToInts_TestData()
        {
            yield return new TestCaseData(
                "123456789012345678___",
                new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 });
            yield return new TestCaseData(
                "_________0___________",
                new List<int> { 10 });
            yield return new TestCaseData(
                "___4_67______________",
                new List<int> { 4, 6, 7});
        }
    }
}