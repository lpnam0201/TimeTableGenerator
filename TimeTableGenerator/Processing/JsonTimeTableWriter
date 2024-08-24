using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using TimeTableGenerator.Data;

namespace TimeTableGenerator.Processing
{
    public class JsonTimeTableWriter : ITimeTableWriter
    {
        public void Write(string fileName, IList<Occurrence> occurrences, Options options)
        {
            var text = JsonSerializer.Serialize(occurrences, options: new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            });

            File.WriteAllText($"{fileName}.json", text, Encoding.UTF8);
        }
        
    }
}
