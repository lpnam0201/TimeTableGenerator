using TimeTableGenerator.Data;

namespace TimeTableGenerator.Processing
{
    public interface ITimeTableWriter
    {
        void Write(string fileName, string directory, IList<Occurrence> occurrences, Options options);
        
    }
}
